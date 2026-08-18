# H.Framework.Data.ORM 重构与优化方案

> 目标：在保持现有业务代码兼容的前提下，逐步将自研 ORM 升级为支持多数据库、单库分表、高性能映射的小型框架。
> 当前范围：先以 **单库分表** 为分片目标，分库与跨分片事务放到后续阶段。

---

## 一、项目现状

- 基于 ADO.NET + MySql.Data 8.4.0，目标框架 `netstandard2.1`。
- 通过 Lambda 表达式树解析生成 SQL。
- 使用 Attribute 标注表名、主键、列名、外键、明细表。
- `FoundationDAL` / `FoundationBLL` 存在 0~6 个外键表的泛型重载爆炸。
- 映射层每行都反射，未做元数据缓存。
- SQL 拼接大量硬编码 MySQL 语法。
- 静态连接字符串 `FoundationDAL.ConnectedString`。
- 主表 + 明细通过 LEFT JOIN 一次性查出，内存去重为 `O(N²)`。

---

## 二、总体目标

1. **性能**：查询映射提升 5~20 倍，大表分页与关联查询显著改善。
2. **结构**：消除泛型爆炸，SQL 构造职责清晰，代码量减少 50% 以上。
3. **扩展**：支持 MySQL / SQL Server / Oracle。
4. **分片**：支持单库水平分表。
5. **兼容**：旧 API 作为 facade 保留，内部逐步替换。

---

## 三、性能优化

### 3.1 表达式树解析层

#### 问题
- `ExpressionExtensions.cs` 600+ 行，0~5 外键表重载泛滥。
- 表达式合并直接 `Expression.And(expr.Body, newExpr.Body)`，参数引用易错。
- `MySQLUtility` 中大段 switch + 字符串拼接。

#### 方案
- 引入统一的 `SqlExpressionVisitor`，将表达式树解析为中间 AST `SqlParseResult`。
- 用 `ParameterReplacer` 统一替换参数，避免多表达式合并出错。
- 加 `ExpressionCache` 缓存常见表达式的解析结果。
- 用 `QuerySpec<T>` 替代 `WhereQueryable<T, T1, T2...>`。

```csharp
public class QuerySpec<T> where T : IFoundationModel, new()
{
    public Expression<Func<T, bool>> Predicate { get; set; }
    public List<OrderBy> OrderBy { get; } = new();
    public List<IncludeSpec> Includes { get; } = new();
    public int? Skip { get; set; }
    public int? Take { get; set; }
}
```

### 3.2 元数据缓存与编译映射

#### 问题
- `MySqlFabricate.Fill<T>` 每行反射 `typeof(T).GetProperties()`。
- `DataConvert.ToList` 每行线性扫描主表去重。
- `Activator.CreateInstance(typeof(Nullable<>), value)` 低效。

#### 方案
- 启动时构建 `EntityMetadataCache<T>`，缓存属性、列名、转换器、编译 setter。
- 用 `Expression` 编译 `PropertyInfo.SetValue` 为委托：

```csharp
public static Action<object, object> CompileSetter(PropertyInfo property)
{
    var instance = Expression.Parameter(typeof(object), "instance");
    var value = Expression.Parameter(typeof(object), "value");
    var body = Expression.Assign(
        Expression.Property(Expression.Convert(instance, property.DeclaringType), property),
        Expression.Convert(value, property.PropertyType));
    return Expression.Lambda<Action<object, object>>(body, instance, value).Compile();
}
```

- 可空类型统一用 `Nullable.GetUnderlyingType` + 直接赋值，避免反射创建。
- 主表/子表去重全部改为 `Dictionary<TKey, T>`，O(1) 查找。

### 3.3 ADO.NET 查询优化

#### 问题
- 同步 `GetListByReader<T>` 与异步 API 混存。
- 每条 SQL 新建连接，未使用 `Command Prepare`。
- `DataTable` 中间层导致全量内存加载。

