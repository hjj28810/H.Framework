using H.Framework.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace H.Framework.Data.ORM.Foundations
{
    public abstract class FoundationBLL<TViewModel, TModel, TDAL> : IFoundationBLL<TViewModel>
                                                                where TModel : IFoundationModel, new()
                                                                where TDAL : FoundationDAL<TModel>
                                                                where TViewModel : IFoundationViewModel, new()
    {
        public TDAL DAL { get; set; }

        public FoundationBLL()
        {
            DAL = Activator.CreateInstance<TDAL>();
        }

        public virtual async Task<string> AddAsync(List<TViewModel> list)
        {
            return await AddAsync(list, Selector);
        }

        public virtual async Task<string> AddAsync(TViewModel model)
        {
            return await AddAsync(new List<TViewModel> { model }, Selector);
        }

        protected async Task<string> AddAsync(List<TViewModel> list, Func<TViewModel, TModel> selector)
        {
            if (list == null) return "";
            return await DAL.AddAsync(list.MapAllTo(selector));
        }

        public virtual async Task DeleteAsync(string id)
        {
            await DAL.DeleteAsync(id);
        }

        public virtual async Task DeleteAsync(List<string> ids)
        {
            await DAL.DeleteAsync(ids);
        }

        public virtual async Task DeleteLogicAsync(string id)
        {
            await DAL.DeleteLogicAsync(new TModel { ID = id });
        }

        public virtual async Task UpdateAsync(List<TViewModel> list, string include = "")
        {
            await UpdateAsync(list, Selector, include);
        }

        public virtual async Task UpdateAsync(TViewModel model, string include = "")
        {
            await UpdateAsync(new List<TViewModel> { model }, Selector, include);
        }

        protected async Task UpdateAsync(List<TViewModel> list, Func<TViewModel, TModel> selector, string include = "")
        {
            if (list == null) return;
            await DAL.UpdateAsync(list.MapAllTo(selector), include);
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, bool>> whereSelector)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(whereSelector));
        }

        public virtual async Task<TViewModel> GetAsync(WhereQueryable<TViewModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public virtual async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public virtual async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel> whereSelector)
        {
            return await CountAsync(whereSelector.Expr);
        }

        //public List<TViewModel> ExecuteQuerySQL(string sqlText)
        //{
        //    return ExecuteQuerySQL(sqlText, RetrieveSelector);
        //}

        //protected List<TViewModel> ExecuteQuerySQL(string sqlText, Func<TModel, TViewModel> selector)
        //{
        //    return DAL.ExecuteQuerySQL(sqlText).MapAllTo(RetrieveSelector).ToList();
        //}

        public virtual Func<TModel, TViewModel> RetrieveSelector
        {
            get;
            set;
        } = item => new TViewModel();

        public virtual Func<TViewModel, TModel> Selector
        {
            get;
            set;
        } = item => new TModel();

        public virtual async Task<TViewModel> GetAsync<TForeignModel>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel>(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel>(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel>(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel>(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel>(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel>(WhereQueryable<TViewModel, TForeignModel> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel>(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel>(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector.Expr), include, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector.Expr), include, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector.Expr), include, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector.Expr), include, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector.Expr), include, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", bool isAll = true) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector.Expr), include, isAll);
        }
    }

    public class ConvertMemberVisitor : ExpressionVisitor
    {
        private readonly List<ParameterExpression> _parms;

        public ConvertMemberVisitor(List<ParameterExpression> parms)
        {
            _parms = parms;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        //protected Expression VisitMemberChild(MemberExpression node, ParameterExpression[] parms)
        //{
        //    if (!(node.Expression is ParameterExpression))
        //    {
        //        return ChildVisitMember(node);
        //    }
        //    foreach (var item in parms)
        //    {
        //        var me = item.Type.GetMember(node.Member.Name).FirstOrDefault();
        //        if (me != null && me.ReflectedType == node.Expression.Type)
        //        {
        //            return Expression.MakeMemberAccess(item, me);
        //        }
        //    }
        //    return VisitMember(node);
        //}

        protected override Expression VisitMember(MemberExpression node)
        {
            var nodeExp = node.Expression;
            if (!(nodeExp is ParameterExpression))
                return ChildVisitMember(node);
            foreach (var item in _parms)
            {
                if (nodeExp is ParameterExpression exp && exp.Type == item.Type)//关联多个同类型，暂时用顺序和表达式字符长短判断，例：y.ID == "aa" && yy.ID = "bb"
                {
                    var paramExps = _parms.Where(x => x.Type == exp.Type).ToList();
                    if (paramExps.Count > 1)
                    {
                        var index = paramExps.IndexOf(item);
                        if (index != (exp.Name.Length - 1) && index > -1)
                            continue;
                    }
                }
                var member = item.Type.GetMember(node.Member.Name).FirstOrDefault();
                if (member != null && member.ReflectedType == nodeExp.Type)
                    return Expression.MakeMemberAccess(item, member);
            }
            var mainMember = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
            if (!(nodeExp is ParameterExpression) || mainMember == null)
            {
                object value = null, model = null;
                if (nodeExp is MemberExpression memberExp)
                {
                    //var expr = VisitMember(memberExp);
                    //if (expr is ConstantExpression const1Exp)
                    //    model = const1Exp.Value;
                    nodeExp = VisitMember(memberExp);
                }
                if (nodeExp is ConstantExpression constExp)
                    model = constExp.Value;
                if (node.Member is FieldInfo field && model != null)
                    value = field.GetValue(model);
                if (node.Member is PropertyInfo prop && model != null)
                    value = prop.GetValue(model);
                return Expression.Constant(value, value.GetType());
            }
            return Expression.MakeMemberAccess(_parms[0], mainMember);
        }

        public Expression ChildVisitMember(MemberExpression node)
        {
            object value = null, model = null;
            if (node.Expression is MemberExpression)
            {
                var expr = VisitMember(node.Expression as MemberExpression);
                if (expr is ConstantExpression)
                    model = (expr as ConstantExpression).Value;
            }
            if (node.Expression is ConstantExpression)
                model = (node.Expression as ConstantExpression).Value;
            if (node.Member is FieldInfo fieldInfo && model != null)
            {
                if (fieldInfo.FieldType == typeof(string))
                    value = fieldInfo.GetValue(model) == null ? "" : fieldInfo.GetValue(model)?.ToString().Replace("'", "''");
                else
                    value = fieldInfo.GetValue(model);
            }
            if (node.Member is PropertyInfo propertyInfo && model != null)
                if (propertyInfo.PropertyType == typeof(string))
                    value = propertyInfo.GetValue(model) == null ? "" : propertyInfo.GetValue(model)?.ToString().Replace("'", "''");
                else
                    value = propertyInfo.GetValue(model);
            return Expression.Constant(value, value?.GetType());
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Object is MemberExpression ndoe_expr)
            {
                return base.VisitMethodCall(node);
            }
            else
            {
                var value = Expression.Lambda(node, _parms).Compile().DynamicInvoke();
                return Expression.Constant(value, value.GetType());
            }
        }
    }
}