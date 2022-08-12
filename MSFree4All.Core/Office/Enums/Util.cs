using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Core
{
    public static class Util
    {

        /// <summary>
        /// Get the all values of an <see cref="Enum"/>.
        /// </summary>
        public static List<T> GetEnumList<T>()
        {
            T[] array = (T[])Enum.GetValues(typeof(T));
            List<T> list = new(array);
            return list;
        }
    }
}