#### 方案
- 删除同步 API 或标记 `[Obsolete]`，全链路异步。
- 优先使用 `DbDataReader` 流式映射，移除 `DataTable` 中间层。
- 高频查询启用 `cmd.Prepare()`。
- 引入 `IDbSession` / `IUnitOfWork`，在同一会话内复用连接和事务。
- 连接字符串从静态属性改为构造函数注入 `IDbConnectionFactory`。

### 3.4 分页优化

#### 问题
- 统一使用 `LIMIT offset, count`，深分页性能差。
- `pageNum` 默认值 0 与 1 混用，语义不一致。

#### 方案
- 单库分页：统一为 `skip/take` 语义。
- 深分页场景提供 Keyset Pagination：

```sql
WHERE ID > @lastId ORDER BY ID LIMIT @take
```

- 必须 OFFSET 时，使用延迟关联（deferred join）：

```sql
SELECT t.* FROM table t
JOIN (SELECT id FROM table ORDER BY id LIMIT @offset, @limit) tmp ON t.id = tmp.id;
```

### 3.5 主表 + 明细合并优化

#### 问题
- LEFT JOIN 一次性拉取主表重复行，网络传输量大。
- 内存去重为 `O(N²)`。
- 依赖 `TableMap` 字符串匹配，易出错。

#### 方案
提供两种模式：

**模式 A：SingleQuery（小数据量）**
- 保持一次 JOIN。
- 用 `DbDataReader` 流式读取。
- 主表对象用 `Dictionary<TKey, T>` 缓存。
- 子表对象用 `Dictionary<TKey, object>` 缓存，避免重复添加。

**模式 B：SplitQuery（大数据量）**
- 先查主表 ID 列表。
- 用 `WHERE ParentID IN (...)` 分批查子表。
- 按外键分组挂回主表。

引入结构化 `IncludePlan` 替代字符串 `include`：

```csharp
public class IncludePlan
{
    public string NavigationName { get; set; }
    public PropertyInfo ListProperty { get; set; }
    public Type ChildType { get; set; }
    public string ForeignKeyProperty { get; set; }
}
```

---

## 四、代码结构优化

### 4.1 新目录结构

```
H.Framework.Data.ORM/
├── Core/
│   ├── QuerySpec.cs
│   ├── IncludeSpec.cs
│   ├── OrderBy.cs
│   ├── ISqlDialect.cs
│   └── ISqlBuilder.cs
├── ExpressionParsing/
│   ├── SqlExpressionVisitor.cs
│   ├── ParameterReplacer.cs
│   ├── ExpressionEqualityComparer.cs
│   ├── ExpressionCache.cs
│   └── SqlParseResult.cs
├── Mapping/
│   ├── EntityMapper.cs
│   ├── EntityMetadata.cs
│   ├── PropertyMapping.cs
│   ├── NavigationMapping.cs
│   ├── MasterDetailMapper.cs
│   └── ValueConverter.cs
├── SqlBuilding/
│   ├── MySqlBuilder.cs
│   ├── SelectBuilder.cs
│   ├── WhereBuilder.cs
│   ├── InsertBuilder.cs
│   └── UpdateBuilder.cs
├── DataAccess/
│   ├── IDbConnectionFactory.cs
│   ├── IDbSession.cs
│   ├── DbSession.cs
│   └── IDbProviderFactory.cs
├── Sharding/
│   ├── IShardingStrategy.cs
│   ├── IShardingRouter.cs
│   ├── ISqlRewriter.cs
│   ├── ModuloShardingStrategy.cs
│   └── ShardTarget.cs
├── Attributes/          (精简后)
├── Foundations/         (旧代码，逐步废弃)
└── BaseDAL.cs / BaseBLL.cs (保留 facade)
```

### 4.2 消除泛型爆炸

