using System;
using System.ComponentModel;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static T TryParse<T>(this Enum e, string s) where T : Enum
        {
            Enum.TryParse(e.GetType(), s, out var result);
            return (T)result;
        }

        public static string GetDescription<T>(this T enumerationValue) where T : Enum
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo?.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs?.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        public static T[] GetValues<T>(this Enum e) where T : Enum
        {
            return null;
        }
    }
}
