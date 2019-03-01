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
        private static object _locker;

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

        public static Tuple<string, string, List<MySqlParameter>, string> ExecuteParm<TModel>(TModel model, string type = "", string include = "", OrderByEntity orderBy = null) where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var properties = typeof(TModel).GetProperties();
                var parms = new List<MySqlParameter>();
                string columnName = "", columnParm = "", tableName = typeof(TModel).Name;
                foreach (var prop in properties)
                {
                    if (prop.IsDefined(typeof(DataFieldIgnoreAttribute))) continue;
                    if (string.IsNullOrWhiteSpace(include) || include.Contains(prop.Name))
                    {
                        var propValue = prop.GetValue(model);
                        if (!prop.IsDefined(typeof(ForeignAttribute)) && !prop.IsDefined(typeof(DetailListAttribute)))
                        {
                            if (propValue != null)
                            {
                                if (type == "add")
                                {
                                    columnName += prop.Name + ",";
                                    //if (prop.PropertyType == typeof(DateTime)) //oracle
                                    //    columnParm += "to_date('" + propValue + "','yyyy-mm-dd hh24:mi:ss'),";
                                    //else
                                    if (prop.PropertyType == typeof(bool))
                                        columnParm += Convert.ToInt32(propValue) + ",";
                                    else
                                        columnParm += "'" + propValue + "',";
                                }
                                else
                                {
                                    if (prop.Name != "ID")
                                        //if (prop.PropertyType == typeof(DateTime)) //oracle
                                        //    columnName += "a." + prop.Name + " = to_date('" + propValue + "','yyyy-mm-dd hh24:mi:ss'),";
                                        //else
                                        if (prop.PropertyType == typeof(bool))
                                            columnName += "a." + prop.Name + " = " + Convert.ToInt32(propValue) + ",";
                                        else
                                            columnName += "a." + prop.Name + " = '" + propValue + "',";
                                    columnParm = "a.id = '" + properties.First(item => item.Name == "ID").GetValue(model) + "'";
                                }

                                parms.Add(new MySqlParameter(prop.Name, propValue));
                            }
                        }
                    }
                }
                return new Tuple<string, string, List<MySqlParameter>, string>(columnName, columnParm, parms, tableName);
            }
        }

        public static Tuple<string, string, List<MySqlParameter>, string, List<TableMap>> ExecuteParm<TModel>(Expression<Func<TModel, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var arr = ExecuteParm<TModel>(mapList, include);
                var visit = new MemberSQLVisitor(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                return new Tuple<string, string, List<MySqlParameter>, string, List<TableMap>>(arr.Item1, whereSQL, arr.Item2, arr.Item3, mapList);
            }
        }

        public static Tuple<string, List<MySqlParameter>, string> ExecuteParm<TModel>(List<TableMap> mapList = null, string include = "") where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var properties = typeof(TModel).GetProperties();
                var parms = new List<MySqlParameter>();
                string columnName = "", tableName = typeof(TModel).Name + " a ";
                int i = 0;
                var hasPrimaryProp = properties.Any(x => x.IsDefined(typeof(PrimaryKeyIDAttribute)));
                foreach (var prop in properties)
                {
                    if (prop.IsDefined(typeof(DataFieldIgnoreAttribute))) continue;
                    if (prop.IsDefined(typeof(ForeignAttribute)))
                    {
                        if (include.Contains(prop.Name))
                        {
                            var foreignAttribute = prop.GetCustomAttribute<ForeignAttribute>();
                            GetMap(mapList, prop.PropertyType, i, TableType.Foreign, prop.Name);
                            var map = mapList.FirstOrDefault(x => x.Type == TableType.Foreign && x.ForeignPropName?.ToLower() == prop.Name.ToLower());
                            if (string.IsNullOrWhiteSpace(foreignAttribute.ForeignPrimaryKeyIDName))
                                tableName += "left join " + prop.PropertyType.Name + " " + map.Alias + " on a." + foreignAttribute.ForeignKeyIDPropName + " = " + map.Alias + ".id ";
                            else
                                tableName += "left join " + prop.PropertyType.Name + " " + map.Alias + " on a." + foreignAttribute.ForeignKeyIDPropName + " = " + map.Alias + "." + foreignAttribute.ForeignPrimaryKeyIDName + " ";
                            var foreignProps = prop.PropertyType.GetProperties();
                            foreach (var foreignProp in foreignProps)
                            {
                                if (foreignProp.Name.ToUpper() == "ID" && !string.IsNullOrWhiteSpace(foreignAttribute.ForeignPrimaryKeyIDName))
                                    continue;
                                if (!foreignProp.IsDefined(typeof(ForeignAttribute)) && !foreignProp.IsDefined(typeof(DetailListAttribute)))
                                {
                                    columnName += map.Alias + "." + foreignProp.Name + " as " + map.Alias + "_" + foreignProp.Name + ",";
                                }
                            }
                            i++;
                        }
                    }
                    else if (prop.IsDefined(typeof(DetailListAttribute)))
                    {
                        if (include.Contains(prop.Name))
                        {
                            var listAttribute = prop.GetCustomAttribute<DetailListAttribute>();
                            var detailType = prop.PropertyType.GetGenericArguments()[0];
                            var detailProps = detailType.GetProperties();
                            var hasDetailPrimaryKeyProp = detailProps.Any(x => x.IsDefined(typeof(PrimaryKeyIDAttribute)));
                            var detailForeignIDProp = detailProps.FirstOrDefault(item => (item.GetCustomAttribute<ForeignKeyIDAttribute>())?.TableName.ToUpper() == typeof(TModel).Name.ToUpper());
                            if (string.IsNullOrWhiteSpace(listAttribute.TableName))
                            {
                                GetMap(mapList, detailType, i, TableType.Detail, "");
                                var mapTable = mapList.First(item => item.TableName == detailType.Name);
                                tableName += "left join " + detailType.Name + " " + mapTable.Alias + " on " + mapTable.Alias + "." + detailForeignIDProp.Name + " = a.id ";
                                foreach (var detailProp in detailProps)
                                {
                                    if (detailProp.Name.ToUpper() == "ID" && hasDetailPrimaryKeyProp)
                                        continue;
                                    if (!detailProp.IsDefined(typeof(ForeignAttribute)) && !detailProp.IsDefined(typeof(DetailListAttribute)))
                                        columnName += mapTable.Alias + "." + detailProp.Name + " as " + mapTable.Alias + "_" + detailProp.Name + ",";
                                }
                            }
                            else
                            {
                                var transitionAlias = "a" + i.ToString();
                                var transitionAliasID = transitionAlias + "." + listAttribute.ForeignKeyIDName;
                                tableName += "left join " + listAttribute.TableName + " " + transitionAlias + " on " + transitionAliasID + " = a.id ";
                                i++;
                                GetMap(mapList, detailType, i, TableType.Detail, "");
                                var mapTable = mapList.First(item => item.TableName == detailType.Name);
                                tableName += "left join " + detailType.Name + " " + mapTable.Alias + " on " + transitionAlias + "." + listAttribute.ForeignKeyIDName2 + " = " + mapTable.Alias + ".id";
                                foreach (var detailProp in detailProps)
                                {
                                    if (detailProp.Name.ToUpper() == "ID" && hasDetailPrimaryKeyProp)
                                        continue;
                                    if (!detailProp.IsDefined(typeof(ForeignAttribute)) && !detailProp.IsDefined(typeof(DetailListAttribute)))
                                        columnName += mapTable.Alias + "." + detailProp.Name + " as " + mapTable.Alias + "_" + detailProp.Name + ",";
                                }
                            }

                            i++;
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
                return new Tuple<string, List<MySqlParameter>, string>(columnName.ToLower(), parms, tableName.ToLower());
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

        public static Tuple<string, string, List<MySqlParameter>, string, List<TableMap>> ExecuteParm<TModel, TForeignModel>(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var arr = ExecuteParm<TModel>(mapList, include);
                var visit = new MemberSQLVisitor<TForeignModel>(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                return new Tuple<string, string, List<MySqlParameter>, string, List<TableMap>>(arr.Item1, whereSQL, arr.Item2, arr.Item3, mapList);
            }
        }

        public static Tuple<string, string, List<MySqlParameter>, string, List<TableMap>> ExecuteParm<TModel, TForeignModel, TForeignModel1>(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var arr = ExecuteParm<TModel>(mapList, include);
                var visit = new MemberSQLVisitor<TForeignModel1>(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                return new Tuple<string, string, List<MySqlParameter>, string, List<TableMap>>(arr.Item1, whereSQL, arr.Item2, arr.Item3, mapList);
            }
        }

        public static Tuple<string, string, List<MySqlParameter>, string, List<TableMap>> ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var arr = ExecuteParm<TModel>(mapList, include);
                var visit = new MemberSQLVisitor<TForeignModel2>(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                return new Tuple<string, string, List<MySqlParameter>, string, List<TableMap>>(arr.Item1, whereSQL, arr.Item2, arr.Item3, mapList);
            }
        }

        public static Tuple<string, string, List<MySqlParameter>, string, List<TableMap>> ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var arr = ExecuteParm<TModel>(mapList, include);
                var visit = new MemberSQLVisitor<TForeignModel3>(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                return new Tuple<string, string, List<MySqlParameter>, string, List<TableMap>>(arr.Item1, whereSQL, arr.Item2, arr.Item3, mapList);
            }
        }

        public static Tuple<string, string, List<MySqlParameter>, string, List<TableMap>> ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var arr = ExecuteParm<TModel>(mapList, include);
                var visit = new MemberSQLVisitor<TForeignModel4>(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                return new Tuple<string, string, List<MySqlParameter>, string, List<TableMap>>(arr.Item1, whereSQL, arr.Item2, arr.Item3, mapList);
            }
        }

        public static Tuple<string, string, List<MySqlParameter>, string, List<TableMap>> ExecuteParm<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "") where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            lock (_locker)
            {
                var mapList = new List<TableMap>();
                var arr = ExecuteParm<TModel>(mapList, include);
                var visit = new MemberSQLVisitor<TForeignModel5>(mapList);
                var whereSQL = "";
                if (whereSelector != null)
                {
                    visit.Visit(whereSelector);
                    whereSQL = " and " + visit.WhereSQL;
                }
                return new Tuple<string, string, List<MySqlParameter>, string, List<TableMap>>(arr.Item1, whereSQL, arr.Item2, arr.Item3, mapList);
            }
        }

        public static Expression<Func<TModel, bool>> GetModelExpr<TViewModel, TModel>(Expression<Func<TViewModel, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parameter = Expression.Parameter(typeof(TModel));
                return Expression.Lambda<Func<TModel, bool>>(((LambdaExpression)new ConvertMemberVisitor<TViewModel>(parameter).Visit(whereSelector)).Body, parameter);
            }
        }

        public static Expression<Func<TModel, TForeignModel, bool>> GetModelExpr<TViewModel, TModel, TForeignModel>(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new ParameterExpression[] { Expression.Parameter(typeof(TModel)), Expression.Parameter(typeof(TForeignModel)) };
                return Expression.Lambda<Func<TModel, TForeignModel, bool>>(((LambdaExpression)new ConvertMemberVisitor<TViewModel, TForeignModel>(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new ParameterExpression[] { Expression.Parameter(typeof(TModel)), Expression.Parameter(typeof(TForeignModel)), Expression.Parameter(typeof(TForeignModel1)) };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, bool>>(((LambdaExpression)new ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1>(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new ParameterExpression[] { Expression.Parameter(typeof(TModel)), Expression.Parameter(typeof(TForeignModel)), Expression.Parameter(typeof(TForeignModel1)), Expression.Parameter(typeof(TForeignModel2)) };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>>(((LambdaExpression)new ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new ParameterExpression[] { Expression.Parameter(typeof(TModel)), Expression.Parameter(typeof(TForeignModel)), Expression.Parameter(typeof(TForeignModel1)), Expression.Parameter(typeof(TForeignModel2)), Expression.Parameter(typeof(TForeignModel3)) };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>(((LambdaExpression)new ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new ParameterExpression[] { Expression.Parameter(typeof(TModel)), Expression.Parameter(typeof(TForeignModel)), Expression.Parameter(typeof(TForeignModel1)), Expression.Parameter(typeof(TForeignModel2)), Expression.Parameter(typeof(TForeignModel3)), Expression.Parameter(typeof(TForeignModel4)) };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>(((LambdaExpression)new ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }

        public static Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector) where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            lock (_locker)
            {
                if (whereSelector == null)
                    return null;
                var parmArr = new ParameterExpression[] { Expression.Parameter(typeof(TModel)), Expression.Parameter(typeof(TForeignModel)), Expression.Parameter(typeof(TForeignModel1)), Expression.Parameter(typeof(TForeignModel2)), Expression.Parameter(typeof(TForeignModel3)), Expression.Parameter(typeof(TForeignModel4)), Expression.Parameter(typeof(TForeignModel5)) };
                return Expression.Lambda<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>>(((LambdaExpression)new ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(parmArr).Visit(whereSelector)).Body, parmArr);
            }
        }
    }
}