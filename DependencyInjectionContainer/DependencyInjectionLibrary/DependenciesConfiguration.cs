using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionLibrary
{
    public enum DependencyLifetime 
    {
        InstancePerDependency,
        Singleton // multithread
    }

    public class DependenciesConfiguration
    {
        public DependenciesConfiguration()
        { 
        
        }

        public void Register<T, U>(DependencyLifetime lifetime) where U : T
        {

        }

        public void Register(Type tDependency, Type tImplementation)
        { 
        
        }
    }
}
