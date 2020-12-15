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
        public DependencyLifetime Lifetime { get; private set; }
        public Dictionary<Type, List<Type>> Dependecies { get; private set; }

        public DependenciesConfiguration(DependencyLifetime lifetime)
        {
            Lifetime = lifetime;
            Dependecies = new Dictionary<Type, List<Type>>();
        }

        public void Register<T, U>()
            where T : class
            where U : T
        {
            if (!typeof(U).IsAbstract)
            {
                if (Dependecies.ContainsKey(typeof(T)))
                {
                    Dependecies.GetValueOrDefault(typeof(T)).Add(typeof(U));
                }
                else
                {
                    Dependecies.Add(typeof(T), new List<Type> { typeof(U) });
                }
            }
        }

        public void Register(Type tDependency, Type tImplementation)
        {
            if (!tImplementation.IsAbstract)
            {
                if (Dependecies.ContainsKey(tDependency))
                {
                    Dependecies.GetValueOrDefault(tDependency).Add(tImplementation);
                }
                else
                {
                    Dependecies.Add(tDependency, new List<Type> { tImplementation });
                }
            }
        }
    }
}
