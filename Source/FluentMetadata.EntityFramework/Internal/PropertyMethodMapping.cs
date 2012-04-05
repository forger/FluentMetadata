using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentMetadata.EntityFramework.Internal
{
    internal static class PropertyMethodMapping
    {
        internal static MethodInfo GetPropertyMappingMethod(Type configurationType, Type instanceType, Type propertyType)
        {
            var expressionFuncType = CreateExpressionFuncTypeOf(instanceType, propertyType);

            return (from methodInfo in configurationType.GetMethods()
                    let parameters = methodInfo.GetParameters()
                    where methodInfo.Name == "Property" &&
                          parameters[0].ParameterType == expressionFuncType
                    select methodInfo).FirstOrDefault();
        }

        static Type CreateExpressionFuncTypeOf(Type instanceType, Type propertyType)
        {
            var funcType = typeof(Func<,>).MakeGenericType(instanceType, propertyType);
            return typeof(Expression<>).MakeGenericType(funcType);
        }
    }
}