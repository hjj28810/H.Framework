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

        public static Expression<Func<TViewModel, TForeignModel, bool>> WhereAnd<TViewModel, TForeignModel>(this Expression<Func<TViewModel, TForeignModel, bool>> expr, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, bool>> WhereOr<TViewModel, TForeignModel>(this Expression<Func<TViewModel, TForeignModel, bool>> expr, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel> WhereAnd<TViewModel, TForeignModel>(this WhereQueryable<TViewModel, TForeignModel> wq, WhereQueryable<TViewModel, TForeignModel> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel> WhereOr<TViewModel, TForeignModel>(this WhereQueryable<TViewModel, TForeignModel> wq, WhereQueryable<TViewModel, TForeignModel> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel> WhereQueryAnd<TViewModel, TForeignModel>(this Expression<Func<TViewModel, TForeignModel, bool>> expr, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel> WhereQueryOr<TViewModel, TForeignModel>(this Expression<Func<TViewModel, TForeignModel, bool>> expr, Expression<Func<TViewModel, TForeignModel, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
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

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> WhereQueryAnd<TViewModel, TForeignModel, TForeignModel1>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> WhereQueryOr<TViewModel, TForeignModel, TForeignModel1>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
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

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> WhereAnd<TViewModel, TForeignModel, TForeignModel1>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> WhereOr<TViewModel, TForeignModel, TForeignModel1>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> WhereAnd<TViewModel, TForeignModel, TForeignModel1>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1> WhereOr<TViewModel, TForeignModel, TForeignModel1>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> WhereQueryAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> WhereQueryOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
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

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereQueryAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereQueryOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
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

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereQueryAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(expr).WhereAnd(new WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(newExpr));
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereQueryOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
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

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>
                  (Expression.And(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr, Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> newExpr) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            return Expression.Lambda<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>>
                  (Expression.Or(expr.Body, newExpr.Body), expr.Parameters);
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereAnd<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereAnd(newWq.Expr);
            return wq;
        }

        public static WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> WhereOr<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(this WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> wq, WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> newWq) where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
        {
            wq.Expr = wq.Expr.WhereOr(newWq.Expr);
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

        public string ToString<TModel>() where TModel : IFoundationModel, new()
        {
            return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel>(Expr), "").Item2;
        }
    }

    public class WhereQueryable<TViewModel, TForeignModel> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, bool>> expr)
        {
            Expr = expr;
        }

        public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        {
            return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel>(Expr), include).Item2;
        }
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, bool>> expr)
        {
            Expr = expr;
        }

        public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        {
            return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1>(Expr), include).Item2;
        }
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, bool>> expr)
        {
            Expr = expr;
        }

        public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        {
            return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2>(Expr), include).Item2;
        }
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, bool>> expr)
        {
            Expr = expr;
        }

        public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        {
            return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3>(Expr), include).Item2;
        }
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, bool>> expr)
        {
            Expr = expr;
        }

        public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        {
            return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4>(Expr), include).Item2;
        }
    }

    public class WhereQueryable<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> where TViewModel : IFoundationViewModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        public Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> Expr { get; set; }

        public WhereQueryable(Expression<Func<TViewModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5, bool>> expr)
        {
            Expr = expr;
        }

        public string ToString<TModel>(string include) where TModel : IFoundationModel, new()
        {
            return MySQLUtility.ExecuteParm(MySQLUtility.GetModelExpr<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5>(Expr), include).Item2;
        }
    }
}