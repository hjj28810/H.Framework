using H.Framework.Data.ORM.Foundations;

namespace H.Framework.Data.ORM
{
    public abstract class BaseDAL<TModel> : FoundationDAL<TModel> where TModel : IFoundationModel, new()
    {
        public BaseDAL()
        { }
    }

    public abstract class BaseDAL<TModel, TForeignModel> : FoundationDAL<TModel, TForeignModel> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new()
    {
        public BaseDAL()
        { }
    }

    public abstract class BaseDAL<TModel, TForeignModel, TForeignModel1> : FoundationDAL<TModel, TForeignModel, TForeignModel1> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new()
    {
        public BaseDAL()
        { }
    }

    public abstract class BaseDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2> : FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new()
    {
        public BaseDAL()
        { }
    }

    public abstract class BaseDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> : FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new()
    {
        public BaseDAL()
        { }
    }

    public abstract class BaseDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> : FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new()
    {
        public BaseDAL()
        { }
    }

    public abstract class BaseDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> : FoundationDAL<TModel, TForeignModel, TForeignModel1, TForeignModel2, TForeignModel3, TForeignModel4, TForeignModel5> where TModel : IFoundationModel, new() where TForeignModel : IFoundationModel, new() where TForeignModel1 : IFoundationModel, new() where TForeignModel2 : IFoundationModel, new() where TForeignModel3 : IFoundationModel, new() where TForeignModel4 : IFoundationModel, new() where TForeignModel5 : IFoundationModel, new()
    {
        public BaseDAL()
        { }
    }
}