- 用 `QuerySpec<T>` + `IncludeSpec` 取代 0~N 外键泛型重载。
- `FoundationDAL<TModel>` 只保留核心 CRUD，复杂查询走 `QuerySpec`。
- `BaseDAL` / `BaseBLL` 保留一层 facade，内部转发到新实现。

### 4.3 Attribute 映射优化

#### 当前问题
- `[DataFieldIgnore]` 与 `[DBIgnore]` 重复。
- `PrimaryKeyIDAttribute`、`DataFieldAttribute` 命名与主流 ORM 不一致。
- `IFoundationModel` 强制 `string ID`。

#### 改造方案
- 合并忽略 Attribute 为 `[NotMapped]`。
- 重命名对齐 EF Core：

| 旧名 | 新名 |
|---|---|
| `PrimaryKeyIDAttribute` | `KeyAttribute` |
| `DataFieldAttribute` | `ColumnAttribute` |
| `DataFieldIgnoreAttribute` / `DBIgnoreAttribute` | `NotMappedAttribute` |
| `DetailListAttribute` | `OneToManyAttribute` |
| `ForeignAttribute` | `ManyToOneAttribute` |
| `OnlyQueryAttribute` | `ReadOnlyAttribute` |

- 移除 `IFoundationModel` 强制 `string ID`，主键由 `[Key]` Attribute 标注。
- 引入泛型 `IRepository<T, TKey>`。
- 启动时扫描所有实体并校验：必须有且只有一个主键。

### 4.4 引入 Fluent API（混合模式）

简单映射继续用 Attribute，复杂/多变映射用 Fluent API 覆盖。

解析优先级：**Fluent > Attribute > Convention**。

```csharp
public interface IEntityTypeConfiguration<T>
{
    void Configure(EntityTypeBuilder<T> builder);
}

public class OrderMap : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(o => o.ID);
        builder.Property(o => o.ID).HasColumnName("OrderID");
        builder.HasMany(o => o.Items).WithOne().HasForeignKey(i => i.OrderID);
    }
}
```

---

## 五、多数据库支持

### 5.1 核心抽象

```csharp
public interface ISqlDialect
{
    string Name { get; }
    string QuoteIdentifier(string name);
    string ParameterPrefix { get; }
    DbParameter CreateParameter(string name, object value);
    string BuildPageSql(string selectSql, string orderBy, int skip, int take);
    string BuildGetIdentitySql(string tableName, string primaryKeyColumn);
    string GetCurrentTimestampSql();
    string GetConcatSql(params string[] args);
    string GetLengthSql(string expression);
    string GetIsNullSql(string expression, string defaultValue);
    DbType MapClrType(Type clrType);
}

public interface IDbProviderFactory
{
    DbConnection CreateConnection(string connectionString);
    DbCommand CreateCommand();
    DbParameter CreateParameter();
    DbDataAdapter CreateDataAdapter();
}
```

### 5.2 三种方言要点

| 特性 | MySQL | SQL Server | Oracle |
|---|---|---|---|
| 引用符 | `` `Name` `` | `[Name]` | `"NAME"` |
| 参数前缀 | `@` | `@` | `:` |
| 分页 | `LIMIT offset, count` | `OFFSET ... FETCH NEXT` | `OFFSET ... FETCH NEXT`（12c+） |
| 自增 ID | `LAST_INSERT_ID()` | `SCOPE_IDENTITY()` / `OUTPUT INSERTED` | Sequence / `RETURNING INTO` |
| 当前时间 | `NOW()` | `SYSUTCDATETIME()` | `SYSTIMESTAMP` |
| 字符串拼接 | `CONCAT(a,b)` | `a + b` | `a \|\| b` |
| 空值处理 | `IFNULL` | `ISNULL` | `NVL` |
| 布尔 | `tinyint(1)` | `bit` | `NUMBER(1)` |

### 5.3 关键改造点

