using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            }

            List<Type> implementations;

            _depConfigs.Dependecies.TryGetValue(typeof(T), out implementations);
            

            return null;
        }

        private bool ValidateConfiguration(Type type)
        {
            List<Type> implementations;

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
