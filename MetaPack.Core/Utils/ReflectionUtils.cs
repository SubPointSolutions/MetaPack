﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MetaPack.Core.Utils
{
    public class PropResult
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Type ObjectType { get; set; }
    }

    /// <summary>
    /// Reflection helpers.
    /// Internal usage only.
    /// </summary>
    public static class ReflectionUtils
    {
        #region methods

        #region set helpers

        public static void SetNonPublicFieldValue(object obj, string fieldName, object value)
        {
            var prop = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (prop != null)
                prop.SetValue(obj, value);
        }


        public static void SetPropertyValue(object obj, string propName, object value)
        {
            var prop = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);

            if (prop != null)
                prop.SetValue(obj, value);
        }

        #endregion

        #region get prop methods

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TType>(object obj)
        {
            return GetPropertiesWithCustomAttribute(obj.GetType(), typeof(TType), false);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TType>(object obj, bool inherit)
        {
            return GetPropertiesWithCustomAttribute(obj.GetType(), typeof(TType), inherit);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TType>(Type type)
        {
            return GetPropertiesWithCustomAttribute(type, typeof(TType), false);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TType>(Type type, bool inherit)
        {
            return GetPropertiesWithCustomAttribute(type, typeof(TType), inherit);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute(object obj, Type attributeType)
        {
            return GetPropertiesWithCustomAttribute(obj, attributeType, false);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute(object obj, Type attributeType, bool inherit)
        {
            return GetPropertiesWithCustomAttribute(obj.GetType(), attributeType, inherit);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute(Type type, Type attributeType)
        {
            return GetPropertiesWithCustomAttribute(type, attributeType, false);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute(Type type, Type attributeType, bool inherit)
        {
            return type
                           .GetProperties()
                           .Where(p => p.GetCustomAttributes(attributeType, inherit).Any());
        }
        #endregion

        public static IEnumerable<object> GetStaticFieldValues(Type staticClassType)
        {
            return GetStaticFieldValues<object>(staticClassType);
        }

        public static IEnumerable<TValueType> GetStaticFieldValues<TValueType>(Type staticClassType)
        {
            var result = new List<TValueType>();

            foreach (var p in staticClassType.GetFields())
            {
                var value = p.GetValue(null);

                if (value != null && value.GetType() == typeof(TValueType))
                    result.Add((TValueType)value);
            }

            return result;
        }

        public static IEnumerable<Type> GetTypesFromCurrentDomain<TType>()
        {
            return GetTypesFromAssemblies<TType>(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IEnumerable<Type> GetTypesFromAssembly<TType>(Assembly assembly)
        {
            return GetTypesFromAssemblies<TType>(new[] { assembly });
        }

        public static IEnumerable<Type> GetTypesFromAssemblies<TType>(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetTypes())
                       .Where(t => typeof(TType).IsAssignableFrom(t) && !t.IsAbstract);
        }

        public static PropResult GetExpressionValue<TSource, TProperty>(this TSource source,
           Expression<Func<TSource, TProperty>> exp)
        {
            Type type = typeof(TSource);

            if (exp.Body is MethodCallExpression)
            {
                var member = exp.Body as MethodCallExpression;
                var methodResult = Expression.Lambda(member, exp.Parameters.ToArray()).Compile().DynamicInvoke(source);

                var result = new PropResult();

                result.Name = member.ToString();
                result.Value = methodResult;
                result.ObjectType = source.GetType();

                return result;
            }

            if (exp.Body is UnaryExpression)
            {
                var member = exp.Body as UnaryExpression;
                var methodResult = Expression.Lambda(member, exp.Parameters.ToArray()).Compile().DynamicInvoke(source);

                var result = new PropResult();

                result.Name = member.ToString();
                result.Value = methodResult;
                result.ObjectType = source.GetType();

                return result;
            }

            if (exp.Body is MemberExpression)
            {
                var member = exp.Body as MemberExpression;
                var propInfo = member.Member as PropertyInfo;

                var result = new PropResult();

                result.Name = propInfo.Name;
                result.Value = propInfo.GetValue(source, null);
                result.ObjectType = source.GetType();

                return result;
            }

            throw new NotImplementedException("GetExpressionValue");
        }

        public static object GetStaticPropertyValue(Type type, string propName)
        {
            var prop = type.GetProperties(BindingFlags.Instance |
                                                  BindingFlags.NonPublic |
                                                  BindingFlags.Public)
                                   .FirstOrDefault(p => p.Name.ToUpper() == propName.ToUpper());

            if (prop == null)
                throw new Exception(string.Format("Can't find prop: [{0}] in obj of type:[{1}]", propName, type));

            return prop.GetValue(null, null);
        }

        public static object GetPropertyValue(object obj, string propName)
        {
            var prop = obj.GetType().GetProperties(BindingFlags.Instance |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.Public)
                                    .FirstOrDefault(p => p.Name.ToUpper() == propName.ToUpper());

            if (prop == null)
                throw new Exception(string.Format("Can't find prop: [{0}] in obj:[{1}]", propName, obj));

            return prop.GetValue(obj, null);
        }

        // AddSupportedUILanguage

        public static object InvokeStaticMethod(Type type, string methodName)
        {
            return InvokeStaticMethod(type, methodName, null);
        }

        public static object InvokeStaticMethod(Type type, string methodName, object[] parameters)
        {
            var method = type.GetMethod(methodName);

            if (method == null)
                throw new Exception(string.Format("Cannot find method by name:[{0}] on object of type:[{1}]",
                    methodName, type));

            return method.Invoke(null, parameters);

        }

        public static object InvokeMethod(object obj, string methodName)
        {
            return InvokeMethod(obj, methodName, null);
        }

        public static object InvokeMethod(object obj, string methodName, object[] parametes)
        {
            MethodInfo method = null;

            if (parametes == null)
            {
                method = obj.GetType().GetMethods()
                                      .FirstOrDefault(m => m.Name == methodName
                                                    & m.GetParameters().Count() == 0);
            }
            else
            {
                method = obj.GetType().GetMethods()
                                      .FirstOrDefault(m => m.Name == methodName
                                                    & m.GetParameters().Count() == parametes.Count());
            }


            if (method == null)
                throw new Exception(string.Format("Cannot find method by name:[{0}] on object of type:[{1}]",
                    methodName, obj.GetType()));

            return method.Invoke(obj, parametes);
        }

        public static MethodInfo GetMethod(object obj, string methodName)
        {
            var methods = obj.GetType().GetMethods(BindingFlags.Instance |
                                                      BindingFlags.NonPublic |
                                                      BindingFlags.Public);

            return methods.FirstOrDefault(p => p.Name.ToUpper() == methodName.ToUpper());
        }

        public static bool HasMethod(object obj, string methodName)
        {
            return HasMethods(obj, new[] { methodName });
        }

        public static bool HasMethods(object obj, IEnumerable<string> methodNames)
        {
            var methods = obj.GetType().GetMethods(BindingFlags.Instance |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Public);

            foreach (var methodName in methodNames)
            {
                var method = methods.FirstOrDefault(p => p.Name.ToUpper() == methodName.ToUpper());

                if (method == null)
                    return false;
            }

            return true;
        }

        public static bool HasProperty(object obj, string propName)
        {
            return HasProperties(obj, new[] { propName });
        }

        public static bool HasProperties(object obj, IEnumerable<string> propNames)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Instance |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Public);

            foreach (var propName in propNames)
            {
                var prop = props.FirstOrDefault(p => p.Name.ToUpper() == propName.ToUpper());

                if (prop == null)
                    return false;
            }

            return true;
        }

        public static bool HasPropertyPublicSetter(object obj, string propName)
        {
            var prop = obj.GetType().GetProperty(propName);
            if (prop != null)
            {
                return prop.GetSetMethod(false) != null;
            }

            return false;
        }

        public static IEnumerable<Type> GetAllTypesFromCurrentAppDomain()
        {
            return GetAllTypesFromAppDomain(AppDomain.CurrentDomain);
        }

        public static IEnumerable<Type> GetAllTypesFromAppDomain(AppDomain domain)
        {
            return domain.GetAssemblies()
                         .SelectMany(a => a.GetTypes());
        }

        public static Type FindTypeByFullName(IEnumerable<Type> types, string fullName)
        {
            return types.FirstOrDefault(c => c.FullName.ToUpper() == fullName.ToUpper());
        }

        public static Type FindTypeByName(IEnumerable<Type> types, string name)
        {
            return types.FirstOrDefault(c => c.Name.ToUpper() == name.ToUpper());
        }

        public static IEnumerable<Assembly> GetAllAssembliesFromCurrentAppDomain()
        {
            return GetAllAssembliesAppDomain(AppDomain.CurrentDomain);
        }

        public static IEnumerable<Assembly> GetAllAssembliesAppDomain(AppDomain domain)
        {
            return domain.GetAssemblies()
                         .OrderBy(a => a.FullName);
        }

        public static Assembly FindAssemblyByFullName(IEnumerable<Assembly> assemblies, string fullName)
        {
            return assemblies.FirstOrDefault(a => a.FullName.ToUpper() == fullName.ToUpper());
        }

        #endregion
    }
}
