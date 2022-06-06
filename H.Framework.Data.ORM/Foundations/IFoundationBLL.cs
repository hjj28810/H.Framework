using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public interface IFoundationBLL<TViewModel> where TViewModel : IFoundationViewModel, new()
    {
        Task<string> AddAsync(List<TViewModel> list);

        Task<string> AddAsync(TViewModel model);

        Task DeleteAsync(List<string> ids);

        Task DeleteAsync(string id);

        Task DeleteLogicAsync(string id);

        Task UpdateAsync(List<TViewModel> list, string include = "");

        Task UpdateAsync(TViewModel model, string include = "");

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        //List<TViewModel> ExecuteQuerySQL(string sqlText);

        Task<int> CountAsync(Expression<Func<TViewModel, bool>> whereSelector);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(WhereQueryable<TViewModel> whereSelector);
    }
}