using Autofac;
using System;
using System.Collections.Generic;

namespace H.Framework.NETCore.IoC.Autofac
{
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="config">Config</param>
        void Register(ContainerBuilder builder, List<Type> listType);

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}