- `MySqlFabricate` 改名为 `DbFabricate`，依赖 `DbConnection`/`DbCommand`。
- `BuilderSQLHelper` / `MySQLUtility` 全部通过 `ISqlDialect` 生成 SQL。
- 连接字符串配置支持多数据库 Provider。

### 5.4 推荐 NuGet 包

| 数据库 | 包 |
|---|---|
| MySQL | `MySql.Data` 或 `MySqlConnector` |
| SQL Server | `Microsoft.Data.SqlClient` |
| Oracle | `Oracle.ManagedDataAccess.Core` |

---

## 六、单库分表方案

> 本阶段先实现**单库水平分表**，分库与跨库事务放到后续阶段。

### 6.1 核心抽象

```csharp
public interface IShardingKeyResolver<T> where T : IFoundationModel
{
    object Resolve(T entity);
    object Resolve(Expression<Func<T, bool>> predicate);
}

public interface IShardingStrategy
{
    ShardingResult Route(object shardingKey, ShardingTopology topology);
}

public class ShardingResult
{
    public string DataSourceName { get; set; }
    public string PhysicalTableSuffix { get; set; }
}

public class ShardingTopology
{
    public string LogicTableName { get; set; }
    public int TableCount { get; set; }
}
```

### 6.2 分片策略

#### 取模分片

```csharp
public class ModuloShardingStrategy : IShardingStrategy
{
    public ShardingResult Route(object shardingKey, ShardingTopology topology)
    {
        var key = Convert.ToInt64(shardingKey);
        var index = (int)(key % topology.TableCount);
        return new ShardingResult
        {
            DataSourceName = topology.DefaultDataSource,
            PhysicalTableSuffix = index.ToString("D2")
        };
    }
}
```

#### 哈希分片

```csharp
public class HashShardingStrategy : IShardingStrategy
{
    public ShardingResult Route(object shardingKey, ShardingTopology topology)
    {
        var hash = shardingKey.GetHashCode();
        var index = Math.Abs(hash) % topology.TableCount;
        return new ShardingResult { PhysicalTableSuffix = index.ToString("D2") };
    }
}
```

#### 时间分片（如按月份）

```csharp
public class DateShardingStrategy : IShardingStrategy
{
    public ShardingResult Route(object shardingKey, ShardingTopology topology)
    {
        var date = (DateTime)shardingKey;
        return new ShardingResult
        {
            PhysicalTableSuffix = date.ToString("yyyyMM")
        };
    }
}
```

### 6.3 SQL 改写

```csharp
public interface ISqlRewriter
{
    string Rewrite(string sql, string logicTableName, string physicalTableName);
}

public class TableNameSqlRewriter : ISqlRewriter
{
    public string Rewrite(string sql, string logicTableName, string physicalTableName)
    {
        var patterns = new[]
        {
            $@"`{Regex.Escape(logicTableName)}`",
            $@"\[{Regex.Escape(logicTableName)}\]",
            $@"""{Regex.Escape(logicTableName)}""",
            $@"\b{Regex.Escape(logicTableName)}\b"
        };

        foreach (var pattern in patterns)
            sql = Regex.Replace(sql, pattern, physicalTableName, RegexOptions.IgnoreCase);

        return sql;
    }
}
```

### 6.4 单库分表集成到 DAL

