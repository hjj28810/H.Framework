using H.Framework.Data.ORM.Attributes;
using H.Framework.Data.ORM.Foundations;
using Renci.SshNet.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace H.Framework.Data.ORM
{
    public static class DataConvert
    {
        /// <summary>
        /// 将DataRow行转换成Entity
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataRow dr) where T : new()
        {
            var entity = new T();
            var members = typeof(T).GetMembers();
            foreach (var mi in members)
            {
                if (mi.MemberType == MemberTypes.Property)
                {
                    //读取属性上的DataField特性
                    var attributes = mi.GetCustomAttributes<DataFieldAttribute>(true);
                    foreach (var attr in attributes)
                    {
                        if (attr != null)
                        {
                            var propInfo = typeof(T).GetProperty(mi.Name);
                            if (dr.Table.Columns.Contains(attr.ColumnName))
                            {
                                //根据ColumnName，将dr中的相对字段赋值给Entity属性
                                propInfo.SetValue(entity,
                                 Convert.ChangeType(dr[attr.ColumnName], propInfo.PropertyType),
                                 null);
                            }
                        }
                    }
                }
            }
            return entity;
        }

        public static List<T> ToList<T>(this DataTable dt) where T : new()
        {
            var ts = new List<T>();

            var members = typeof(T).GetMembers();
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new T();
                foreach (var mi in members)
                {
                    if (mi.MemberType == MemberTypes.Property)
                    {
                        var attributes = mi.GetCustomAttributes<DataFieldAttribute>(true);
                        foreach (var attr in attributes)
                        {
                            if (attr != null)
                            {
                                var propInfo = typeof(T).GetProperty(mi.Name);
                                if (dt.Columns.Contains(attr.ColumnName))
                                    propInfo.SetValue(entity,
                                     Convert.ChangeType(dr[attr.ColumnName], propInfo.PropertyType),
                                     null);
                            }
                        }
                        if (attributes.Count() == 0)
                        {
                            var propInfo = typeof(T).GetProperty(mi.Name);
                            if (dt.Columns.Contains(propInfo.Name))
                                propInfo.SetValue(entity,
                                 Convert.ChangeType(dr[propInfo.Name], propInfo.PropertyType),
                                 null);
                        }
                    }
                }
                ts.Add(entity);
            }
            return ts;
        }

        public static Dictionary<string, IEnumerable<T>> ToDictList<T>(this DataSet ds, string[] keys) where T : new()
        {
            var dict = new Dictionary<string, IEnumerable<T>>();
            foreach (var key in keys)
                dict.Add(key, ds.Tables[Array.IndexOf(keys, key)].ToList<T>());
            return dict;
        }

        public static Dictionary<string, List<T>> ToDictList<T>(this DataSet ds, string[] keys, List<TableMap> listMap = null, string include = "") where T : IFoundationModel, new()
        {
            var dict = new Dictionary<string, List<T>>();
            foreach (var key in keys)
                dict.Add(key, ds.Tables[Array.IndexOf(keys, key)].ToList<T>(listMap, include));
            return dict;
        }

        /// <summary>
        /// 将DataTable转换成Entity列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt, List<TableMap> listMap = null, string include = "") where T : IFoundationModel, new()
        {
            var ts = new List<T>();
            var allProperties = typeof(T).GetProperties();
            var primaryKeyProperty = allProperties.FirstOrDefault(x => x.IsDefined(typeof(PrimaryKeyIDAttribute)));
            foreach (DataRow dr in dt.Rows)
            {
                T hasItem = default;
                if (primaryKeyProperty != null)
                {
                    foreach (var item in ts)
                    {
                        if (primaryKeyProperty.GetValue(item).ToString() == dr[primaryKeyProperty.Name]?.ToString())
                        {
                            hasItem = item;
                            break;
                        }
                    }
                }
                else
                    hasItem = ts.FirstOrDefault(newItem => newItem.ID == dr["ID"]?.ToString());
                var detailListPropertys = allProperties.Where(item => item.IsDefined(typeof(DetailListAttribute)));
                var propertites = allProperties.Where(item => !item.IsDefined(typeof(DetailListAttribute)));
                if (hasItem == null)
                {
                    var t = new T();
                    foreach (var pi in propertites)
                    {
                        if (pi.IsDefined(typeof(DataFieldIgnoreAttribute))) continue;
                        if (pi.IsDefined(typeof(ForeignAttribute)) && listMap != null && include.Contains(pi.Name))
                        {
                            var foreignProps = pi.PropertyType.GetProperties();
                            var foreignT = Activator.CreateInstance(pi.PropertyType);
                            foreach (var foreignProp in foreignProps)
                            {
                                if (foreignProp.IsDefined(typeof(DataFieldIgnoreAttribute))) continue;
                                if (foreignProp.IsDefined(typeof(DetailListAttribute))) continue;
                                var mapItem = listMap?.FirstOrDefault(item => item.ForeignPropName?.ToLower() == pi.Name.ToLower() && item.ColumnName == foreignProp.Name);
                                if (mapItem == null) continue;
                                if (dt.Columns.Contains(mapItem?.AliasColumn))
                                {
                                    try
                                    {
                                        if (!foreignProp.CanWrite) continue;
                                        if (dr.IsNull(mapItem?.AliasColumn)) continue;
                                        if (foreignProp.PropertyType.IsGenericType && foreignProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                            foreignProp.SetValue(foreignT, Activator.CreateInstance(foreignProp.PropertyType, dr[mapItem?.AliasColumn]), null);
                                        else
                                            foreignProp.SetValue(foreignT, Convert.ChangeType(dr[mapItem?.AliasColumn], foreignProp.PropertyType), null);
                                    }
                                    catch (Exception ex)
                                    {
                                        Trace.WriteLine(ex.Message);
                                        throw new InvalidCastException("字段名:" + mapItem?.AliasColumn + "-数据库值:" + dr[mapItem?.AliasColumn].ToString() + "-属性类型:" + foreignProp.PropertyType.ToString());
                                    }
                                }
                            }
                            pi.SetValue(t, foreignT, null);
                        }
                        else
                        {
                            if (dt.Columns.Contains(pi.Name))
                            {
                                try
                                {
                                    if (!pi.CanWrite) continue;
                                    if (dr.IsNull(pi?.Name)) continue;
                                    if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                        pi.SetValue(t, Activator.CreateInstance(pi.PropertyType, dr[pi?.Name]), null);
                                    else
                                        pi.SetValue(t, Convert.ChangeType(dr[pi?.Name], pi.PropertyType), null);
                                }
                                catch (Exception e)
                                {
                                    Trace.WriteLine(e.Message);
                                    throw new InvalidCastException("字段名:" + pi?.Name + "-数据库值:" + dr[pi?.Name].ToString() + "-属性类型:" + pi.PropertyType.ToString());
                                }
                            }
                        }
                    }
                    ts.Add(t);
                }

                foreach (var pi in detailListPropertys)
                {
                    if (pi.IsDefined(typeof(OnlyQueryAttribute))) continue;
                    if (listMap != null && include.Contains(pi.Name))
                    {
                        var childType = pi.PropertyType.GetGenericArguments()[0];
                        var childProps = childType.GetProperties();
                        var childPrimaryKeyProperty = childProps.FirstOrDefault(x => x.IsDefined(typeof(PrimaryKeyIDAttribute)));
                        IList foreignListT = null;
                        if (hasItem != null && pi.GetValue(hasItem) != null)
                            foreignListT = pi.GetValue(hasItem) as IList;
                        else
                            foreignListT = Activator.CreateInstance(pi.PropertyType) as IList;
                        var listItem = Activator.CreateInstance(childType) as IFoundationModel;
                        foreach (var childProp in childProps)
                        {
                            if (childProp.IsDefined(typeof(DataFieldIgnoreAttribute))) continue;
                            if (childProp.IsDefined(typeof(DetailListAttribute))) continue;
                            if (childProp.IsDefined(typeof(ForeignAttribute))) continue;

                            var mapItem = listMap?.FirstOrDefault(item => item.TableName == childType.Name && item.ColumnName == childProp.Name);
                            try
                            {
                                if (string.IsNullOrWhiteSpace(mapItem?.AliasColumn)) continue;
                                if (dr.IsNull(mapItem?.AliasColumn)) continue;
                                if (!childProp.CanWrite) continue;
                                if (childProp.PropertyType.IsGenericType && childProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    childProp.SetValue(listItem, Activator.CreateInstance(childProp.PropertyType, dr[mapItem?.AliasColumn]), null);
                                else
                                    childProp.SetValue(listItem, Convert.ChangeType(dr[mapItem?.AliasColumn], childProp.PropertyType));
                            }
                            catch (Exception e)
                            {
                                Trace.WriteLine(e.Message);
                                throw new InvalidCastException("字段名:" + mapItem?.AliasColumn + "-数据库值:" + dr[mapItem?.AliasColumn].ToString() + "-属性类型:" + childProp.PropertyType.ToString());
                            }
                        }
                        if (childPrimaryKeyProperty == null)
                        {
                            if (!string.IsNullOrWhiteSpace(listItem.ID))
                            {
                                bool hasListItem = false;
                                foreach (var item in foreignListT)
                                {
                                    if ((item as IFoundationModel).ID == listItem.ID)
                                    {
                                        hasListItem = true;
                                        break;
                                    }
                                }
                                if (!hasListItem)
                                    foreignListT.Add(listItem);
                            }
                            //if (listItem.GetType().GetProperty("ID").GetValue(listItem) != null)
                            //    foreignListT.Add(listItem);
                            foreach (var newItem in ts)
                                if (newItem.ID == dr["ID"]?.ToString())
                                    pi.SetValue(newItem, foreignListT, null);
                        }
                        else
                        {
                            var primaryValue = childPrimaryKeyProperty.GetValue(listItem).ToString();
                            if (!string.IsNullOrWhiteSpace(primaryValue))
                            {
                                bool hasListItem = false;
                                foreach (var item in foreignListT)
                                {
                                    if (childPrimaryKeyProperty.GetValue(item).ToString() == primaryValue)
                                    {
                                        hasListItem = true;
                                        break;
                                    }
                                }
                                if (!hasListItem)
                                    foreignListT.Add(listItem);
                            }
                            foreach (var newItem in ts)
                            {
                                if (childPrimaryKeyProperty.GetValue(newItem).ToString() == dr[childPrimaryKeyProperty.Name]?.ToString())
                                    pi.SetValue(newItem, foreignListT, null);
                            }
                        }
                    }
                }
            }

            return ts;
        }
    }
}