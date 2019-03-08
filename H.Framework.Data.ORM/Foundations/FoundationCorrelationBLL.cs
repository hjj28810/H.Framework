using H.Framework.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public abstract class FoundationBLL<TViewModel, TModel, TForeignModel, TDAL> : FoundationBLL<TViewModel, TModel, TDAL>, IFoundationBLL<TViewModel, TForeignModel>
                                                                   where TModel : IFoundationModel, new()
                                                                   where TDAL : FoundationDAL<TModel, TForeignModel>
                                                                   where TViewModel : IFoundationViewModel, new()
                                                                   where TForeignModel : IFoundationModel, new()
    {
        public virtual TViewModel Get(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            var item = DAL.Get(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default(TViewModel);
        }

        public virtual Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), pageSize, pageNum, include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, pageSize, pageNum, include, orderBy));
        }

        public TViewModel Get(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Get(whereSelector.Expr, include, orderBy);
        }

        public Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy));
        }

        public virtual int Count(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, isAll);
        }

        public virtual Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector, include, isAll));
        }

        public virtual int Count(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector.Expr), include, isAll);
        }

        public virtual Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector.Expr, include, isAll));
        }
    }

    public abstract class FoundationBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TDAL> : FoundationBLL<TViewModel, TModel, TDAL>, IFoundationBLL<TViewModel, TForeignModel, TForeignModel1>
                                                                   where TModel : IFoundationModel, new()
                                                                   where TDAL : FoundationDAL<TModel, TForeignModel, TForeignModel1>
                                                                   where TViewModel : IFoundationViewModel, new()
                                                                   where TForeignModel : IFoundationModel, new()
                                                                   where TForeignModel1 : IFoundationModel, new()
    {
        public virtual TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            var item = DAL.Get(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default(TViewModel);
        }

        public virtual Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), pageSize, pageNum, include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, pageSize, pageNum, include, orderBy));
        }

        public TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Get(whereSelector.Expr, include, orderBy);
        }

        public Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy));
        }

        public virtual int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, isAll);
        }

        public virtual Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector, include, isAll));
        }

        public virtual int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector.Expr), include, isAll);
        }

        public virtual Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector.Expr, include, isAll));
        }
    }

    public abstract class FoundationBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TDAL> : FoundationBLL<TViewModel, TModel, TDAL>, IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>
                                                                 where TModel : IFoundationModel, new()
                                                                 where TDAL : FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2>
                                                                 where TViewModel : IFoundationViewModel, new()
                                                                 where TForeignModel : IFoundationModel, new()
                                                                 where TForeignModel1 : IFoundationModel, new()
                                                                 where TForeignModel2 : IFoundationModel, new()
    {
        public virtual TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            var item = DAL.Get(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default(TViewModel);
        }

        public virtual Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), pageSize, pageNum, include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, pageSize, pageNum, include, orderBy));
        }

        public TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Get(whereSelector.Expr, include, orderBy);
        }

        public Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy));
        }

        public virtual int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, isAll);
        }

        public virtual Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector, include, isAll));
        }

        public virtual int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector.Expr), include, isAll);
        }

        public virtual Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector.Expr, include, isAll));
        }
    }

    public abstract class FoundationBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TDAL> : FoundationBLL<TViewModel, TModel, TDAL>, IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>
                                                             where TModel : IFoundationModel, new()
                                                             where TDAL : FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>
                                                             where TViewModel : IFoundationViewModel, new()
                                                             where TForeignModel : IFoundationModel, new()
                                                             where TForeignModel1 : IFoundationModel, new()
                                                             where TForeignModel2 : IFoundationModel, new()
                                                             where TForeignModel3 : IFoundationModel, new()
    {
        public virtual TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            var item = DAL.Get(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default(TViewModel);
        }

        public virtual Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), pageSize, pageNum, include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, pageSize, pageNum, include, orderBy));
        }

        public TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Get(whereSelector.Expr, include, orderBy);
        }

        public Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy));
        }

        public virtual int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, isAll);
        }

        public virtual Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector, include, isAll));
        }

        public virtual int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector.Expr), include, isAll);
        }

        public virtual Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector.Expr, include, isAll));
        }
    }

    public abstract class FoundationBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TDAL> : FoundationBLL<TViewModel, TModel, TDAL>, IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>
                                                         where TModel : IFoundationModel, new()
                                                         where TDAL : FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>
                                                         where TViewModel : IFoundationViewModel, new()
                                                         where TForeignModel : IFoundationModel, new()
                                                         where TForeignModel1 : IFoundationModel, new()
                                                         where TForeignModel2 : IFoundationModel, new()
                                                         where TForeignModel3 : IFoundationModel, new()
                                                         where TForeignModel4 : IFoundationModel, new()
    {
        public virtual TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            var item = DAL.Get(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default(TViewModel);
        }

        public virtual Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), pageSize, pageNum, include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, pageSize, pageNum, include, orderBy));
        }

        public TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Get(whereSelector.Expr, include, orderBy);
        }

        public Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy));
        }

        public virtual int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, isAll);
        }

        public virtual Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector, include, isAll));
        }

        public virtual int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector.Expr), include, isAll);
        }

        public virtual Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector.Expr, include, isAll));
        }
    }

    public abstract class FoundationBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, TDAL> : FoundationBLL<TViewModel, TModel, TDAL>, IFoundationBLL<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>
                                                         where TModel : IFoundationModel, new()
                                                         where TDAL : FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>
                                                         where TViewModel : IFoundationViewModel, new()
                                                         where TForeignModel : IFoundationModel, new()
                                                         where TForeignModel1 : IFoundationModel, new()
                                                         where TForeignModel2 : IFoundationModel, new()
                                                         where TForeignModel3 : IFoundationModel, new()
                                                         where TForeignModel4 : IFoundationModel, new()
                                                         where TForeignModel5 : IFoundationModel, new()
    {
        public virtual TViewModel Get(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            var item = DAL.Get(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default(TViewModel);
        }

        public virtual Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, include, orderBy));
        }

        public virtual List<TViewModel> GetList(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return DAL.GetList(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), pageSize, pageNum, include, orderBy).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector, pageSize, pageNum, include, orderBy));
        }

        public TViewModel Get(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Get(whereSelector.Expr, include, orderBy);
        }

        public Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => Get(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, include, orderBy));
        }

        public List<TViewModel> GetList(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, int pageSize, int pageNum, string include = "", OrderByEntity orderBy = null)
        {
            return Task.Run(() => GetList(whereSelector.Expr, pageSize, pageNum, include, orderBy));
        }

        public virtual int Count(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, isAll);
        }

        public virtual Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector, include, isAll));
        }

        public virtual int Count(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", bool isAll = true)
        {
            return DAL.Count(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector.Expr), include, isAll);
        }

        public virtual Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", bool isAll = true)
        {
            return Task.Run(() => Count(whereSelector.Expr, include, isAll));
        }
    }

    public class ConvertMemberVisitor<TViewModel, TForeignModel> : ConvertMemberVisitor<TViewModel>
    {
        private readonly ParameterExpression[] _parms;

        public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
        {
            _parms = parms;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
            if (!(node.Expression is ParameterExpression) && (member == null || memberFM == null))
            {
                return ChildVisitMember(node);
            }
            if (memberFM != null && memberFM.ReflectedType == node.Expression.Type)
                return Expression.MakeMemberAccess(_parms[1], memberFM);
            if (member != null)
                return Expression.MakeMemberAccess(_parms[0], member);
            return base.VisitMember(node);
        }
    }

    public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1> : ConvertMemberVisitor<TViewModel>
    {
        private readonly ParameterExpression[] _parms;

        public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
        {
            _parms = parms;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            ///BUG 取相同属性，会return第一个
            var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
            if (!(node.Expression is ParameterExpression) && (member == null || memberFM == null || memberFM1 == null))
            {
                return ChildVisitMember(node);
            }
            if (memberFM1 != null && memberFM1.ReflectedType == node.Expression.Type)
                return Expression.MakeMemberAccess(_parms[2], memberFM1);
            if (memberFM != null && memberFM.ReflectedType == node.Expression.Type)
                return Expression.MakeMemberAccess(_parms[1], memberFM);
            if (member != null)
                return Expression.MakeMemberAccess(_parms[0], member);
            return base.VisitMember(node);
        }
    }

    public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> : ConvertMemberVisitor<TViewModel>
    {
        private readonly ParameterExpression[] _parms;

        public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
        {
            _parms = parms;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            ///BUG 取相同属性，会return第一个
            var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM2 = _parms[3].Type.GetMember(node.Member.Name).FirstOrDefault();
            if (!(node.Expression is ParameterExpression) && (member == null || memberFM == null || memberFM1 == null || memberFM2 == null))
            {
                return ChildVisitMember(node);
            }
            if (memberFM2 != null && memberFM2.ReflectedType == node.Expression.Type)
                return Expression.MakeMemberAccess(_parms[3], memberFM2);
            if (memberFM1 != null && memberFM1.ReflectedType == node.Expression.Type)
                return Expression.MakeMemberAccess(_parms[2], memberFM1);
            if (memberFM != null && memberFM.ReflectedType == node.Expression.Type)
                return Expression.MakeMemberAccess(_parms[1], memberFM);
            if (member != null)
                return Expression.MakeMemberAccess(_parms[0], member);
            return base.VisitMember(node);
        }
    }

    public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> : ConvertMemberVisitor<TViewModel>
    {
        private readonly ParameterExpression[] _parms;

        public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
        {
            _parms = parms;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            ///BUG 取相同属性，会return第一个
            var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM2 = _parms[3].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM3 = _parms[4].Type.GetMember(node.Member.Name).FirstOrDefault();
            if (!(node.Expression is ParameterExpression) && (member == null || memberFM == null || memberFM1 == null || memberFM2 == null || memberFM3 == null))
            {
                return ChildVisitMember(node);
            }
            if (member != null)
                return Expression.MakeMemberAccess(_parms[0], member);
            if (memberFM != null)
                return Expression.MakeMemberAccess(_parms[1], memberFM);
            if (memberFM1 != null)
                return Expression.MakeMemberAccess(_parms[2], memberFM1);
            if (memberFM2 != null)
                return Expression.MakeMemberAccess(_parms[3], memberFM2);
            if (memberFM3 != null)
                return Expression.MakeMemberAccess(_parms[4], memberFM3);
            return base.VisitMember(node);
        }
    }

    public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> : ConvertMemberVisitor<TViewModel>
    {
        private readonly ParameterExpression[] _parms;

        public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
        {
            _parms = parms;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            ///BUG 取相同属性，会return第一个
            var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM2 = _parms[3].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM3 = _parms[4].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM4 = _parms[5].Type.GetMember(node.Member.Name).FirstOrDefault();
            if (!(node.Expression is ParameterExpression) && (member == null || memberFM == null || memberFM1 == null || memberFM2 == null || memberFM3 == null || memberFM4 == null))
            {
                return ChildVisitMember(node);
            }
            if (member != null)
                return Expression.MakeMemberAccess(_parms[0], member);
            if (memberFM != null)
                return Expression.MakeMemberAccess(_parms[1], memberFM);
            if (memberFM1 != null)
                return Expression.MakeMemberAccess(_parms[2], memberFM1);
            if (memberFM2 != null)
                return Expression.MakeMemberAccess(_parms[3], memberFM2);
            if (memberFM3 != null)
                return Expression.MakeMemberAccess(_parms[4], memberFM3);
            if (memberFM4 != null)
                return Expression.MakeMemberAccess(_parms[5], memberFM4);
            return base.VisitMember(node);
        }
    }

    public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> : ConvertMemberVisitor<TViewModel>
    {
        private readonly ParameterExpression[] _parms;

        public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
        {
            _parms = parms;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            ///BUG 取相同属性，会return第一个
            var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM2 = _parms[3].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM3 = _parms[4].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM4 = _parms[5].Type.GetMember(node.Member.Name).FirstOrDefault();
            var memberFM5 = _parms[6].Type.GetMember(node.Member.Name).FirstOrDefault();
            if (!(node.Expression is ParameterExpression) && (member == null || memberFM == null || memberFM1 == null || memberFM2 == null || memberFM3 == null || memberFM4 == null || memberFM5 == null))
            {
                return ChildVisitMember(node);
            }
            if (member != null)
                return Expression.MakeMemberAccess(_parms[0], member);
            if (memberFM != null)
                return Expression.MakeMemberAccess(_parms[1], memberFM);
            if (memberFM1 != null)
                return Expression.MakeMemberAccess(_parms[2], memberFM1);
            if (memberFM2 != null)
                return Expression.MakeMemberAccess(_parms[3], memberFM2);
            if (memberFM3 != null)
                return Expression.MakeMemberAccess(_parms[4], memberFM3);
            if (memberFM4 != null)
                return Expression.MakeMemberAccess(_parms[5], memberFM4);
            if (memberFM5 != null)
                return Expression.MakeMemberAccess(_parms[6], memberFM5);
            return base.VisitMember(node);
        }
    }
}