```csharp
public virtual async Task<TModel> AddAsync(TModel model)
{
    model.ID = await _idGenerator.NewId();
    var shardingKey = _keyResolver.Resolve(model);
    var route = _router.RouteForWrite<TModel>(shardingKey);
    var physicalTable = $"{typeof(TModel).Name}_{route.PhysicalTableSuffix}";
    var sql = BuildInsertSql(model, physicalTable);
    return await _session.ExecuteAsync(sql, parameters);
}

public virtual async Task<IEnumerable<TModel>> GetListAsync(
    Expression<Func<TModel, bool>> predicate)
{
    var shardingKey = _keyResolver.TryExtractFromExpression(predicate);

    if (shardingKey != null)
    {
        var route = _router.RouteForRead<TModel>(shardingKey.Value);
        var physicalTable = $"{typeof(TModel).Name}_{route.PhysicalTableSuffix}";
        var sql = BuildSelectSql(predicate, physicalTable);
        return await _session.QueryAsync<TModel>(sql, parameters);
    }

    // 无分片键：广播到所有表
    var results = new List<TModel>();
    for (int i = 0; i < _topology.TableCount; i++)
    {
        var physicalTable = $"{typeof(TModel).Name}_{i:D2}";
        var sql = BuildSelectSql(predicate, physicalTable);
        results.AddRange(await _session.QueryAsync<TModel>(sql, parameters));
    }
    return results;
}
```

### 6.5 全局唯一 ID

分表后不能依赖数据库自增 ID，推荐 **Snowflake**：

```csharp
public interface IIdGenerator
{
    Task<long> NewId();
}

public class SnowflakeIdGenerator : IIdGenerator
{
    // 41 位时间戳 + 10 位机器码 + 12 位序列号
}
```

优点：
- 趋势递增，对 B+ 树友好。
- 64 位 long 型，可直接作为分片键。
- 本地生成，无数据库依赖。

### 6.6 跨分片查询（单库内）

单库内跨表查询可用 `UNION ALL` 优化：

```sql
SELECT * FROM Order_00 WHERE UserID = @userId
UNION ALL
SELECT * FROM Order_01 WHERE UserID = @userId
...
```

相比循环查询再内存合并，单库内 `UNION ALL` 让数据库做过滤和合并，通常更快。

封装：

```csharp
public string BuildCrossShardSelectSql<T>(Expression<Func<T, bool>> predicate, int tableCount)
{
    var parts = new List<string>();
    for (int i = 0; i < tableCount; i++)
    {
        var tableName = $"{typeof(T).Name}_{i:D2}";
        parts.Add(BuildSelectSql(predicate, tableName));
    }
    return string.Join("\nUNION ALL\n", parts);
}
```

### 6.7 单库分表配置

```json
{
  "Sharding": {
    "Tables": [
      {
        "LogicTableName": "Order",
        "ShardingColumn": "UserID",
        "Strategy": "Hash",
        "TableCount": 8,
        "DataSource": "default"
      }
    ]
  }
}
```

---

## 七、实施路线图

### Phase 1：基础性能优化（优先做，收益最大）

- [ ] 创建 `EntityMetadataCache<T>` + 编译 setter。
- [ ] 改造 `MySqlFabricate.Fill<T>`，使用缓存元数据映射。
- [ ] 主表/子表去重改为 `Dictionary<TKey, T>`。
- [ ] 移除 `DataTable` 中间层，改用 `DbDataReader` 流式读取。
- [ ] 把 `throw e` 改为 `throw`，引入 `ILogger` 替代 `Trace.WriteLine`。

### Phase 2：多数据库抽象

- [ ] 创建 `ISqlDialect` + `IDbProviderFactory`。
- [ ] 实现 `MySqlDialect` / `MySqlProviderFactory`，行为与现有 MySQL 一致。
- [ ] 改造 `DbFabricate` / `SQLBuilder` 依赖抽象接口。
- [ ] 验证 MySQL 功能完整。
- [ ] 实现 `SqlServerDialect` / `SqlServerProviderFactory`。
- [ ] 实现 `OracleDialect` / `OracleProviderFactory`。

### Phase 3：表达式与代码结构

- [ ] 创建 `SqlExpressionVisitor` + `ParameterReplacer` + `ExpressionCache`。
- [ ] 引入 `QuerySpec<T>` 替代泛型爆炸。
- [ ] 精简 Attribute，对齐 EF Core 命名。
- [ ] 引入 Fluent API（可选）。
- [ ] 移除 `IFoundationModel` 强制 `string ID`。

