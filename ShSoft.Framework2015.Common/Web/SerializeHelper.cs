using System;
using System.Collections.Generic;

namespace ShSoft.Framework2015.Common.Web
{
    public static class SerializeHelper
    {

        public static string Serializer<T>(this T o)
        {
            return JsonHelper.JsonSerializer<T>(o);
        }

        public static string Serializer<T>(this T o, IEnumerable<Type> types)
        {
            return JsonHelper.JsonSerializer(o,types);
        }
        public static T Deserialize<T>(this string str)
        {
            return JsonHelper.JsonDeserialize<T>(str);
        }
    }
}