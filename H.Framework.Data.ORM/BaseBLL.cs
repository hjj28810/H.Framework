using H.Framework.Data.ORM.Foundations;

namespace H.Framework.Data.ORM
{
    public abstract class BaseBLL<TViewModel, TModel, TDAL> : FoundationBLL<TViewModel, TModel, TDAL> where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TDAL : BaseDAL<TModel>, new()
    {
    }

    public abstract class BaseBLL<TViewModel, TModel, TForeignModel, TDAL> : FoundationBLL<TViewModel, TModel, TForeignModel, TDAL> where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TDAL : BaseDAL<TModel, TForeignModel>, new()
    {
    }

    public abstract class BaseBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TDAL> : FoundationBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TDAL> where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TDAL : BaseDAL<TModel, TForeignModel, TForeignModel1>, new()
    {
    }

    public abstract class BaseBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TDAL> : FoundationBLL<TViewModel, TModel, TForeignModel, TForeignModel1, TForeignModel2, TDAL> where TViewModel : IFoundationViewModel, new() where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TDAL : BaseDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2>, new()
    {
    }
}