### Phase 4：单库分表

- [ ] 实现 `IShardingStrategy` / `IShardingRouter` / `ISqlRewriter`。
- [ ] 实现取模/哈希/时间分片策略。
- [ ] 实现 Snowflake ID 生成器。
- [ ] 单分片查询/插入/更新/删除接入分表路由。
- [ ] 实现单库内 `UNION ALL` 跨分片查询。
- [ ] 配置化分片规则。

### Phase 5：高级优化

- [ ] 表达式解析缓存命中统计与优化。
- [ ] Command Prepare 与连接会话复用。
- [ ] 批量插入优化（MySQL `LOAD DATA` / `INSERT VALUES (...),(...)`）。
- [ ] 二级查询缓存（按 SQL + 参数缓存结果）。
- [ ] 异步流 `IAsyncEnumerable<T>` 支持大数据量读取。
- [ ] Source Generator 编译期元数据生成（远期）。

---

## 八、其他可优化点

### 8.1 批量操作

当前每条记录单独 `INSERT`，大数据量写入极慢。

**方案**：
- MySQL：`INSERT INTO table (a,b) VALUES (1,2), (3,4), ...`。
- SQL Server：`SqlBulkCopy`。
- Oracle：`ArrayBind` 或 `OracleBulkCopy`。

### 8.2 二级缓存

对读多写少的表，按 SQL + 参数缓存结果：

```csharp
public interface IQueryCache
{
    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan ttl);
    Task RemoveAsync(string key);
}
```

注意：写操作后需要使相关缓存失效。

### 8.3 异步流

大数据量导出场景，提供 `IAsyncEnumerable<T>`：

```csharp
public IAsyncEnumerable<T> StreamAsync<T>(QuerySpec<T> spec)
{
    // 流式读取，避免一次性加载全部数据
}
```

### 8.4 SQL 注入加固

虽然大部分查询用参数化，但仍有拼接点：

```csharp
// 当前问题代码
sql += " and " + x.ParameterName + "='" + x.Value + "'";
```

必须改为纯参数化，列名/表名变更时加白名单校验。

### 8.5 可空引用类型

开启 `<Nullable>enable</Nullable>`，减少运行时空引用异常。

### 8.6 单元测试与基准测试

- 为 SQL Builder、表达式解析器、映射器写单元测试。
- 使用 BenchmarkDotNet 对比优化前后性能。

---

## 九、设计原则

1. **兼容优先**：旧 API 作为 facade 保留，内部转发。
2. **小步快跑**：每个 Phase 独立可验证，不一次性大爆炸改造。
3. **缓存为王**：元数据、表达式、连接、结果，能缓存的都缓存。
4. **数据库无关**：所有 SQL 特定语法走 `ISqlDialect`。
5. **分片键优先**：80% 以上查询应带分片键，避免跨分片扫描。

---

## 十、关键决策记录

| 决策 | 选择 | 理由 |
|---|---|---|
| 是否保留 Attribute | 保留，但精简 | 与现有代码兼容，学习成本低 |
| 是否引入 Fluent API | 混合模式 | 简单映射用 Attribute，复杂用 Fluent |
| 分片先单库还是分库 | 先单库分表 | 风险低，满足当前扩容需求 |
| 全局 ID 方案 | Snowflake | 趋势递增，适合作为分片键 |
| 跨分片查询合并方式 | 单库 UNION ALL | 比循环查询内存合并更高效 |
| 是否自研到底 | 保留 facade，内部可替换 | 未来可平滑迁移到 Dapper/EF Core |

---

## 十二、NuGet 依赖建议

```xml
<ItemGroup>
  <!-- MySQL -->
  <PackageReference Include="MySqlConnector" Version="2.3.7" />
  <!-- SQL Server -->
  <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.1" />
  <!-- Oracle -->
  <PackageReference Include="Oracle.ManagedDataAccess.Core" Version="23.5.0" />
  <!-- 测试 -->
  <PackageReference Include="BenchmarkDotNet" Version="0.14.0" />
  <PackageReference Include="xunit" Version="2.9.0" />
</ItemGroup>
```

