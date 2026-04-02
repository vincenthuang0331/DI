using InfraStack.Utility.Dependency.Implementations;
using InfraStack.Utility.Dependency.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace InfraStack.Utility.Dependency.Tests
{
    public class DependencyTest
    {
        private readonly IFruit fruit = null!;

        [Fact]
        public void PropertyGetOnlyTest()
        {
            using var Di = new DefaultDependencyInjector(new DiPropertyTestModel.PropertyRegistration());
            var a = Di.Resolve<DiPropertyTestModel.ParentClass>();
            var b = Di.Resolve<DiPropertyTestModel.ChildClass>();

            Assert.NotNull(a?.MyProperty1);
            Assert.NotNull(a.MyProperty2);
            Assert.NotNull(b?.MyProperty1);
            Assert.NotNull(b.MyProperty2);
        }

        [Fact]
        public void MainTest()
        {
            ResolveTest();
            DependencyInjectorHashCountTest();
        }

        private void ResolveTest()
        {
            var Registration = new RegistrationForTest();
            Registration.RegisterTypeForTest();
            using var Di = new DefaultDependencyInjector(Registration);
            Di.Inject(this);
            Assert.Equal("apple", fruit.GetName());
            Assert.Equal("apple has `2` leaves.", fruit.ToString());
            Assert.Equal("apple", Di.Resolve<IFruit>()?.GetName());
        }

        private void DependencyInjectorHashCountTest()
        {
            var Registration = new RegistrationForTest();
            Registration.RegisterTypeForTest();
            using var Di = new DefaultDependencyInjector(Registration);
            var DIHash = (ICollection<int>)Di.GetType().GetField("_Hashes", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(Di)!;

            Assert.Empty(DIHash);
            var leaf = Di.Resolve<ILeaf>();
            Assert.Single(DIHash);
            var fruit = Di.Resolve<IFruit>();
            Assert.Equal(2, DIHash.Count);
            fruit = null;
            Registration.RemoveTypeObject(typeof(IFruit));
            GC.Collect();
            Di.ResetHashes();
            Assert.Empty(DIHash);
        }

        internal class RegistrationForTest : IRegistration
        {
            public bool IsRegistered(Type type)
            {
                return true;
            }

            public object Resolve(Type type)
            {
                if (TypeTypeMap.ContainsKey(type))
                {
                    return TypeObjectMap.ContainsKey(type) ?
                        TypeObjectMap[type] :
                        TypeObjectMap[type] = Activator.CreateInstance(TypeTypeMap[type])!;
                }

                throw new Exception("Resolve failed");
            }

            private readonly Dictionary<Type, Type> TypeTypeMap = new();
            private readonly Dictionary<Type, object> TypeObjectMap = new();
            public void Register<T, R>() where R : T, new()
            {
                TypeTypeMap[typeof(T)] = typeof(R);
            }

            public void RegisterTypeForTest()
            {
                Register<IFruit, Apple>();
                Register<ILeaf, AppleLeaf>();
            }
            public void RemoveTypeObject(Type type)
            {
                TypeObjectMap.Remove(type);
            }
        }

        internal interface IFruit { string GetName(); }

        internal class Apple : IFruit
        {
            private readonly ILeaf Leaf = null!;
            public string GetName()
            {
                return "apple";
            }

            public override string ToString()
            {
                return $"{GetName()} has `{Leaf.Count()}` leaves.";
            }
        }

        internal interface ILeaf { int Count(); }

        internal class AppleLeaf : ILeaf
        {
            public int Count()
            {
                return 2;
            }
        }
    }
}
