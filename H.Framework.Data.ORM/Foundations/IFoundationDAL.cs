using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public interface IFoundationDAL<TModel> where TModel : IFoundationModel, new()
    {
        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task DeleteAsync(string id);

        Task DeleteAsync(List<string> ids);

        Task DeleteLogicAsync(TModel model);

        Task UpdateAsync(IEnumerable<TModel> list, string include = "");

        Task<string> AddAsync(IEnumerable<TModel> list);

        Task<IEnumerable<T>> ExecuteQuerySQLAsync<T>(string sqlText, params MySqlParameter[] param) where T : new();

        Task<Dictionary<string, IEnumerable<T>>> ExecuteQueryMutiSQLAsync<T>(string sqlText, string[] keys, params MySqlParameter[] param) where T : new();

        Task<int> CountAsync(Expression<Func<TModel, bool>> whereSelector);
    }
}