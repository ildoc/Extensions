using System;
using System.ComponentModel;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumerationValue) where T : Enum
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
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
    }
}
