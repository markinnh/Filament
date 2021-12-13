using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Filament.WPF
{
    public static class Extensions
    {
        public static T GetCustomAttribute<T>(this MemberInfo member) where T:Attribute
        {
            return (T)member.GetCustomAttributes(typeof(T)).FirstOrDefault();
        }
    }
}
