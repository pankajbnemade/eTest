using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ERP.Models.Helpers
{
    public static class EnumHelper
    {
        //////////////////
        ///
        // To get enum value (int)Enum.Name
        // To get enum name Enum.Name.tostring()

        //////////////////


        /// <summary>
        /// Method for retrieve the description on the enum.
        /// e.g. [Description("Ready For Submission")]
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="enumValue">The Enumeration</param>
        /// <returns>
        /// Returns a string representing the friendly name.
        /// </returns>
        public static string GetDescription(Enum enumValue)
        {
            Type type = enumValue.GetType();

            MemberInfo[] memInfo = type.GetMember(enumValue.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return enumValue.ToString();
        }

        public static IList<EnumModel> GetEnumListFor<T>()
        {
            IList<EnumModel> enumModelList = new List<EnumModel>();

            enumModelList = ((T[])Enum.GetValues(typeof(T))).Select(c => new EnumModel()
            {
                Value = Convert.ToInt32(c),
                Name = Convert.ToString(c),
                Description = GetEnumDescription<T>(Convert.ToString(c))
            }).ToList();

            return enumModelList; // returns.
        }

        public static string GetEnumDescription<T>(string value)
        {
            Type type = typeof(T);
            var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            if (name == null)
            {
                return string.Empty;
            }
            var field = type.GetField(name);
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return customAttribute.Length > 0 ? ((DescriptionAttribute)customAttribute[0]).Description : name;
        }

        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }

    }

    /// <summary>
    /// emun model.
    /// </summary>
    public class EnumModel
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
