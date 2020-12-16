using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependencyInjectionLibrary
{
    public class DependencyProvider
    {
        private DependenciesConfiguration _depConfigs;
        private readonly Dictionary<Type, object> _singletons;
        private static readonly object _lock = new object();

        public DependencyProvider(DependenciesConfiguration depConfigs)
        {
            _depConfigs = depConfigs;
            _singletons = new Dictionary<Type, object>();
        }

        public object Resolve<T>()
            where T : class
        {
            if (!ValidateConfiguration(typeof(T)))
            {
                // bad configuration
                return null;
            }

            List<ImplementationInfo> implementations;
            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            { 
                Type genericType = typeof(T).GetGenericArguments()[0];
                List<object> createdImpls = new List<object>();
                _depConfigs.Dependecies.TryGetValue(genericType, out implementations);
                foreach (var impl in implementations)
                {
                    createdImpls.Add(CreateDependency(impl));
                }
                return createdImpls.AsEnumerable();
            }

            if (typeof(T).IsGenericType)
            {
                Type genericBase = typeof(T).GetGenericTypeDefinition();
                Type genericArgument = typeof(T).GetGenericArguments()[0];

                _depConfigs.Dependecies.TryGetValue(genericBase, out implementations);
                if (implementations.Count > 1)
                {
                    // must be 1
                    return null;
                }
                ImplementationInfo impl = new ImplementationInfo(implementations[0].ImplementationType.MakeGenericType(genericArgument),
                                                                 implementations[0].Lifetime);
                return (CreateDependency(impl) as T);
            }

            _depConfigs.Dependecies.TryGetValue(typeof(T), out implementations);
            if (implementations.Count > 1)
            {
                // must be 1
                return null;
            }
            return (CreateDependency(implementations[0]) as T);
        }

        private object CreateDependency(ImplementationInfo implementation)
        {
            ConstructorInfo[] constructors = implementation.ImplementationType.GetConstructors();
            if (constructors.Length < 1)
            {
                // no public constructors
                return null;
            }
            constructors = constructors.OrderByDescending(c => c.GetParameters().Length).ToArray();

            foreach (var constructor in constructors)
            {
                ParameterInfo[] parameters = constructor.GetParameters();
                List<object> createdParams = new List<object>();
                if (parameters.All(p => _depConfigs.Dependecies.ContainsKey(p.ParameterType)))
                {
                    List<ImplementationInfo> implementations;
                    foreach (var parameter in parameters)
                    {
                        _depConfigs.Dependecies.TryGetValue(parameter.ParameterType, out implementations);
                        if (typeof(IEnumerable).IsAssignableFrom(parameter.ParameterType))
                        {
                            Type genericType = parameter.ParameterType.GetGenericArguments()[0];
                            List<object> createdImpls = new List<object>();
                            foreach (var impl in implementations)
                            {
                                createdImpls.Add(CreateDependency(impl));
                            }
                            createdParams.Add(createdImpls.AsEnumerable());
                        }
                        else
                        {
                            if (implementations.Count == 1)
                            {
                                createdParams.Add(CreateDependency(implementations[0]));
                            }
                        }
                    }

                }
                if (implementation.Lifetime == DependencyLifetime.Singleton)
                {
                    return GetOrCreateSingleton(implementation.ImplementationType, createdParams);
                }
                else
                {
                    return CreateObject(implementation.ImplementationType, createdParams);
                }

            }
            return null;
        }

        private object GetOrCreateSingleton(Type type, List<object> arguments)
        {
            object instance = null;
            if (!_singletons.ContainsKey(type))
            {
                lock (_lock)
                {
                    if (!_singletons.ContainsKey(type))
                    {
                        instance = CreateObject(type, arguments);
                        _singletons.Add(type, instance);

                    }
                }
            }
            else
                return _singletons[type];

            return instance;
        }

        private object CreateObject(Type type, List<object> arguments)
        {
            if (arguments.Count != 0)
                return Activator.CreateInstance(type, arguments.ToArray());
            else
                return Activator.CreateInstance(type);
        }



        private bool ValidateConfiguration(Type type)
        {
            List<ImplementationInfo> implementations;

            if (_depConfigs.Dependecies.Count < 1)
            {
                // configuration is empty
                return false;
            }

            foreach (var dependency in _depConfigs.Dependecies)
            {
                if (!_depConfigs.Dependecies.TryGetValue(dependency.Key, out implementations))
                {
                    // dependency not found
                    return false;
                }

                if (implementations.Count > 1 && !typeof(IEnumerable).IsAssignableFrom(type))
                {
                    // must be enumerable
                    return false;
                }
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                Type genericType = type.GetGenericArguments()[0];
                if (!_depConfigs.Dependecies.ContainsKey(genericType))
                {
                    return false;
                }
            }
            else
                if (type.IsGenericType)
                {
                    Type genericBase = type.GetGenericTypeDefinition();
                    if (!_depConfigs.Dependecies.ContainsKey(genericBase))
                    {
                        return false;
                    }
                }
                else
                    if (!_depConfigs.Dependecies.ContainsKey(type))
                    {
                        return false;
                    }    

            return true;
        }

       
    }
}
