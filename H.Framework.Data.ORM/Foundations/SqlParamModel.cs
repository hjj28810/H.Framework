using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.Data.ORM.Foundations
{
    public class SqlParamModel
    {
        public SqlParamModel(string mainTableName, string joinTableName, string columnName)
        {
            MainTableName = mainTableName;
            JoinTableName = joinTableName;
            ColumnName = columnName.TrimEnd(',');
            ListSqlParams = new List<MySqlParameter>();
        }

        public string MainTableName { get; set; }
        public string JoinTableName { get; set; }

        public string TableName => MainTableName + JoinTableName;

        public string ColumnName { get; set; }

        public string WhereSQL => MainWhereSQL + JoinWhereSQL;
        public string JoinWhereSQL { get; set; }
        public string MainWhereSQL { get; set; }

        public List<MySqlParameter> ListSqlParams { get; set; }

        public List<TableMap> ListTableMap { get; set; }
    }
}