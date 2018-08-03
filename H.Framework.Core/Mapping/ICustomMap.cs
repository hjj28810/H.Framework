using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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