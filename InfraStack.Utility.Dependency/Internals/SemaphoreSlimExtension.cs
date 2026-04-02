using System;
using System.Threading;

namespace InfraStack.Utility.Dependency.Internals
{
    internal static class SemaphoreSlimExtension
    {
        public static void Lock(this SemaphoreSlim Signal, Action Act)
        {
            Signal.Wait();
            try
            {
                Act();
            }
            finally
            {
                Signal.Release();
            }
        }
    }
}
