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
        public Dictionary<Type, List<ImplementationInfo>> Dependecies { get; private set; }

        public DependenciesConfiguration()
        {
            Dependecies = new Dictionary<Type, List<ImplementationInfo>>();
        }

        public void Register<T, U>(DependencyLifetime lifetime)
            where T : class
            where U : T
        {
            if (!typeof(U).IsAbstract)
            {
                if (Dependecies.ContainsKey(typeof(T)))
                {
                    Dependecies.GetValueOrDefault(typeof(T)).Add(new ImplementationInfo(typeof(U), lifetime));
                }
                else
                {
                    Dependecies.Add(typeof(T), new List<ImplementationInfo> { new ImplementationInfo(typeof(U), lifetime)});
                }
            }
        }

        public void Register(Type tDependency, Type tImplementation, DependencyLifetime lifetime)
        {
            if (!tImplementation.IsAbstract)
            {
                if (Dependecies.ContainsKey(tDependency))
                {
                    Dependecies.GetValueOrDefault(tDependency).Add(new ImplementationInfo(tImplementation, lifetime));
                }
                else
                {
                    Dependecies.Add(tDependency, new List<ImplementationInfo> { new ImplementationInfo(tImplementation, lifetime) });
                }
            }
        }
    }
}
