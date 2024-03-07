using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class ObjectConverterExtensions
    {
        public static T2 Map<T1, T2>(this T1 obj) where T2 : new()
        {
            var result = new T2();
            var properties = typeof(T1).GetProperties();
            foreach (var property in properties)
            {
                var prop = typeof(T2).GetProperty(property.Name);
                if (prop != null && prop.PropertyType == property.PropertyType)
                {
                    prop.SetValue(result, property.GetValue(obj));
                }
            }
            return result;
        }
    }
}
