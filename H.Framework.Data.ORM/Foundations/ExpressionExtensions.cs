using System;
using System.Linq.Expressions;

namespace H.Framework.Data.ORM.Foundations
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TViewModel, bool>> WhereAnd<TViewModel>(this Expression<Func<TViewModel, bool>> expr, Expression<Func<TViewModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new()
        {
            return Expression.Lambda<Func<TViewModel, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, bool>> WhereOr<TViewModel>(this Expression<Func<TViewModel, bool>> expr, Expression<Func<TViewModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new()
        {
            return Expression.Lambda<Func<TViewModel, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel> WhereAnd<TViewModel>(this WhereQueryable<TViewModel> wq, WhereQueryable<TViewModel> newWq) where TViewModel : IFoundationViewModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel> WhereOr<TViewModel>(this WhereQueryable<TViewModel> wq, WhereQueryable<TViewModel> newWq) where TViewModel : IFoundationViewModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel> WhereQueryAnd<TViewModel>(this Expression<Func<TViewModel, bool>> expr, Expression<Func<TViewModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new()
        {
            return new WhereQueryable<TViewModel>(expr).WhereAnd(new WhereQueryable<TViewModel>(newExpr));
        }

        public static WhereQueryable<TViewModel> WhereQueryOr<TViewModel>(this Expression<Func<TViewModel, bool>> expr, Expression<Func<TViewModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new()
        {
            return new WhereQueryable<TViewModel>(expr).WhereOr(new WhereQueryable<TViewModel>(newExpr));
        }

        public static WhereQueryable<TViewModel> WhereAnd<TViewModel>(this WhereQueryable<TViewModel> wq, Expression<Func<TViewModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new()
        {
            return wq.WhereAnd(new WhereQueryable<TViewModel>(newExpr));
        }

        public static WhereQueryable<TViewModel> WhereOr<TViewModel>(this WhereQueryable<TViewModel> wq, Expression<Func<TViewModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new()
        {
            return wq.WhereOr(new WhereQueryable<TViewModel>(newExpr));
        }

        public static Expression<Func<TForeignModel, bool>> WhereJoinAnd<TForeignModel>(this Expression<Func<TForeignModel, bool>> expr, Expression<Func<TForeignModel, bool>> newExpr) where TForeignModel : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TForeignModel, bool>> WhereJoinOr<TForeignModel>(this Expression<Func<TForeignModel, bool>> expr, Expression<Func<TForeignModel, bool>> newExpr) where TForeignModel : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereJoinQueryable<TForeignModel> WhereAnd<TForeignModel>(this WhereJoinQueryable<TForeignModel> wq, WhereJoinQueryable<TForeignModel> newWq) where TForeignModel : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereJoinAnd(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel> WhereOr<TForeignModel>(this WhereJoinQueryable<TForeignModel> wq, WhereJoinQueryable<TForeignModel> newWq) where TForeignModel : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereJoinOr(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel> JoinWhereAnd<TForeignModel>(this Expression<Func<TForeignModel, bool>> expr, Expression<Func<TForeignModel, bool>> newExpr) where TForeignModel : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel>(expr).WhereAnd(new WhereJoinQueryable<TForeignModel>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel> JoinWhereOr<TForeignModel>(this Expression<Func<TForeignModel, bool>> expr, Expression<Func<TForeignModel, bool>> newExpr) where TForeignModel : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel>(expr).WhereOr(new WhereJoinQueryable<TForeignModel>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel> JoinWhereAnd<TForeignModel>(this WhereJoinQueryable<TForeignModel> wq, Expression<Func<TForeignModel, bool>> newExpr) where TForeignModel : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereJoinQueryable<TForeignModel>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel> WhereOr<TForeignModel>(this WhereJoinQueryable<TForeignModel> wq, Expression<Func<TForeignModel, bool>> newExpr) where TForeignModel : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereJoinQueryable<TForeignModel>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1> WhereQueryAnd<TForeignModel, TForeignModel1>(this Expression<Func<TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1>(expr).WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1> WhereQueryOr<TForeignModel, TForeignModel1>(this Expression<Func<TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1>(expr).WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1> WhereAnd<TForeignModel, TForeignModel1>(this WhereJoinQueryable<TForeignModel, TForeignModel1> wq, Expression<Func<TForeignModel, TForeignModel1, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1> WhereOr<TForeignModel, TForeignModel1>(this WhereJoinQueryable<TForeignModel, TForeignModel1> wq, Expression<Func<TForeignModel, TForeignModel1, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1>(newExpr));
        }

        public static Expression<Func<TForeignModel, TForeignModel1, bool>> WhereAnd<TForeignModel, TForeignModel1>(this Expression<Func<TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TForeignModel, TForeignModel1, bool>> WhereOr<TForeignModel, TForeignModel1>(this Expression<Func<TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1> WhereAnd<TForeignModel, TForeignModel1>(this WhereJoinQueryable<TForeignModel, TForeignModel1> wq, WhereJoinQueryable<TForeignModel, TForeignModel1> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1> WhereOr<TForeignModel, TForeignModel1>(this WhereJoinQueryable<TForeignModel, TForeignModel1> wq, WhereJoinQueryable<TForeignModel, TForeignModel1> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> WhereQueryAnd<TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2>(expr).WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> WhereQueryOr<TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2>(expr).WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> wq, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> WhereOr<TForeignModel, TForeignModel1, TForeignModel2>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> wq, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> WhereOr<TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> wq, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> WhereOr<TForeignModel, TForeignModel1, TForeignModel2>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> wq, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereQueryAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(expr).WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereQueryOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(expr).WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereQueryAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(expr).WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereQueryOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(expr).WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereQueryAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(expr).WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereQueryOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(expr).WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> wq, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(newExpr));
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> wq, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(newExpr));
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr, Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereAnd<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> wq, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereOr<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> wq, WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> newWq) where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static Expression<Func<TViewModel, TForeignModel, bool>> AllWhereAnd<TViewModel, TForeignModel>(this Expression<Func<TViewModel, TForeignModel, bool>> expr, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, bool>> AllWhereOr<TViewModel, TForeignModel>(this Expression<Func<TViewModel, TForeignModel, bool>> expr, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel> WhereAnd<TViewModel, TForeignModel>(this WhereQueryable<TViewModel, TForeignModel> wq, WhereQueryable<TViewModel, TForeignModel> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel> WhereOr<TViewModel, TForeignModel>(this WhereQueryable<TViewModel, TForeignModel> wq, WhereQueryable<TViewModel, TForeignModel> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel> AllWhereQueryAnd<TViewModel, TForeignModel>(this Expression<Func<TViewModel, TForeignModel, bool>> expr, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel> AllWhereQueryOr<TViewModel, TForeignModel>(this Expression<Func<TViewModel, TForeignModel, bool>> expr, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel>(expr).WhereOr(new WhereQueryable<TViewModel, TForeignModel>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel> WhereAnd<TViewModel, TForeignModel>(this WhereQueryable<TViewModel, TForeignModel> wq, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereQueryable<TViewModel, TForeignModel>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel> WhereOr<TViewModel, TForeignModel>(this WhereQueryable<TViewModel, TForeignModel> wq, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereQueryable<TViewModel, TForeignModel>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> AllWhereQueryAnd<TViewModel, TForeignModel, TForeignModel1>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> AllWhereQueryOr<TViewModel, TForeignModel, TForeignModel1>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1>(expr).WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> WhereAnd<TViewModel, TForeignModel, TForeignModel1>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> WhereOr<TViewModel, TForeignModel, TForeignModel1>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1>(newExpr));
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> AllWhereAnd<TViewModel, TForeignModel, TForeignModel1>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> AllWhereOr<TViewModel, TForeignModel, TForeignModel1>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> WhereAnd<TViewModel, TForeignModel, TForeignModel1>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> WhereOr<TViewModel, TForeignModel, TForeignModel1>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> AllWhereQueryAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> AllWhereQueryOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(expr).WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> AllWhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> AllWhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> AllWhereQueryAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> AllWhereQueryOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(expr).WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> AllWhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> AllWhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> AllWhereQueryAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> AllWhereQueryOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(expr).WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> AllWhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> AllWhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.AllWhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereQueryAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereQueryOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(expr).WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return wq.WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> wq, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return wq.WhereOr(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(newExpr));
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }
    }

    public class WhereQueryable<TViewModel> where TViewModel : IFoundationViewModel, new()
    {
        public Expression<Func<TViewModel, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>() where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel>(Expr), "").WhereSQL;
        //}
    }

    public class WhereQueryable<TViewModel, TForeignModel> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(Expr), include).WhereSQL;
        //}
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(Expr), include).WhereSQL;
        //}
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expr), include).WhereSQL;
        //}
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expr), include).WhereSQL;
        //}
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expr), include).WhereSQL;
        //}
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expr), include).WhereSQL;
        //}
    }

    public class WhereJoinQueryable<TForeignModel> where TForeignModel : IFoundationModel, new()
    {
        public Expression<Func<TForeignModel, bool>> Expr { get; set; }

        public WhereJoinQueryable(Expression<Func<TForeignModel, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(Expr), include).WhereSQL;
        //}
    }

    public class WhereJoinQueryable<TForeignModel, TForeignModel1> where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        public Expression<Func<TForeignModel, TForeignModel1, bool>> Expr { get; set; }

        public WhereJoinQueryable(Expression<Func<TForeignModel, TForeignModel1, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(Expr), include).WhereSQL;
        //}
    }

    public class WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2> where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        public Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> Expr { get; set; }

        public WhereJoinQueryable(Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expr), include).WhereSQL;
        //}
    }

    public class WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        public Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> Expr { get; set; }

        public WhereJoinQueryable(Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expr), include).WhereSQL;
        //}
    }

    public class WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        public Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> Expr { get; set; }

        public WhereJoinQueryable(Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expr), include).WhereSQL;
        //}
    }

    public class WhereJoinQueryable<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        public Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> Expr { get; set; }

        public WhereJoinQueryable(Expression<Func<TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr)
        {
            Expr = expr;
        }

        //public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        //{
        //    return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expr), include).WhereSQL;
        //}
    }
}