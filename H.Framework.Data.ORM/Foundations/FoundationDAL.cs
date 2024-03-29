﻿using H.Framework.Core.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public abstract class FoundationDAL
    {
        public static string ConnectedString { get; set; }
    }

    public abstract class FoundationDAL<TModel> : FoundationDAL, IFoundationDAL<TModel> where TModel : IFoundationModel, new()
    {
        public MySqlFabricate Fabricate { get; private set; }

        public FoundationDAL()
        {
            if (!string.IsNullOrWhiteSpace(ConnectedString))
                Fabricate = new MySqlFabricate(ConnectedString);
            else
                throw new ArgumentNullException("DB ConnectedString can't be NULL");
        }

        private readonly string modelName = typeof(TModel).Name;

        public async Task<string> AddAsync(IEnumerable<TModel> list)
        {
            if (list == null || !list.Any()) return "";
            //var builder = new StringBuilder("begin "); //oracle
            var builder = new StringBuilder();
            var i = 0;
            var paramList = new List<MySqlParameter>();
            foreach (var model in list)
            {
                var arr = MySQLUtility.ExecuteParm(model, "add", i);
                paramList.AddRange(arr.Item3);
                builder.Append(CreateSql(SqlType.Add, "`" + modelName + "`", arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2.Remove(arr.Item2.Length - 1)));
                i++;
            }
            var lastArr = MySQLUtility.ExecuteLastIDParm(list.Last());
            builder.Append(CreateSql(SqlType.LastID, "`" + modelName + "`", "", CreateSql(lastArr.Item1)));
            //builder.Append(" end;"); //oracle
            return await ExecuteReaderAsync(CommandType.Text, builder.ToString(), paramList.ToArray());
        }

        public async Task UpdateAsync(IEnumerable<TModel> list, string include = "")
        {
            if (list == null || !list.Any()) return;
            //var builder = new StringBuilder("begin ");//oracle
            var builder = new StringBuilder();
            var i = 0;
            var paramList = new List<MySqlParameter>();
            foreach (var model in list)
            {
                var arr = MySQLUtility.ExecuteParm(model, "update", i, include);
                paramList.AddRange(arr.Item3);
                builder.Append(CreateSql(SqlType.Update, "`" + modelName + "`", arr.Item1.Substring(0, arr.Item1.Length - 1), arr.Item2));
                i++;
            }
            //builder.Append(" end;");//oracle
            await ExecuteReaderAsync(CommandType.Text, builder.ToString(), paramList.ToArray());
        }

        public async Task DeleteAsync(string id)
        {
            await ExecuteReaderAsync(CommandType.Text, CreateSql(SqlType.Delete, "`" + modelName + "`").ToLower(), new MySqlParameter("@id", id));
        }

        public async Task DeleteAsync(List<string> ids)
        {
            var sqlStr = "";
            var paramList = new List<MySqlParameter>();
            ids.ForEach((x) =>
            {
                sqlStr += CreateSql(SqlType.Delete, "`" + modelName + "`", "", "", null, 0, 0, x).ToLower();
                paramList.Add(new MySqlParameter("@id" + x, x));
            });
            await ExecuteReaderAsync(CommandType.Text, sqlStr, paramList.ToArray());
        }

        public async Task DeleteLogicAsync(TModel model)
        {
            await ExecuteReaderAsync(CommandType.Text, CreateSql(SqlType.DeleteLogic, "`" + modelName + "`").ToLower(), new MySqlParameter("@id", model.ID));
        }

        protected async Task<string> ExecuteReaderAsync(CommandType commandType, string sqlText, params MySqlParameter[] param)
        {
            return await Fabricate.ExecuteReaderAsync(commandType, sqlText, param);
        }

        public async Task<IEnumerable<T>> ExecuteQuerySQLAsync<T>(string sqlText, params MySqlParameter[] param) where T : new()
        {
            return (await Fabricate.GetTableAsync(CommandType.Text, sqlText, param)).ToList<T>();
        }

        public async Task<Dictionary<string, IEnumerable<T>>> ExecuteQueryMutiSQLAsync<T>(string sqlText, string[] keys, params MySqlParameter[] param) where T : new()
        {
            return (await Fabricate.GetSetAsync(CommandType.Text, sqlText, param)).ToDictList<T>(keys);
        }

        public async Task<int> ExecuteNonQuerySQLAsync(string sqlText, params MySqlParameter[] param)
        {
            return await Fabricate.ExecuteNonQueryAsync(CommandType.Text, sqlText, param);
        }

        public async Task<int> CountAsync(Expression<Func<TModel, bool>> whereSelector)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, "");
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(SqlType.Count_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync(Expression<Func<TModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await GetListAsync(whereSelector, include, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> whereSelector, int pageSize = 20, int pageNum = 0, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        protected string CreateSql(SqlType type, string tableName, string columnName = "", string columnParm = "", IEnumerable<OrderByEntity> orderBy = null, int pageSize = 20, int pageNum = 0, string paramID = "")
        {
            string orderbyStr = CreateOrderBy(orderBy);
            var sqlStr = "";
            switch (type)
            {
                case SqlType.Add:
                    sqlStr = $"insert into {tableName.ToLower()} ({columnName}) values ({columnParm});";
                    break;

                case SqlType.Delete:
                    sqlStr = $"delete from {tableName.ToLower()} where id = @id{paramID};commit;";
                    break;

                case SqlType.DeleteLogic:
                    sqlStr = $"update {tableName.ToLower()} set isdeleted = 1 where id = @id";
                    break;

                case SqlType.Update:
                    sqlStr = $"update {tableName.ToLower()} a set {columnName} where {columnParm};";
                    break;

                case SqlType.Get:
                    var groupbyStr = columnName.Contains("case when") ? " group by id " : "";
                    sqlStr = $"select {columnName.ReplaceKeyword()} from {tableName.ToLower()} where true {columnParm}{groupbyStr}{orderbyStr}";
                    break;

                //case SqlType.GetPage_Oracle:
                //    sqlStr = "select " + ReplaceName(columnName) + " FROM (select " + ReplaceName(columnName) + ", ROWNUM rn FROM(select " + columnName + " from " + tableName + " where 1 = 1" + columnParm + orderbyStr + ") a WHERE ROWNUM <= " + (pageSize * (pageNum + 1)).ToString() + ") WHERE rn >= " + (pageSize * pageNum + 1).ToString();
                //    break;

                //case SqlType.GetPage_MySQL:
                //    sqlStr = "select rn," + ReplaceName(columnName).ReplaceKeyword() + " FROM (select if(@tid = a.id, @rownum := @rownum, @rownum := @rownum + 1) rn, @tid := a.id," + columnName + " from " + tableName.ToLower() + " join (SELECT @rownum := 0, @tid := NULL) rntemp where 1 = 1" + columnParm + orderbyStr + ") a where rn > " + (pageNum * pageSize).ToString() + " and rn <= " + ((pageNum + 1) * pageSize).ToString();
                //    break;
                case SqlType.GetPageOneToOne_MySQL:
                    sqlStr = $"select {columnName.ReplaceKeyword()} from {tableName.ToLower()} where true {columnParm}{orderbyStr} limit {pageNum * pageSize}, {pageSize}";
                    break;
                //case SqlType.CountAsync:
                //    sqlStr = "select sum(ct) as DataCount from (select count(id) as ct from (select a.id from " + tableName + " where 1 = 1" + columnParm + " group by a.id))";
                //    break;

                case SqlType.Count_MySQL:
                    sqlStr = $"select count(id) as DataCount from (select a.id from {tableName.ToLower()} where true {columnParm} group by a.id) a";
                    break;

                case SqlType.CountDetail:
                    sqlStr = $"select sum(ct) as DataCount from (select count(a.id) as ct from {tableName.ToLower()} where 1 = 1{columnParm}) a";
                    break;

                case SqlType.LastID:
                    sqlStr = $"select LAST_INSERT_ID() from {tableName.ToLower()} where true {columnParm} limit 1;";
                    break;
            }
            return sqlStr;
        }

        protected string CreateSql(string mainTableName, string columnName, string mainColumnName, string mainColumnParm = "", string tableName = "", string columnParm = "", IEnumerable<OrderByEntity> orderBy = null, int pageSize = 20, int pageNum = 0)
        {
            string orderbyStr = CreateOrderBy(orderBy);
            string mainOrderbyStr = CreateOrderBy(orderBy?.Where(x => x.IsMainTable));
            //var sqlStr = $@"create TEMPORARY table idsTable (Select id from {mainTableName} where true {mainColumnParm}{mainOrderbyStr} limit {(pageNum * pageSize)},{pageSize});
            //               SELECT {columnName} FROM {mainTableName}{tableName} where a.id in(select id from idsTable) and {columnParm}{orderbyStr};
            //               drop table idsTable;";
            var groupbyStr = columnName.Contains("case when") ? " group by id " : "";
            var sqlStr = $"SELECT {columnName} from (select {mainColumnName} from {mainTableName} where true {mainColumnParm}{mainOrderbyStr} limit {pageNum * pageSize},{pageSize}) a {tableName} where true {columnParm}{groupbyStr}{orderbyStr}";
            return sqlStr;
        }

        private string CreateOrderBy(IEnumerable<OrderByEntity> orderBy)
        {
            var orderbyStr = "";
            if (orderBy.NotNullAny())
            {
                foreach (var item in orderBy)
                {
                    orderbyStr += item.KeyWord + (item.IsAsc ? " asc" : " desc") + ",";
                }
                orderbyStr = " order by " + orderbyStr.TrimEnd(',');
            }
            return orderbyStr;
        }

        protected string CreateSql(List<MySqlParameter> list)
        {
            string sql = "";
            list.ForEach(x =>
            {
                sql += " and " + x.ParameterName + "='" + x.Value + "'";
            });
            return sql;
        }

        private string ReplaceName(string name)
        {
            return Regex.Replace(name, @"a\d{1,2}\.+\w+\ as\ ", "").Replace("a.", "");
        }

        public async Task<TModel> GetAsync<TForeignModel>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel>(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return (await GetListAsync(whereSelector, include, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel>(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel>(Expression<Func<TModel, TForeignModel, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel>(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await GetListAsync(whereSelector, include, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await GetListAsync(whereSelector, include, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await GetListAsync(whereSelector, include, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await GetListAsync(whereSelector, include, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await GetListAsync(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }

        public async Task<TModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await GetListAsync(whereSelector, include, orderBy)).FirstOrDefault();
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<IEnumerable<TModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return await Fabricate.GetListByTableAsync<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32((await Fabricate.GetTableAsync(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL))).Rows[0][0]);
        }
    }

    public enum SqlType
    {
        Add,
        Delete,
        DeleteLogic,
        Update,
        Get,
        GetPageOneToOne_MySQL,
        Count_MySQL,

        CountDetail,
        LastID
        //CountAsync,
        //GetPage_Oracle,
    }

    public class MemberSQLVisitor : ExpressionVisitor
    {
        protected List<TableMap> _list;
        protected string _whereSQL;

        public MemberSQLVisitor(List<TableMap> list)
        {
            _list = list;
        }

        public string WhereSQL => _whereSQL.ReplaceSQLKW();

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var builder = new StringBuilder(node.Body.ToString());
            foreach (var param in node.Parameters)
            {
                var tableMap = _list.FirstOrDefault(tb => tb.TableName == param.Type.Name);
                if (tableMap == null) continue;
                builder.Replace(param.ToString(), tableMap.Alias);
            }
            _whereSQL = builder.ReplaceSQLKW().ToString();
            return BaseVisitLambda(node);
        }

        protected Expression BaseVisitLambda<T>(Expression<T> node)
        {
            return base.VisitLambda(node);
        }

        private bool isMemberExpr;

        protected override Expression VisitMember(MemberExpression node)
        {
            var result = VisitExpr(node);
            if (result.Item1)
                return result.Item2;
            else
                return base.VisitMember((MemberExpression)result.Item2);
        }

        protected Tuple<bool, Expression> VisitExpr(MemberExpression node)
        {
            object value = null, model = null;
            if (node.Expression is MemberExpression)
            {
                isMemberExpr = true;
                var expr = VisitMember(node.Expression as MemberExpression);
                if (expr is ConstantExpression)
                {
                    model = (expr as ConstantExpression).Value;
                    if (node.Member is FieldInfo fInfo)
                        value = fInfo.GetValue(model);
                    if (node.Member is PropertyInfo pInfo)
                        value = pInfo.GetValue(model);
                    _whereSQL = _whereSQL.Replace(node.ToString(), "'" + value.ToString() + "'");
                }
            }
            if (node.Expression is ConstantExpression)
            {
                model = (node.Expression as ConstantExpression).Value;
                if (node.Member is FieldInfo fInfo)
                    value = fInfo.GetValue(model);
                if (node.Member is PropertyInfo pInfo)
                    value = pInfo.GetValue(model);
                if (!isMemberExpr)
                    _whereSQL = _whereSQL.Replace(node.ToString(), "'" + value.ToString() + "'");
                isMemberExpr = false;
                return new Tuple<bool, Expression>(true, Expression.Constant(value, value.GetType()));
            }
            return new Tuple<bool, Expression>(false, node);
        }
    }
}