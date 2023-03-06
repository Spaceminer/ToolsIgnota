using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsIgnota.Data.Extensions
{
    public static class LinqExtensions
    {
        public static void Iter<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            int i = 0;
            foreach(T item in enumerable)
            {
                action(item, i++);
            }
        }

        public static void Iter<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }
    }
}
