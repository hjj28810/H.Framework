﻿using Autofac;
using Autofac.Configuration;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace H.Framework.NETCore.IoC.Autofac
{
    public class IoCManager : IIoCManager
    {
        private IContainer _container;

        public static IoCManager Instance { get; } = new IoCManager();

        /// <summary>
        /// Ioc容器初始化
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public IServiceProvider Initialize(IServiceCollection services, IConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(Instance).As<IIoCManager>().SingleInstance();
            //所有程序集 和程序集下类型
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");//排除所有的系统程序集、Nuget下载包
            var listAllType = new List<Type>();
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    listAllType.AddRange(assembly.GetTypes().Where(type => type != null));
                }
                catch { }
            }
            //找到所有外部IDependencyRegistrar实现，调用注册
            var registrarType = typeof(IDependencyRegistrar);
            var arrRegistrarType = listAllType.Where(t => registrarType.IsAssignableFrom(t) && t != registrarType).ToArray();
            var listRegistrarInstances = new List<IDependencyRegistrar>();
            foreach (var drType in arrRegistrarType)
            {
                listRegistrarInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            }
            //排序
            listRegistrarInstances = listRegistrarInstances.OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in listRegistrarInstances)
            {
                dependencyRegistrar.Register(builder, listAllType);
            }

            ////注册ITransientDependency实现类
            //var dependencyType = typeof(ITransientDependency);
            //var arrDependencyType = listAllType.Where(t => dependencyType.IsAssignableFrom(t) && t != dependencyType).ToArray();
            //builder.RegisterTypes(arrDependencyType)
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope()
            //    .PropertiesAutowired().EnableInterfaceInterceptors();

            //foreach (Type type in arrDependencyType)
            //{
            //    if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
            //    {
            //        builder.RegisterType(type).As(type.BaseType)
            //            .InstancePerLifetimeScope()
            //            .PropertiesAutowired();
            //    }
            //}

            ////注册ISingletonDependency实现类
            //var singletonDependencyType = typeof(ISingletonDependency);
            //var arrSingletonDependencyType = listAllType.Where(t => singletonDependencyType.IsAssignableFrom(t) && t != singletonDependencyType).ToArray();
            //builder.RegisterTypes(arrSingletonDependencyType)
            //    .AsImplementedInterfaces()
            //    .SingleInstance()
            //    .PropertiesAutowired();

            //foreach (Type type in arrSingletonDependencyType)
            //{
            //    if (type.IsClass && !type.IsAbstract && !type.BaseType.IsInterface && type.BaseType != typeof(object))
            //    {
            //        builder.RegisterType(type).As(type.BaseType)
            //            .SingleInstance()
            //            .PropertiesAutowired();
            //    }
            //}
            var module = new ConfigurationModule(configuration);
            builder.RegisterModule(module);
            builder.Populate(services);
            _container = builder.Build();
            return new AutofacServiceProvider(_container);
        }

        /// <summary>
        /// Gets a container
        /// </summary>
        public virtual IContainer Container => _container;

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(string key = "", ILifetimeScope scope = null) where T : class
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<T>();
            }
            return scope.ResolveKeyed<T>(key);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual T Resolve<T>(params Parameter[] parameters) where T : class
        {
            var scope = Scope();
            return scope.Resolve<T>(parameters);
        }

        /// <summary>
        /// Resolve
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object Resolve(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.Resolve(type);
        }

        /// <summary>
        /// Resolve all
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved services</returns>
        public virtual T[] ResolveAll<T>(string key = "", ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            if (string.IsNullOrEmpty(key))
            {
                return scope.Resolve<IEnumerable<T>>().ToArray();
            }
            return scope.ResolveKeyed<IEnumerable<T>>(key).ToArray();
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual T ResolveUnregistered<T>(ILifetimeScope scope = null) where T : class
        {
            return ResolveUnregistered(typeof(T), scope) as T;
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveUnregistered(Type type, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var parameterInstances = new List<object>();
                    foreach (var parameter in parameters)
                    {
                        var service = Resolve(parameter.ParameterType, scope);
                        if (service == null) throw new Exception("Unknown dependency");
                        parameterInstances.Add(service);
                    }
                    return Activator.CreateInstance(type, parameterInstances.ToArray());
                }
                catch (Exception)
                {
                }
            }
            throw new Exception("No constructor  was found that had all the dependencies satisfied.");
        }

        /// <summary>
        /// Try to resolve srevice
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <param name="instance">Resolved service</param>
        /// <returns>Value indicating whether service has been successfully resolved</returns>
        public virtual bool TryResolve(Type serviceType, ILifetimeScope scope, out object instance)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.TryResolve(serviceType, out instance);
        }

        /// <summary>
        /// Check whether some service is registered (can be resolved)
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Result</returns>
        public virtual bool IsRegistered(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.IsRegistered(serviceType);
        }

        /// <summary>
        /// Resolve optional
        /// </summary>
        /// <param name="serviceType">Type</param>
        /// <param name="scope">Scope; pass null to automatically resolve the current scope</param>
        /// <returns>Resolved service</returns>
        public virtual object ResolveOptional(Type serviceType, ILifetimeScope scope = null)
        {
            if (scope == null)
            {
                //no scope specified
                scope = Scope();
            }
            return scope.ResolveOptional(serviceType);
        }

        /// <summary>
        /// Get current scope
        /// </summary>
        /// <returns>Scope</returns>
        public virtual ILifetimeScope Scope()
        {
            try
            {
                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope();
            }
            catch (Exception)
            {
                //we can get an exception here if RequestLifetimeScope is already disposed
                //for example, requested in or after "Application_EndRequest" handler
                //but note that usually it should never happen

                //when such lifetime scope is returned, you should be sure that it'll be disposed once used (e.g. in schedule tasks)
                return Container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag);
            }
        }
    }
}