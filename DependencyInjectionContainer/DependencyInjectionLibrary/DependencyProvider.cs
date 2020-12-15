using System;

namespace DependencyInjectionLibrary
{
    public class DependencyProvider
    {
        private DependenciesConfiguration _dependencies;

        public DependencyProvider(DependenciesConfiguration dependencies)
        {
            _dependencies = dependencies;         
        }

        public U Resolve<T, U>()
            where T : class
            where U : T
        {
            return default(U);
        }
    }
}
