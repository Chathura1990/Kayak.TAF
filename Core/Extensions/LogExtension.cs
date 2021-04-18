using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Extensions
{
    public static class LogExtension
    {
        private static Logger _log = Logger.Instance;

        public static bool IsSimpleType<T>(this T o)
        {
            TypeInfo typeInfo = o.GetType().GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimpleType(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || o.GetType().Equals(typeof(string))
              || o.GetType().Equals(typeof(decimal));
        }

        public static T GetPropertiesToLog<T>(this T obj)
        {
            Type type = obj.GetType();
            if (IsList(obj))
            {
                _log.Debug($"[List Count] {((IList)obj).Count}");
                foreach (object el in (IList)obj)
                {
                    _log.Debug("[List Item]");
                    el.GetPropertiesToLog();
                }
            }
            else if (IsDictionary(obj))
            {
                _log.Debug($"[Dictionary Count] {((IDictionary)obj).Count}");
                foreach (object el in (IDictionary)obj)
                {
                    _log.Debug("[Dictionary Item]");
                    el.GetPropertiesToLog();
                }
            }
            else
            {
                obj.ToLog();
            }

            return obj;
        }

        private static void ToLog<T>(this T obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo p in properties)
            {
                object value = p.GetValue(obj, null);
                if (value != null)
                {
                    _log.Debug($"[VARIABLE PROPERTY] {p.Name} = {value}");
                }
            }
        }

        private static bool IsList(object o)
        {
            if (o == null)
            {
                return false;
            }

            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        private static bool IsDictionary(object o)
        {
            if (o == null)
            {
                return false;
            }

            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }
    }
}