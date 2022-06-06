using H.Framework.Data.ORM.Attributes;
using H.Framework.Data.ORM.Foundations;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM
{
    public class MySqlFabricate
    {
        public MySqlFabricate()
        {
        }

        public MySqlFabricate(string connectStr)
        {
            ConnectStr = connectStr;
        }

        public string ConnectStr { get; set; }

        /// <summary>
        /// 判断某列是否存在并且有无数据
        /// </summary>
        /// <param name="table"></param>
        /// <param name="reader"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool ReaderExists(Hashtable table, MySqlDataReader reader, string columnName)
        {
            if (table.Contains(columnName.ToLower()) && !Convert.IsDBNull(reader[columnName]))
                return true;
            return false;
        }

        /// <summary>
        /// 组装一个模型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public T Fill<T>(MySqlDataReader reader, Hashtable table) where T : new()
        {
            var t = new T();

            if (table == null || table.Count == 0)
                table = FillTable(reader);

            foreach (var item in typeof(T).GetProperties())
                if (ReaderExists(table, reader, item.Name))
                    try
                    {
                        if (item.IsDefined(typeof(DataFieldIgnoreAttribute), false)) continue;
                        if (!item.IsDefined(typeof(DetailListAttribute), false) && !item.IsDefined(typeof(ForeignAttribute), false))
                            //item.SetValue(t, Convert.ChangeType(reader[item.Name], item.PropertyType), null);
                            if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                item.SetValue(t, Activator.CreateInstance(item.PropertyType, reader[item.Name]));
                            else
                                item.SetValue(t, Convert.ChangeType(reader[item.Name], item.PropertyType));
                    }
                    catch
                    {
                        item.SetValue(t, Enum.Parse(item.PropertyType, Convert.ToString(reader[item.Name])));
                    }
            return t;
        }

        /// <summary>
        /// 组装一个模型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public T Fill<T>(MySqlDataReader reader) where T : new()
        {
            if (reader != null && !reader.IsClosed && reader.HasRows && reader.Read())
                return Fill<T>(reader, null);
            else
                return default(T);//System.Activator.CreateInstance<T>();
        }

        /// <summary>
        /// 获取模型对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public List<T> FillList<T>(MySqlDataReader reader) where T : new()
        {
            var list = new List<T>();
            if (reader != null && !reader.IsClosed && reader.HasRows)
            {
                var table = FillTable(reader);
                while (reader.Read())
                    list.Add(Fill<T>(reader, table));
                reader.Close();
                reader.Dispose();
            }

            return list;
        }

        /// <summary>
        /// 获取reader中列名集合
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public Hashtable FillTable(MySqlDataReader reader)
        {
            var table = new Hashtable();
            for (int i = 0; i < reader.FieldCount; i++)
                table.Add(reader.GetName(i).ToLower(), null);
            return table;
        }

        /// <summary>
        /// 获取模型对象集合
        /// 自动关闭连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandType"></param>
        /// <param name="sqlText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public List<T> GetListByReader<T>(CommandType commandType, string sqlText, List<TableMap> list = null, params DbParameter[] param) where T : new()
        {
            Trace.WriteLine(ConnectStr);
            Trace.WriteLine(sqlText);
            using var conn = new MySqlConnection(ConnectStr);
            conn.Open();
            using var cmd = new MySqlCommand(sqlText, conn);
            cmd.CommandType = commandType;
            if (param != null)
                cmd.Parameters.AddRange(param);
            using (var reader = cmd.ExecuteReader())
                return FillList<T>(reader);
        }

        public async Task<List<T>> GetListByTableAsync<T>(CommandType commandType, string sqlText, List<TableMap> list = null, string include = "", params MySqlParameter[] param) where T : IFoundationModel, new()
        {
            return (await GetTableAsync(commandType, sqlText, param)).ToList<T>(list, include);
        }

        public async Task<DataTable> GetTableAsync(CommandType commandType, string sqlText, params MySqlParameter[] param)
        {
            Trace.WriteLine(ConnectStr);
            Trace.WriteLine(sqlText);
            using var conn = new MySqlConnection(ConnectStr);
            using var adapter = new MySqlDataAdapter(sqlText, conn);
            adapter.SelectCommand.CommandType = commandType;
            if (param != null)
                adapter.SelectCommand.Parameters.AddRange(param);
            var ds = new DataTable();
            await adapter.FillAsync(ds);
            return ds;
        }

        public async Task<DataSet> GetSetAsync(CommandType commandType, string sqlText, params MySqlParameter[] param)
        {
            Trace.WriteLine(ConnectStr);
            Trace.WriteLine(sqlText);
            using var conn = new MySqlConnection(ConnectStr);
            using var adapter = new MySqlDataAdapter(sqlText, conn);
            adapter.SelectCommand.CommandType = commandType;
            if (param != null)
                adapter.SelectCommand.Parameters.AddRange(param);
            var ds = new DataSet();
            await adapter.FillAsync(ds);
            return ds;
        }

        /// <summary>
        /// 组装一个模型对象
        /// 自动关闭连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandType"></param>
        /// <param name="sqlText"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public T Get<T>(CommandType commandType, string sqlText, params MySqlParameter[] param) where T : new()
        {
            Trace.WriteLine(ConnectStr);
            Trace.WriteLine(sqlText);
            using var conn = new MySqlConnection(ConnectStr);
            conn.Open();
            using var cmd = new MySqlCommand(sqlText, conn);
            cmd.CommandType = commandType;
            if (param != null)
                cmd.Parameters.AddRange(param);
            using (var reader = cmd.ExecuteReader())
                return Fill<T>(reader);
        }

        public async Task<int> ExecuteNonQueryAsync(CommandType commandType, string sqlText, params MySqlParameter[] param)
        {
            Trace.WriteLine(ConnectStr);
            Trace.WriteLine(sqlText);
            using var conn = new MySqlConnection(ConnectStr);
            conn.Open();
            using var cmd = new MySqlCommand(sqlText, conn);
            int result;
            try
            {
                cmd.CommandType = commandType;
                if (param != null)
                    cmd.Parameters.AddRange(param);
                result = await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Trace.WriteLine("错误sql:" + sqlText);
                Trace.WriteLine("错误信息:" + e.Message);
                throw e;
            }

            return result;
        }

        public async Task<string> ExecuteReaderAsync(CommandType commandType, string sqlText, params MySqlParameter[] param)
        {
            Trace.WriteLine(ConnectStr);
            Trace.WriteLine(sqlText);
            string result = "";
            using var conn = new MySqlConnection(ConnectStr);
            conn.Open();
            using var tran = await conn.BeginTransactionAsync();
            DbDataReader reader = null;
            using var cmd = new MySqlCommand(sqlText, conn);
            try
            {
                cmd.Transaction = tran;
                cmd.CommandType = commandType;
                if (param != null)
                    cmd.Parameters.AddRange(param);
                using (reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result = reader[0].ToString();
                    }
                }
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                Trace.WriteLine("错误sql:" + sqlText);
                Trace.WriteLine("错误信息:" + e.Message);
                throw e;
            }

            return result;
        }
    }

    public class TableMap
    {
        public string Alias { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string AliasColumn => Alias + "_" + ColumnName;

        public string ForeignPropName { get; set; }

        public TableType Type { get; set; } = TableType.Self;
    }

    public enum TableType
    {
        Self,
        Foreign,
        Detail
    }
}