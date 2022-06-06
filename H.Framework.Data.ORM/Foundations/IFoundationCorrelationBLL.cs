using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public interface IFoundationBLL<TViewModel, TForeignModel> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
    {
        Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true);

        Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", bool isAll = true);
    }
}