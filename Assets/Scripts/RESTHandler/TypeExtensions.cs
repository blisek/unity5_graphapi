using System;
using System.Reflection;

namespace Assets.Scripts.RESTHandler
{
    public static class TypeExtensions
    {
        public static Attribute GetCustomAttribute(this MethodInfo methodInfo, Type attributeType)
        {
            var customAttributes = methodInfo.GetCustomAttributes(attributeType, false);
            if (customAttributes.Length > 0)
                return customAttributes[0] as Attribute;
            return null;
        }
    }
}
