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

        public DependencyProvider(DependenciesConfiguration depConfigs)
        {
            _depConfigs = depConfigs;         
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
            if (typeof(T).GetInterfaces().Contains(typeof(IEnumerable)))
            { 
                Type genericType = typeof(T).GetGenericArguments()[0];
                List<object> createdImpls = new List<object>();
                _depConfigs.Dependecies.TryGetValue(typeof(T), out implementations);
                foreach (var impl in implementations)
                {
                    createdImpls.Add(CreateDependency(typeof(T), impl));
                }
                return createdImpls.AsEnumerable();
            }

            if (typeof(T).IsGenericType)
            {
                Type genericBase = typeof(T).GetGenericTypeDefinition();
                Type genericArgument = typeof(T).GetGenericArguments()[0];

                if (!ValidateConfiguration(genericBase) || !ValidateConfiguration(genericArgument))
                {
                    // bad configuration
                    return null;
                }
                _depConfigs.Dependecies.TryGetValue(genericBase, out implementations);
                if (implementations.Count > 1)
                {
                    // must be 1
                }
                ImplementationInfo impl = new ImplementationInfo(implementations[0].ImplementationType.MakeGenericType(genericArgument),
                                                                 implementations[0].Lifetime);
                return (CreateDependency(genericArgument, impl) as T);
            }

            _depConfigs.Dependecies.TryGetValue(typeof(T), out implementations);
            if (implementations.Count > 1)
            { 
                // must be 1
            }
            return (CreateDependency(typeof(T), implementations[0]) as T);
        }

        private IEnumerable<object> CreateDependency(Type type, List<ImplementationInfo> implementations)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length < 1)
            { 
                // no public constructors
            }
            ConstructorInfo constructor = ChooseConstructor(constructors);
            ParameterInfo[] parameters = constructor.GetParameters();
            if (parameters.All(p => _depConfigs.Dependecies.ContainsKey(p.ParameterType)))
            { 
                
            }

            
            return null;
        }

        private object CreateDependency(Type type, ImplementationInfo implementation)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            if (constructors.Length < 1)
            {
                // no public constructors
            }
            ConstructorInfo constructor = ChooseConstructor(constructors);
            ParameterInfo[] parameters = constructor.GetParameters();
            if (parameters.All(p => _depConfigs.Dependecies.ContainsKey(p.ParameterType)))
            {

            }


            return null;
        }

        private ConstructorInfo ChooseConstructor(ConstructorInfo[] constructors)
        {
            return constructors.OrderByDescending(c => c.GetParameters().Length).First();
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

                if (implementations.Count > 1 && !dependency.Key.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    // must be enumerable
                    return false;
                }
            }

            if (!_depConfigs.Dependecies.ContainsKey(type))
            {
                return false;
            }    

            return true;
        }
    }
}
