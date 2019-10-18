using Autofac;
using Autofac.Core;
using System;

namespace H.Framework.NETCore.IoC.Autofac
{
    public interface IIoCManager
    {
        IContainer Container { get; }

        bool IsRegistered(Type serviceType);

        object Resolve(Type type);

        T Resolve<T>(string key = "") where T : class;

        T Resolve<T>(params Parameter[] parameters) where T : class;

        T[] ResolveAll<T>(string key = "");

        object ResolveOptional(Type serviceType);

        object ResolveUnregistered(Type type);

        T ResolveUnregistered<T>() where T : class;

        void InitScope();

        ILifetimeScope Scope { get; set; }

        bool TryResolve(Type serviceType, out object instance);
    }
}