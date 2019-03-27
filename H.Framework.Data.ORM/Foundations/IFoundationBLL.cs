using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public interface IFoundationBLL<TViewModel> where TViewModel : IFoundationViewModel, new()
    {
        string Add(List<TViewModel> list);

        string Add(TViewModel model);

        Task<string> AddAsync(List<TViewModel> list);

        Task<string> AddAsync(TViewModel model);

        void Delete(string id);

        void DeleteLogic(string id);

        void Update(List<TViewModel> list, string include = "");

        void Update(TViewModel model, string include = "");

        TViewModel Get(Expression<Func<TViewModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        //List<TViewModel> ExecuteQuerySQL(string sqlText);

        int Count(Expression<Func<TViewModel, bool>> whereSelector);

        Task<int> CountAsync(Expression<Func<TViewModel, bool>> whereSelector);

        TViewModel Get(WhereQueryable<TViewModel> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        int Count(WhereQueryable<TViewModel> whereSelector);

        Task<int> CountAsync(WhereQueryable<TViewModel> whereSelector);
    }
}