using System;

namespace DependencyInjectionLibrary
{
    public class DependencyProvider
    {
        public DependencyProvider(DependenciesConfiguration dependencies)
        {
            
        }

        public U Resolve<T, U>() where U : T
        {
            return default(U);
        }
    }
}
