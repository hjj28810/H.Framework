using H.Framework.Data.ORM.Attributes;
using H.Framework.Data.ORM.Foundations;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace H.Framework.Data.ORM
{
    public class MySQLUtility
    {
        private static readonly object _locker;

        static MySQLUtility()
        {
            _locker = new object();
        }

        public static Tuple<List<MySqlParameter>, string> ExecuteLastIDParm<TModel>(TModel model) where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var properties = typeof(TModel).GetProperties();
                var parms = new List<MySqlParameter>();
                string tableName = typeof(TModel).Name;
                foreach (var prop in properties)
                {
                    if (prop.IsDefined(typeof(LastIDConditionAttribute)))
                    {
                        var propValue = prop.GetValue(model);
                        if (propValue != null)
                        {
                            parms.Add(new MySqlParameter(prop.Name, propValue));
                        }
                    }
                }
                return new Tuple<List<MySqlParameter>, string>(parms, tableName);
            }
        }

        public static Tuple<string, string, List<MySqlParameter>, string> ExecuteParm<TModel>(TModel model, string type = "", int index = 0, string include = "") where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var properties = typeof(TModel).GetProperties();
                var parms = new List<MySqlParameter>();
                string columnName = "", columnParm = "", tableName = typeof(TModel).Name;
                foreach (var prop in properties)
                {
                    if (prop.IsDefined(typeof(DataFieldIgnoreAttribute)) || prop.IsDefined(typeof(DynamicSQLFieldAttribute))) continue;
                    if (string.IsNullOrWhiteSpace(include) || include.Contains(prop.Name))
                    {
                        var propValue = prop.GetValue(model);
                        if (!prop.IsDefined(typeof(ForeignAttribute)) && !prop.IsDefined(typeof(DetailListAttribute)))
                        {
                            if (propValue == null) continue;
                            if (prop.PropertyType == typeof(DateTime) && Convert.ToDateTime(propValue) == DateTime.MinValue)
                                continue;
                            if (type == "add")
                            {
                                if (prop.IsDefined(typeof(PrimaryKeyIDAttribute))) continue;
                                if (prop.Name.ToUpper() == "ID") continue;
                                columnName += prop.Name + ",";

                                #region 直接拼sql

                                //if (prop.PropertyType == typeof(bool))
                                //    columnParm += Convert.ToInt32(propValue) + ",";
                                //else if (prop.PropertyType == typeof(DateTime))
                                //    columnParm += "'" + Convert.ToDateTime(propValue).ToString("yyyyMMddHHmmss") + "',";
                                //else
                                //    columnParm += "'" + propValue + "',";

                                #endregion 直接拼sql

                                #region 参数化

                                columnParm += $"@{prop.Name + index.ToString()},";

                                #endregion 参数化
                            }
                            else
                            {
                                #region 直接拼sql

                                //if (prop.Name != "ID")
                                //    if (prop.PropertyType == typeof(bool))
                                //        columnName += "a." + prop.Name + " = " + Convert.ToInt32(propValue) + ",";
                                //    else if (prop.PropertyType == typeof(DateTime))
                                //        columnName += "a." + prop.Name + " = '" + Convert.ToDateTime(propValue).ToString("yyyyMMddHHmmss") + "',";
                                //    else
                                //        columnName += "a." + prop.Name + " = '" + propValue + "',";

                                #endregion 直接拼sql

                                #region 参数化

                                if (prop.Name.ToUpper() != "ID")
                                    columnName += $"a.{ prop.Name } = @{prop.Name + index.ToString()},";

                                #endregion 参数化

                                columnParm = "a.id = '" + properties.First(item => item.Name.ToUpper() == "ID").GetValue(model) + "'";
                            }

                            parms.Add(new MySqlParameter(prop.Name + index.ToString(), propValue));
                        }
                    }
                }
                return new Tuple<string, string, List<MySqlParameter>, string>(columnName, columnParm, parms, tableName);
            }
        }

        public static SqlParamModel ExecuteParm<TModel>(List<TableMap> mapList = null, string include = "", int i = 0, string joinMode = "left join") where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var properties = typeof(TModel).GetProperties();
                string columnName = "", simpleColumnName = "", joinColumnName = "", mainTableName = $"`{typeof(TModel).GetCustomAttribute<DataTableAttribute>()?.TableName ?? typeof(TModel).Name.ToLower()}` a ", tableName = "";
                var hasPrimaryProp = properties.Any(x => x.IsDefined(typeof(PrimaryKeyIDAttribute)));
                var includeArray = include.Split(",");
                var dynamicSQLProps = properties.Where(x => x.IsDefined(typeof(DynamicSQLFieldAttribute)));
                foreach (var prop in properties)
                {
                    if (prop.IsDefined(typeof(DataFieldIgnoreAttribute))) continue;
                    if (prop.IsDefined(typeof(DynamicSQLFieldAttribute))) continue;
                    if (prop.IsDefined(typeof(ForeignAttribute)))
                    {
                        if (include.Contains(prop.Name))
                        {
                            var foreignAttribute = prop.GetCustomAttribute<ForeignAttribute>();
                            GetMap(mapList, prop.PropertyType, i, TableType.Foreign, prop.Name);
                            var map = mapList.FirstOrDefault(x => x.Type == TableType.Foreign && x.ForeignPropName?.ToLower() == prop.Name.ToLower());
                            if (string.IsNullOrWhiteSpace(foreignAttribute.ForeignPrimaryKeyIDName))
                                tableName += "left join `" + prop.PropertyType.Name + "` " + map.Alias + " on a." + foreignAttribute.ForeignKeyIDPropName + " = " + map.Alias + ".id ";
                            else
                                tableName += "left join `" + prop.PropertyType.Name + "` " + map.Alias + " on a." + foreignAttribute.ForeignKeyIDPropName + " = " + map.Alias + "." + foreignAttribute.ForeignPrimaryKeyIDName + " ";
                            var foreignProps = prop.PropertyType.GetProperties();
                            foreach (var foreignProp in foreignProps)
                            {
                                if (foreignProp.Name.ToUpper() == "ID" && !string.IsNullOrWhiteSpace(foreignAttribute.ForeignPrimaryKeyIDName))
                                    continue;
                                if (!foreignProp.IsDefined(typeof(ForeignAttribute)) && !foreignProp.IsDefined(typeof(DetailListAttribute)) && !foreignProp.IsDefined(typeof(DynamicSQLFieldAttribute)))
                                {
                                    //columnName += map.Alias + "." + foreignProp.Name + " as " + map.Alias + "_" + foreignProp.Name + ",";
                                    joinColumnName += map.Alias + "." + foreignProp.Name + " as " + map.Alias + "_" + foreignProp.Name + ",";
                                    simpleColumnName += map.Alias + "_" + foreignProp.Name + ",";
                                }
                            }
                            i++;
                        }
                    }
                    else if (prop.IsDefined(typeof(DetailListAttribute)))
                    {
                        if (includeArray.Any(x => x == prop.Name + "%%"))
                        {
                            i++;
                        }
                        else
                        {
                            if (include.Contains(prop.Name))
                            {
                                var listAttribute = prop.GetCustomAttribute<DetailListAttribute>();
                                var detailType = prop.PropertyType.GetGenericArguments()[0];
                                var detailProps = detailType.GetProperties();
                                var hasDetailPrimaryKeyProp = detailProps.Any(x => x.IsDefined(typeof(PrimaryKeyIDAttribute)));
                                var detailForeignIDProp = detailProps.FirstOrDefault(item => item.GetCustomAttribute<ForeignKeyIDAttribute>()?.TableName.ToUpper() == typeof(TModel).Name.ToUpper());
                                if (string.IsNullOrWhiteSpace(listAttribute.TableName))
                                {
                                    GetMap(mapList, detailType, i, TableType.Detail, "");
                                    var mapTable = mapList.First(item => item.TableName == detailType.Name);
                                    tableName += joinMode + " `" + detailType.Name + "` " + mapTable.Alias + " on " + mapTable.Alias + "." + detailForeignIDProp.Name + " = a.id ";
                                    if (!prop.IsDefined(typeof(OnlyQueryAttribute)))
                                        foreach (var detailProp in detailProps)
                                        {
                                            if (detailProp.Name.ToUpper() == "ID" && hasDetailPrimaryKeyProp)
                                                continue;
                                            if (!detailProp.IsDefined(typeof(ForeignAttribute)) && !detailProp.IsDefined(typeof(DetailListAttribute)) && !detailProp.IsDefined(typeof(DynamicSQLFieldAttribute)))
                                            {
                                                //columnName += mapTable.Alias + "." + detailProp.Name + " as " + mapTable.Alias + "_" + detailProp.Name + ",";
                                                joinColumnName += mapTable.Alias + "." + detailProp.Name + " as " + mapTable.Alias + "_" + detailProp.Name + ",";
                                                simpleColumnName += mapTable.Alias + "_" + detailProp.Name + ",";
                                            }
                                        }
                                }
                                else
                                {
                                    var transitionAlias = "a" + i.ToString();
                                    var transitionAliasID = transitionAlias + "." + listAttribute.ForeignKeyIDName;
                                    tableName += joinMode + " `" + listAttribute.TableName + "` " + transitionAlias + " on " + transitionAliasID + " = a.id ";
                                    i++;
                                    GetMap(mapList, detailType, i, TableType.Detail, "");
                                    var mapTable = mapList.First(item => item.TableName == detailType.Name);
                                    tableName += joinMode + " `" + detailType.Name + "` " + mapTable.Alias + " on " + transitionAlias + "." + listAttribute.ForeignKeyIDName2 + " = " + mapTable.Alias + ".id ";
                                    if (!prop.IsDefined(typeof(OnlyQueryAttribute)))
                                        foreach (var detailProp in detailProps)
                                        {
                                            if (detailProp.Name.ToUpper() == "ID" && hasDetailPrimaryKeyProp)
                                                continue;
                                            if (!detailProp.IsDefined(typeof(ForeignAttribute)) && !detailProp.IsDefined(typeof(DetailListAttribute)) && !detailProp.IsDefined(typeof(DynamicSQLFieldAttribute)))
                                            {
                                                //columnName += mapTable.Alias + "." + detailProp.Name + " as " + mapTable.Alias + "_" + detailProp.Name + ",";
                                                joinColumnName += mapTable.Alias + "." + detailProp.Name + " as " + mapTable.Alias + "_" + detailProp.Name + ",";
                                                simpleColumnName += mapTable.Alias + "_" + detailProp.Name + ",";
                                            }
                                        }
                                }

                                i++;
                            }
                        }
                    }
                    else
                    {
                        if (hasPrimaryProp && prop.Name.ToUpper() == "ID")
                            continue;
                        columnName += "a." + prop.Name + ",";
                        mapList.Add(new TableMap { Alias = "a", TableName = typeof(TModel).Name, ColumnName = prop.Name });
                    }
                }
                var listDynamicSQLField = new List<string>();
                foreach (var item in dynamicSQLProps)
                {
                    var sql = item.GetCustomAttribute<DynamicSQLFieldAttribute>().SQLString;
                    listDynamicSQLField.Add(sql);
                    columnName += sql + ",";
                }
                columnName += joinColumnName;
                return new SqlParamModel(mainTableName, tableName.ToLower(), columnName, simpleColumnName, joinColumnName, listDynamicSQLField);
            }
        }

        public static void GetMap(List<TableMap> list, Type model, int i, TableType type, string foreignPropName)
        {
            lock (_locker)
            {
                if (list == null) list = new List<TableMap>();
                var props = model.GetProperties();
                foreach (var prop in props)
                {
                    if (prop.IsDefined(typeof(DataFieldIgnoreAttribute))) continue;
                    if (!prop.IsDefined(typeof(DetailListAttribute)) && !prop.IsDefined(typeof(ForeignAttribute)))
                        list.Add(new TableMap { Alias = "a" + i.ToString(), TableName = model.Name, ColumnName = prop.Name, Type = type, ForeignPropName = foreignPropName });
                }
            }
        }

        public static void GetMap(List<TableMap> list, string tableName, TableType type, string foreignPropName, ref int i)
        {
            lock (_locker)
            {
                if (list == null) list = new List<TableMap>();
                list.Add(new TableMap { Alias = "a" + i.ToString(), TableName = tableName, ColumnName = foreignPropName, Type = type, ForeignPropName = foreignPropName });
                i++;
            }
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel>(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel>(whereSelector, include);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel1>(whereSelector, include);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel2>(whereSelector, include);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel3>(whereSelector, include);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel4>(whereSelector, include);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel5>(whereSelector, include);
        }

        //public static SqlParamModel ExecuteParm<TModel, TForeignModel>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        //{
        //    return ExecuteParmInternal<TModel, TForeignModel>(mainWhereSelector, joinWhereSelector, include);
        //}

        public static SqlParamModel ExecuteParm<TModel, TForeignModel>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        //public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        //{
        //    return ExecuteParmInternal<TModel, TForeignModel1>(mainWhereSelector, joinWhereSelector, include);
        //}

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel1>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel1>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel2>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel2>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel2>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel3>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel3>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel3>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel3>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel4>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel4>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel4>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel4>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel4>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel5>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel5>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel5>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel5>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel5>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return ExecuteParmInternal<TModel, TForeignModel5>(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
        }

        public static SqlParamModel ExecuteParm<TModel>(Expression<Func<TModel, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var paramModel = ExecuteParm<TModel>(mapList, include);
                var visit = new MemberSQLVisitor(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                paramModel.WhereSQL = whereSQL;
                paramModel.ListTableMap = mapList;
                return paramModel;
            }
        }

        private static SqlParamModel ExecuteParmInternal<TModel, TForeignModel>(Expression whereSelector, string include = "", int i = 0, string joinMode = "left join") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var paramModel = ExecuteParm<TModel>(mapList, include, i, joinMode);
                var visit = new MemberSQLVisitor<TForeignModel>(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                paramModel.WhereSQL = whereSQL;
                paramModel.ListTableMap = mapList;
                return paramModel;
            }
        }

        private static SqlParamModel ExecuteParmInternal<TModel, TForeignModel>(Expression mainWhereSelector, Expression joinWhereSelector, string mainInclude = "", string joinInclude = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mainSqlParam = ExecuteParmInternal<TModel, TForeignModel>(mainWhereSelector, mainInclude);
                var joinSqlParam = ExecuteParmInternal<TModel, TForeignModel>(joinWhereSelector, joinInclude, mainInclude.Split(',').Length, "left join");
                mainSqlParam.MainTableName = mainSqlParam.TableName;
                mainSqlParam.MainColumnName = mainSqlParam.ColumnName;
                if (mainSqlParam.ListDynamicSQLField != null)
                    foreach (var item in mainSqlParam.ListDynamicSQLField)
                    {
                        mainSqlParam.MainColumnName = mainSqlParam.MainColumnName.Replace(item + ",", "");
                    }
                mainSqlParam.JoinWhereSQL = joinSqlParam.WhereSQL;
                mainSqlParam.MainWhereSQL = mainSqlParam.WhereSQL;
                mainSqlParam.JoinTableName = joinSqlParam.JoinTableName;
                mainSqlParam.WhereSQL = mainSqlParam.MainWhereSQL + mainSqlParam.JoinWhereSQL;
                mainSqlParam.PageColumnName = mainSqlParam.SimpleColumnName + joinSqlParam.ColumnName;
                mainSqlParam.ListTableMap.AddRange(joinSqlParam.ListTableMap.Where(x => x.Alias != "a"));
                mainSqlParam.ColumnName = joinSqlParam.JoinColumnName + mainSqlParam.ColumnName;
                return mainSqlParam;
            }
        }

        public static Expression<Func<TModel, bool>> GetModelExpr<TViewModel, TModel>(Expression<Func<TViewModel, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parameter = new List<ParameterExpression> { Expression.Parameter(typeof(TModel), "a") };
                return Expression.Lambda<Func<TModel, bool>>(((LambdaExpression)new ConvertMemberVisitor(parameter).Visit(whereSelector)).Body, parameter);
            }
        }

        public static Expression<Func<TModel, TForeignModel, bool>> GetModelExpr<TViewModel, TModel, TForeignModel>(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TModel), "a"), Expression.Parameter(typeof(TForeignModel), "a0") };
                return Expression.Lambda<Func<TModel, TForeignModel, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, int i = 0) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TModel), "a"), Expression.Parameter(typeof(TForeignModel), "a0"), Expression.Parameter(typeof(TForeignModel1), "a1") };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TModel), "a"), Expression.Parameter(typeof(TForeignModel), "a0"), Expression.Parameter(typeof(TForeignModel1), "a1"), Expression.Parameter(typeof(TForeignModel2), "a2") };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TModel), "a"), Expression.Parameter(typeof(TForeignModel), "a0"), Expression.Parameter(typeof(TForeignModel1), "a1"), Expression.Parameter(typeof(TForeignModel2), "a2"), Expression.Parameter(typeof(TForeignModel3), "a3") };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TModel), "a"), Expression.Parameter(typeof(TForeignModel), "a0"), Expression.Parameter(typeof(TForeignModel1), "a1"), Expression.Parameter(typeof(TForeignModel2), "a2"), Expression.Parameter(typeof(TForeignModel3), "a3"), Expression.Parameter(typeof(TForeignModel4), "a4") };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TModel), "a"), Expression.Parameter(typeof(TForeignModel), "a0"), Expression.Parameter(typeof(TForeignModel1), "a1"), Expression.Parameter(typeof(TForeignModel2), "a2"), Expression.Parameter(typeof(TForeignModel3), "a3"), Expression.Parameter(typeof(TForeignModel4), "a4"), Expression.Parameter(typeof(TForeignModel5), "a5") };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TForeignModel, bool>> GetModelExpr<TForeignModel>(Expression<Func<TForeignModel, bool>> whereSelector, int i = 0) where TForeignModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TForeignModel), $"a{i}") };
                return Expression.Lambda<Func<TForeignModel, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TForeignModel, TForeignModel1, bool>> GetModelExpr<TForeignModel, TForeignModel1>(Expression<Func<TForeignModel, TForeignModel1, bool>> whereSelector, int i = 0) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TForeignModel), $"a{i}"), Expression.Parameter(typeof(TForeignModel1), $"a{i + 1}") };
                return Expression.Lambda<Func<TForeignModel, TForeignModel1, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> GetModelExpr<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int i = 0) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TForeignModel), $"a{i}"), Expression.Parameter(typeof(TForeignModel1), $"a{i + 1}"), Expression.Parameter(typeof(TForeignModel2), $"a{i + 2}") };
                return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> GetModelExpr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int i = 0) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TForeignModel), $"a{i}"), Expression.Parameter(typeof(TForeignModel1), $"a{i + 1}"), Expression.Parameter(typeof(TForeignModel2), $"a{i + 2}"), Expression.Parameter(typeof(TForeignModel3), $"a{i + 3}") };
                return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> GetModelExpr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int i = 0) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TForeignModel), $"a{i}"), Expression.Parameter(typeof(TForeignModel1), $"a{i + 1}"), Expression.Parameter(typeof(TForeignModel2), $"a{i + 2}"), Expression.Parameter(typeof(TForeignModel3), $"a{i + 3}"), Expression.Parameter(typeof(TForeignModel4), $"a{i + 4}") };
                return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> GetModelExpr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int i = 0) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new List<ParameterExpression> { Expression.Parameter(typeof(TForeignModel), $"a{i}"), Expression.Parameter(typeof(TForeignModel1), $"a{i + 1}"), Expression.Parameter(typeof(TForeignModel2), $"a{i + 2}"), Expression.Parameter(typeof(TForeignModel3), $"a{i + 3}"), Expression.Parameter(typeof(TForeignModel4), $"a{i + 4}"), Expression.Parameter(typeof(TForeignModel5), $"a{i + 5}") };
                return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>>(((LambdaExpression)new ConvertMemberVisitor(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }
    }
}