using System;
using System.Linq;
using JetBrains.Annotations;

namespace Safir.Common
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class TypeExtensions
    {
        public static bool IsAssignableToGeneric(this Type given, Type generic)
        {
            if (given.GetInterfaces().Any(@interface =>
                @interface.IsGenericType && @interface.GetGenericTypeDefinition() == generic))
            {
                return true;
            }

            if (given.IsGenericType && given.GetGenericTypeDefinition() == generic)
            {
                return true;
            }

            var baseType = given.BaseType;

            return baseType != null && IsAssignableToGeneric(baseType, generic);
        }
    }
}
