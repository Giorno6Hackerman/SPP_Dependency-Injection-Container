using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionLibrary
{
    public class ImplementationInfo
    {
        public DependencyLifetime Lifetime { get; private set; }
        public Type ImplementationType { get; private set; }
        private object _implementation;

        public ImplementationInfo(Type type, DependencyLifetime lifetime)
        {
            ImplementationType = type;
            Lifetime = lifetime;
        }

        public object GetInstance()
        {
            return _implementation;
        }

        public void SetInstance(object implementation)
        {
            _implementation = implementation;
        }
    }
}
