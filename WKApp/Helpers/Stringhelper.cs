using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WKApp.Helpers
{
    public static class StringHelper
    {

        public static string ToCode(this string input)
        {
            string filename;
            var number = Char.ConvertToUtf32(input, 0);
            filename = string.Format("0{0:X}", number);
            return filename;
        }
    }
}
