using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionTestProject
{

    public interface IService
    {
        void Do();
    }

    public interface IService2
    {
        void Eat();
    }

    public class Service1 : IService
    {
        public Service1()
        {

        }

        public void Do()
        {

        }
    }

    public class Service2 : IService
    {
        public Service2()
        {

        }

        public void Do()
        {

        }
    }

    public class Service3 : IService
    {
        public IService2 _service2;

        public Service3(IService2 service2)
        {
            _service2 = service2;
        }

        public void Do()
        {

        }
    }

    public class Service21 : IService2
    {
        public Service21()
        {

        }

        public void Eat()
        {

        }
    }

    public interface IOpenService<T>
    {
        void Do(T par);
    }

    public class OpenService<T> : IOpenService<T>
    {
        public void Do(T par)
        { 
        
        }
    }
}
