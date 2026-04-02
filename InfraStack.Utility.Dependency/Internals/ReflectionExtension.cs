using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace InfraStack.Utility.Dependency.Internals
{
    internal static class ReflectionExtension
    {
        public static FieldInfo? GetBackingField(this Type TheType, string PropertyName)
        {
            var FieldName = $"<{PropertyName}>k__BackingField";
            const BindingFlags FieldFlags = BindingFlags.Instance | BindingFlags.NonPublic;

            for (var Current = TheType; Current != null; Current = Current.BaseType)
            {
                var Field = Current.GetField(FieldName, FieldFlags);
                if (Field != null) return Field;
            }

            return null;
        }
    }
}
