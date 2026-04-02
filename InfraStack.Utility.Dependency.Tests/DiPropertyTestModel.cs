using InfraStack.Utility.Dependency.Interfaces;
using System;

namespace InfraStack.Utility.Dependency.Tests
{
    static class DiPropertyTestModel
    {
        internal class ParentClass
        {
            public virtual IService MyProperty1 { get; } = null!;
            public virtual IService MyProperty2 { get; } = null!;
        }

        internal class ChildClass : ParentClass
        {
            public override IService2 MyProperty1 { get; } = null!;
        }

        internal interface IService
        {

        }

        internal interface IService2 : IService { }

        class ConcreteClass : IService { }
        class MoreConcreteClass : ConcreteClass, IService2 { }

        internal class PropertyRegistration : IRegistration
        {
            public bool IsRegistered(Type type) => true;

            public object Resolve(Type type)
            {
                if (type == typeof(ParentClass)) return new ParentClass();
                if (type == typeof(ChildClass)) return new ChildClass();
                if (type == typeof(IService)) return new ConcreteClass();
                if (type == typeof(IService2)) return new MoreConcreteClass();

                throw new Exception("Resolve failed");
            }
        }
    }
}
