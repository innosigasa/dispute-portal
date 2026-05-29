using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DisputePortal.Application.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescriptionOrName<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            var type = typeof(TEnum);
            var memInfo = type.GetMember(enumValue.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0
                ? ((DescriptionAttribute)attributes[0]).Description
                : enumValue.ToString();
        }
    }
}
