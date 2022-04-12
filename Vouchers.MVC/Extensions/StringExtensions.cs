using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vouchers.MVC.Extensions
{
    public static class StringExtensions
    {
        public static string ToControllerNameOnly(this string text) => Regex.Replace(text, @"(C|c)ontroller", @"");
    }
}
