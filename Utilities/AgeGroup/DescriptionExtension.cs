using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.AgeGroup
{
    /// <summary>
    /// Класс расширения для Description.
    /// </summary>
    public static class DescriptionExtension
    {
        public static string GetDescription(this Enum enumType)
        {
            Type type = enumType.GetType();

            MemberInfo[] memInfo = type.GetMember(enumType.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(Description), false);

                if (attrs != null && attrs.Length > 0)

                    return ((Description)attrs[0]).Text;
            }

            return enumType.ToString();
        }
    }
}
