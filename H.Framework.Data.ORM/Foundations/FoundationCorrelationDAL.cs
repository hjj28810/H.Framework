﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace H.Framework.Data.ORM.Foundations
{
    public abstract class FoundationDAL<TModel, TForeignModel> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);

            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, 1, 0, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(paramModel.MainTableName, paramModel.PageColumnName, paramModel.MainColumnName, paramModel.MainWhereSQL, paramModel.JoinTableName, paramModel.JoinWhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, mainInclude + "," + joinInclude, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(mainWhereSelector, joinWhereSelector, mainInclude, joinInclude);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPageOneToOne_MySQL, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL, orderBy, pageSize, pageNum), paramModel.ListTableMap, include, paramModel.ListSqlParams.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var paramModel = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, paramModel.TableName, paramModel.ColumnName, paramModel.WhereSQL)).Rows[0][0]);
        }
    }

    public class MemberSQLVisitor<TForeignModel> : MemberSQLVisitor
    {
        public MemberSQLVisitor(List<TableMap> list) : base(list)
        {
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var builder = new StringBuilder(Regex.Replace(node.ToString(), @"\(.*\=\>\ ", ""));
            //foreach (var param in node.Parameters)
            //{
            //    var tableMap = _list.FirstOrDefault(tb => tb.TableName == param.Type.Name);
            //    if (tableMap == null) continue;
            //    builder.Replace("Param_" + node.Parameters.IndexOf(param).ToString(), tableMap.Alias);
            //}
            _whereSQL = builder.ReplaceSQLKW().ToString();
            _whereSQL = Regex.Replace(_whereSQL, @"a.*\=\>\ ", "");
            return BaseVisitLambda(node);
        }
    }
}