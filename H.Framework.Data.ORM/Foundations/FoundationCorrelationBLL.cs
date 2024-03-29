﻿using H.Framework.Core.Mapping;
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
        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(whereSelector.Expr), include, isAll);
        }
    }

    public abstract class FoundationBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TDAL> : FoundationBLL<TViewModel, TModel, TDAL>, IFoundationBLL<TViewModel, TForeignModel, TForeignModel1>
                                                                   where TModel : IFoundationModel, new()
                                                                   where TDAL : FoundationDAL<TModel, TForeignModel, TForeignModel1>
                                                                   where TViewModel : IFoundationViewModel, new()
                                                                   where TForeignModel : IFoundationModel, new()
                                                                   where TForeignModel1 : IFoundationModel, new()
    {
        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(whereSelector.Expr), include, isAll);
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
        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(whereSelector.Expr), include, isAll);
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
        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(whereSelector.Expr), include, isAll);
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
        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(whereSelector.Expr), include, isAll);
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
        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), pageSize, pageNum, mainInclude, joinInclude, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, int pageSize, int pageNum, string mainInclude = "", string joinInclude = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(mainWhereSelector.Expr, joinWhereSelector.Expr, pageSize, pageNum, mainInclude, joinInclude, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, bool>> mainWhereSelector, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, bool>> mainWhereSelector, Expression<Func<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> mainWhereSelector, Expression<Func<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> mainWhereSelector, Expression<Func<TForeignModel3, TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> mainWhereSelector, Expression<Func<TForeignModel4, TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> mainWhereSelector, Expression<Func<TForeignModel5, bool>> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector), MySQLUtility.GetModelExpr(joinWhereSelector, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel> mainWhereSelector, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel> mainWhereSelector, WhereJoinQueryable<TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1> mainWhereSelector, WhereJoinQueryable<TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> mainWhereSelector, WhereJoinQueryable<TForeignModel3, TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> mainWhereSelector, WhereJoinQueryable<TForeignModel4, TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> mainWhereSelector, WhereJoinQueryable<TForeignModel5> joinWhereSelector, string mainInclude = "", string joinInclude = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(mainWhereSelector.Expr), MySQLUtility.GetModelExpr(joinWhereSelector.Expr, mainInclude.Split(',').Length), mainInclude, joinInclude, isAll);
        }

        public virtual async Task<TViewModel> GetAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            var item = await DAL.GetAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, orderBy);
            if (item != null)
                return item.MapTo(RetrieveSelector);
            else
                return default;
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public virtual async Task<List<TViewModel>> GetListAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return (await DAL.GetListAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), pageSize, pageNum, include, orderBy)).MapAllTo(RetrieveSelector).ToList();
        }

        public async Task<TViewModel> GetAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, include, orderBy);
        }

        public async Task<List<TViewModel>> GetListAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, int pageSize, int pageNum, string include = "", IEnumerable<OrderByEntity> orderBy = null)
        {
            return await GetListAsync(whereSelector.Expr, pageSize, pageNum, include, orderBy);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector), include, isAll);
        }

        public virtual async Task<int> CountAsync(WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> whereSelector, string include = "", bool isAll = true)
        {
            return await DAL.CountAsync(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(whereSelector.Expr), include, isAll);
        }
    }

    //public class ConvertMemberVisitor<TViewModel, TForeignModel> : ConvertMemberVisitor<TViewModel>
    //{
    //    private readonly ParameterExpression[] _parms;

    //    public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
    //    {
    //        _parms = parms;
    //    }

    //    protected override Expression VisitMember(MemberExpression node)
    //    {
    //        var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        if (!(node.Expression is ParameterExpression))//&& (member == null || memberFM == null)所有表有同一个字段名时，无法查询bug，比如id,a.id=a.id
    //        {
    //            return ChildVisitMember(node);
    //        }
    //        if (memberFM != null && memberFM.ReflectedType == node.Expression.Type)
    //            return Expression.MakeMemberAccess(_parms[1], memberFM);
    //        if (member != null)
    //            return Expression.MakeMemberAccess(_parms[0], member);
    //        return base.VisitMember(node);
    //    }
    //}

    //public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1> : ConvertMemberVisitor<TViewModel>
    //{
    //    private readonly ParameterExpression[] _parms;

    //    public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
    //    {
    //        _parms = parms;
    //    }

    //    protected override Expression VisitMember(MemberExpression node)
    //    {
    //        // BUG 取相同属性，会return第一个
    //        var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        if (!(node.Expression is ParameterExpression)) //&& (member == null || memberFM == null || memberFM1 == null) 所有表有同一个字段名时，无法查询bug，比如id,a.id=a.id
    //        {
    //            return ChildVisitMember(node);
    //        }
    //        if (memberFM1 != null && memberFM1.ReflectedType == node.Expression.Type)
    //            return Expression.MakeMemberAccess(_parms[2], memberFM1);
    //        if (memberFM != null && memberFM.ReflectedType == node.Expression.Type)
    //            return Expression.MakeMemberAccess(_parms[1], memberFM);
    //        if (member != null)
    //            return Expression.MakeMemberAccess(_parms[0], member);
    //        return base.VisitMember(node);
    //    }
    //}

    //public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> : ConvertMemberVisitor<TViewModel>
    //{
    //    private readonly ParameterExpression[] _parms;

    //    public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
    //    {
    //        _parms = parms;
    //    }

    //    protected override Expression VisitMember(MemberExpression node)
    //    {
    //        // BUG 取相同属性，会return第一个
    //         var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM2 = _parms[3].Type.GetMember(node.Member.Name).FirstOrDefault();

    //        if (memberFM2 != null && memberFM2.ReflectedType == node.Expression.Type)
    //            return Expression.MakeMemberAccess(_parms[3], memberFM2);
    //        if (memberFM1 != null && memberFM1.ReflectedType == node.Expression.Type)
    //            return Expression.MakeMemberAccess(_parms[2], memberFM1);
    //        if (memberFM != null && memberFM.ReflectedType == node.Expression.Type)
    //            return Expression.MakeMemberAccess(_parms[1], memberFM);
    //        if (member != null)
    //            return Expression.MakeMemberAccess(_parms[0], member);
    //        return base.VisitMember(node);
    //    }
    //}

    //public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> : ConvertMemberVisitor<TViewModel>
    //{
    //    private readonly ParameterExpression[] _parms;

    //    public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
    //    {
    //        _parms = parms;
    //    }

    //    protected override Expression VisitMember(MemberExpression node)
    //    {
    //        ////BUG 取相同属性，会return第一个
    //        var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM2 = _parms[3].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM3 = _parms[4].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        if (!(node.Expression is ParameterExpression))//&& (member == null || memberFM == null || memberFM1 == null || memberFM2 == null || memberFM3 == null)所有表有同一个字段名时，无法查询bug，比如id,a.id=a.id
    //        {
    //            return ChildVisitMember(node);
    //        }
    //        if (member != null)
    //            return Expression.MakeMemberAccess(_parms[0], member);
    //        if (memberFM != null)
    //            return Expression.MakeMemberAccess(_parms[1], memberFM);
    //        if (memberFM1 != null)
    //            return Expression.MakeMemberAccess(_parms[2], memberFM1);
    //        if (memberFM2 != null)
    //            return Expression.MakeMemberAccess(_parms[3], memberFM2);
    //        if (memberFM3 != null)
    //            return Expression.MakeMemberAccess(_parms[4], memberFM3);
    //        return base.VisitMember(node);
    //    }
    //}

    //public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> : ConvertMemberVisitor<TViewModel>
    //{
    //    private readonly ParameterExpression[] _parms;

    //    public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
    //    {
    //        _parms = parms;
    //    }

    //    protected override Expression VisitMember(MemberExpression node)
    //    {
    //        //BUG 取相同属性，会return第一个
    //        var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM2 = _parms[3].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM3 = _parms[4].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM4 = _parms[5].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        if (!(node.Expression is ParameterExpression))//&& (member == null || memberFM == null || memberFM1 == null || memberFM2 == null || memberFM3 == null || memberFM4 == null)所有表有同一个字段名时，无法查询bug，比如id,a.id=a.id
    //        {
    //            return ChildVisitMember(node);
    //        }
    //        if (member != null)
    //            return Expression.MakeMemberAccess(_parms[0], member);
    //        if (memberFM != null)
    //            return Expression.MakeMemberAccess(_parms[1], memberFM);
    //        if (memberFM1 != null)
    //            return Expression.MakeMemberAccess(_parms[2], memberFM1);
    //        if (memberFM2 != null)
    //            return Expression.MakeMemberAccess(_parms[3], memberFM2);
    //        if (memberFM3 != null)
    //            return Expression.MakeMemberAccess(_parms[4], memberFM3);
    //        if (memberFM4 != null)
    //            return Expression.MakeMemberAccess(_parms[5], memberFM4);
    //        return base.VisitMember(node);
    //    }
    //}

    //public class ConvertMemberVisitor<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> : ConvertMemberVisitor<TViewModel>
    //{
    //    private readonly ParameterExpression[] _parms;

    //    public ConvertMemberVisitor(params ParameterExpression[] parms) : base(parms)
    //    {
    //        _parms = parms;
    //    }

    //    protected override Expression VisitMember(MemberExpression node)
    //    {
    //        //BUG 取相同属性，会return第一个
    //        var member = _parms[0].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM = _parms[1].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM1 = _parms[2].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM2 = _parms[3].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM3 = _parms[4].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM4 = _parms[5].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        var memberFM5 = _parms[6].Type.GetMember(node.Member.Name).FirstOrDefault();
    //        if (!(node.Expression is ParameterExpression))//&& (member == null || memberFM == null || memberFM1 == null || memberFM2 == null || memberFM3 == null || memberFM4 == null || memberFM5 == null)所有表有同一个字段名时，无法查询bug，比如id,a.id=a.id
    //        {
    //            return ChildVisitMember(node);
    //        }
    //        if (member != null)
    //            return Expression.MakeMemberAccess(_parms[0], member);
    //        if (memberFM != null)
    //            return Expression.MakeMemberAccess(_parms[1], memberFM);
    //        if (memberFM1 != null)
    //            return Expression.MakeMemberAccess(_parms[2], memberFM1);
    //        if (memberFM2 != null)
    //            return Expression.MakeMemberAccess(_parms[3], memberFM2);
    //        if (memberFM3 != null)
    //            return Expression.MakeMemberAccess(_parms[4], memberFM3);
    //        if (memberFM4 != null)
    //            return Expression.MakeMemberAccess(_parms[5], memberFM4);
    //        if (memberFM5 != null)
    //            return Expression.MakeMemberAccess(_parms[6], memberFM5);
    //        return base.VisitMember(node);
    //    }
    //}
}