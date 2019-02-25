using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public interface IFoundationBLL<TViewModel, TForeignModel> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
    {
        TViewModel Get(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        TViewModel Get(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        int Count(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true);

        int Count(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true);

        int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true);

        int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true);

        int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true);

        int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", OrderByEntity orderBy = null);

        List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null);

        int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true);

        int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", bool isAll = true);
    }
}