> 注：`MySqlConnector` 比 `MySql.Data` 性能更好，MIT 协议，建议替换评估。详见第 11 章。

---

## 十一、MySqlConnector 替换评估

### 11.1 兼容性说明

MySqlConnector 提供兼容命名空间 `MySql.Data.MySqlClient`，包替换后大部分 API 签名保持不变。

```xml
<PackageReference Include="MySqlConnector" Version="2.3.7" />
```

因此：
- `MySqlConnection`、`MySqlCommand`、`MySqlParameter`、`MySqlDataReader`、`MySqlDataAdapter` 类型名不变。
- 异步方法签名不变，但底层是真正的异步实现（MySql.Data 大量异步是同步套壳）。

### 11.2 直接换包的改动量

| 改动项 | 说明 |
|---|---|
| csproj 包引用 | `MySql.Data` 8.4.0 → `MySqlConnector` 2.3.7 |
| 命名空间 | 保持 `using MySql.Data.MySqlClient;` 不变 |
| 类型签名 | `MySqlParameter[]`、`MySqlDataReader` 等无需改动 |
| 连接字符串 | 95% 兼容，个别关键字可能需微调 |
| 布尔/DateTime | 行为基本一致，少数边界场景需测试 |

**直接换包改动很小**，适合快速获得异步性能提升。

### 11.3 推荐做法：先做 Provider 抽象再换包

虽然直接换包改动小，但代码仍强耦合 MySQL。建议按 Phase 2 引入 `IDbProviderFactory` 后，再替换驱动：

```csharp
public class MySqlProviderFactory : IDbProviderFactory
{
    public DbConnection CreateConnection(string cs) => new MySqlConnection(cs);
    public DbCommand CreateCommand() => new MySqlCommand();
    public DbParameter CreateParameter() => new MySqlParameter();
    public DbDataAdapter CreateDataAdapter() => new MySqlDataAdapter();
}
```

换包时只需要改 `.csproj`，工厂类代码不变（兼容命名空间下类型名一致）。

同时把方法签名从具体 MySQL 类型改为 ADO.NET 抽象类型：

```csharp
// 改前
public async Task<DataTable> GetTableAsync(CommandType commandType, string sqlText, params MySqlParameter[] param)

// 改后
public async Task<DataTable> GetTableAsync(CommandType commandType, string sqlText, params DbParameter[] param)
```

### 11.4 迁移步骤

1. 保持 `MySql.Data` 不变，先把签名改为 `DbParameter`/`DbDataReader`。
2. 创建 `IDbProviderFactory` 和 `MySqlProviderFactory`（基于 MySql.Data）。
3. 验证 MySQL 功能完整。
4. 把 `.csproj` 的 `MySql.Data` 换成 `MySqlConnector`。
5. 重新编译，处理少量 API/行为差异。
6. 跑测试验证。

### 11.5 性能收益预期

- 异步查询：比 MySql.Data 快 **2~5 倍**。
- 高并发连接池管理更好。
- 内存占用通常更低。
- 对 .NET Core / .NET 5+ 协议支持更友好。

### 11.6 迁移方案对比

| 方案 | 改动量 | 优点 | 缺点 |
|---|---|---|---|
| 直接换包 | 很小 | 快速获得性能提升 | 仍然强耦合 MySQL |
| 先 Provider 抽象再换包 | 中等 | 一次性解决多数据库问题 | 需要更多工作量 |
| 迁移到 Dapper | 较大 | 性能最好，社区成熟 | 改造业务代码多 |

**推荐：先 Provider 抽象，再换 MySqlConnector**，与 Phase 2 多数据库支持目标一致，避免重复劳动。
