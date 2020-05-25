using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace H.Framework.Data.ORM.Foundations
{
    public interface IFoundationDAL<TModel> where TModel : IFoundationModel, new()
    {
        IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        TModel Get(Expression<Func<TModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        void Delete(string id);

        void Delete(List<string> ids);

        void DeleteLogic(TModel model);

        void Update(IEnumerable<TModel> list, string include = "");

        string Add(IEnumerable<TModel> list);

        IEnumerable<T> ExecuteQuerySQL<T>(string sqlText, params MySqlParameter[] param) where T : new();

        Dictionary<string, IEnumerable<T>> ExecuteQueryMutiSQL<T>(string sqlText, string[] keys, params MySqlParameter[] param) where T : new();

        int Count(Expression<Func<TModel, bool>> whereSelector);
    }
}