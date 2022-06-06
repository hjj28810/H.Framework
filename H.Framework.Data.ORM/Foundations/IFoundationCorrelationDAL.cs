using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public interface IFoundationDAL<TModel, TForeignModel> : IFoundationDAL<TModel> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
    {
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationDAL<TModel, TForeignModel, TForeignModel1> : IFoundationDAL<TModel> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2> : IFoundationDAL<TModel> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> : IFoundationDAL<TModel> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> : IFoundationDAL<TModel> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true);
    }

    public interface IFoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> : IFoundationDAL<TModel> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        Task<TModel> GetAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, int pageSize = 20, int pageNum = 0, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true);

        Task<TModel> GetAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<IEnumerable<TModel>> GetListAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize = 20, int pageNum = 1, string include = "", IEnumerable<OrderByEntity> orderBy = null);

        Task<int> CountAsync(Expression<Func<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true);
    }
}