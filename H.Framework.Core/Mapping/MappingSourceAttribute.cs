using System;

namespace H.Framework.Core.Mapping
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MappingSourceAttribute : Attribute
    {
        public MappingSourceAttribute(string name)
            : this(null, name)
        { }

        public MappingSourceAttribute(Type type, string name)
        {
            this.SourceType = type;
            this.Name = name;
        }

        public Type SourceType { get; }

        public string Name { get; }
    }

    public class MappingIgnoreAttribute : Attribute
    { }
}