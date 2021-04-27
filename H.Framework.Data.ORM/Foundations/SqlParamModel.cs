using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace H.Framework.Data.ORM.Foundations
{
    public class SqlParamModel
    {
        public SqlParamModel(string mainTableName, string joinTableName, string columnName, string simpleColumnName, string joinColumnName)
        {
            MainTableName = mainTableName;
            JoinTableName = joinTableName;
            SimpleColumnName = simpleColumnName;
            JoinColumnName = joinColumnName;
            ColumnName = columnName.TrimEnd(',');

            ListSqlParams = new List<MySqlParameter>();
        }

        public string MainTableName { get; set; }
        public string JoinTableName { get; set; }

        public string TableName => MainTableName + JoinTableName;

        public string ColumnName { get; set; }

        public string PageColumnName { get; set; }

        public string SimpleColumnName { get; set; }

        public string MainColumnName { get; set; }
        public string JoinColumnName { get; set; }

        public string WhereSQL { get; set; }
        public string JoinWhereSQL { get; set; }

        public string MainWhereSQL { get; set; }

        public List<MySqlParameter> ListSqlParams { get; set; }

        public List<TableMap> ListTableMap { get; set; }
    }
}