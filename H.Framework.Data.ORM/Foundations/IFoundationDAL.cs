using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace H.Framework.Data.ORM.Foundations
{
    public interface IFoundationDAL<TModel> where TModel : IFoundationModel, new()
    {
        IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        IEnumerable<TModel> GetList(Expression<Func<TModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        TModel Get(Expression<Func<TModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        void Delete(TModel model);

        void DeleteLogic(TModel model);

        void Update(IEnumerable<TModel> list, string include = "");

        void Add(IEnumerable<TModel> list);

        IEnumerable<T> ExecuteQuerySQL<T>(string sqlText) where T : new();

        int Count(Expression<Func<TModel, bool>> whereSelector);
    }
}