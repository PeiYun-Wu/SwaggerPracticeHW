using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

#pragma warning disable 1591
namespace CommonModule
{
    /// <summary>
    /// 擴充Enum功能,加入自訂義Property
    /// Coder:Mou
    /// Date:2015/09/14
    /// Modify By Kngiht
    /// Date:2017/02/18
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed.")]
    public class EnumDetailsAttribute : Attribute
    {
        /// <summary>
        /// 可擴充自訂義Property
        /// </summary>
        public string Description { get; set; }

        public EnumDetailsAttribute(string desc)
        {
            Description = desc;
        }
    }

    /// <summary>
    /// 擴充功能,取得自訂義Property
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed.")]
    public static class EnumExtension
    {
        public static string Description(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            var attrs = (EnumDetailsAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDetailsAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                return attrs[0].Description;
            }

            return string.Empty;
        }

        public static int Value(this Enum value)
        {
            object val = value as object;
            return (int)val;
        }

        public static string Key(this Enum value)
        {
            return value.ToString();
        }

        public static List<T> GetValues<T>()
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<T> values = new List<T>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add((T)value);
            }

            return values;
        }

        public static List<object> GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values;
        }

        public static T GetEnumByValue<T>(int value)
            where T : struct
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            var items = GetValues<T>();
            foreach (var item in items)
            {
                object val = item as object;
                if ((int)val == value)
                {
                    return item;
                }
            }

            throw new Exception("The value doesn't exist");
        }

        public static T GetEnumByDesc<T>(string value)
            where T : struct
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            var items = GetValues<T>();
            foreach (var item in items)
            {
                Enum val = item as Enum;
                if (val.Description() == value)
                {
                    return item;
                }
            }

            throw new Exception("The Desc doesn't exist");
        }

        public static T GetEnumByKey<T>(string value)
            where T : struct
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            var items = GetValues<T>();
            foreach (var item in items)
            {
                if (item.ToString() == value)
                {
                    return item;
                }
            }

            throw new Exception("The Key doesn't exist");
        }
    }
}
