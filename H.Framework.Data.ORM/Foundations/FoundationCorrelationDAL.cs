using System;
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

        public TModel Get(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy), arr.Item5, include, arr.Item3.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPage_MySQL, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy, pageSize, pageNum), arr.Item5, include, arr.Item3.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);

            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy), arr.Item5, include, arr.Item3.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPage_MySQL, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy, pageSize, pageNum), arr.Item5, include, arr.Item3.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy), arr.Item5, include, arr.Item3.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPage_MySQL, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy, pageSize, pageNum), arr.Item5, include, arr.Item3.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy), arr.Item5, include, arr.Item3.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPage_MySQL, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy, pageSize, pageNum), arr.Item5, include, arr.Item3.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy), arr.Item5, include, arr.Item3.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPage_MySQL, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy, pageSize, pageNum), arr.Item5, include, arr.Item3.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2)).Rows[0][0]);
        }
    }

    public abstract class FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> : FoundationDAL<TModel>, IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        public FoundationDAL() : base()
        {
        }

        public TModel Get(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return GetList(whereSelector, include, orderBy).FirstOrDefault();
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.Get, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy), arr.Item5, include, arr.Item3.ToArray());
        }

        public IEnumerable<TModel> GetList(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Fabricate.GetListByTable<TModel>(CommandType.Text, CreateSql(SqlType.GetPage_MySQL, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2, orderBy, pageSize, pageNum), arr.Item5, include, arr.Item3.ToArray());
        }

        public int Count(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true)
        {
            var arr = MySQLUtility.ExecuteParm(whereSelector, include);
            return Convert.ToInt32(Fabricate.GetTable(CommandType.Text, CreateSql(isAll ? SqlType.Count_MySQL : SqlType.CountDetail, arr.Item4, arr.Item1.Remove(arr.Item1.Length - 1), arr.Item2)).Rows[0][0]);
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
            return BaseVisitLambda(node);
        }
    }
}