namespace H.Framework.Core.Mapping
{
    public interface ICustomMap<T>
    {
        void MapFrom(T source);
    }

    public interface ICustomMap
    {
        void MapFrom(object source);
    }
}