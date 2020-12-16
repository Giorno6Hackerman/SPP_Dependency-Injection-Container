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

            _depConfigs.Dependecies.TryGetValue(typeof(T), out implementations);
            

            return null;
        }

        private T CreateDependency<T>(List<Type> implementations)
        {
            Type depType = typeof(T);
            ConstructorInfo[] constructors = depType.GetConstructors();
            if (constructors.Length < 1)
            { 
                // no public constructors
            }
            ConstructorInfo constructor = ChooseConstructor(constructors);
            ParameterInfo[] parameters = constructor.GetParameters();
            if (parameters.All(p => _depConfigs.Dependecies.ContainsKey(p.ParameterType)))
            { 
                
            }

            
            return default(T);
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
            return true;
        }
    }
}
