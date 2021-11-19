using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BRD.Common.Infrastructure
{
    public static class StringExtensions
    {

        /// <summary>
        /// Uppercases the first character of a string
        /// </summary>
        /// <param name="input">The string which first character should be uppercased</param>
        /// <returns>The input string with it'input first character uppercased</returns>
        public static string FirstCharToUpper(this string input)
        {
            return string.IsNullOrEmpty(input)
                ? ""
                : string.Concat(input.Substring(0, 1).ToUpper(), input.Substring(1).ToLower());
        }

        /// <summary>
        /// Formats the string according to the specified mask
        /// </summary>
        /// <param name="input"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static string Mask(this string input, char mask)
        {
            return string.IsNullOrEmpty(input) ? input : new string(mask, input.Length);
        }

        /// <summary>
        /// Is numeric
        /// </summary>
        /// <param name="theValue"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string theValue)
        {
            return long.TryParse(theValue, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out long _);
        }

        /// <summary>
        /// Convert a string to camel case
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static List<string> SplitString(this string base64String, int chunkSize)
        {
            int size = base64String.Length / chunkSize;
            var nineFirst = Enumerable.Range(0, chunkSize - 1)
                .Select(i => base64String.Substring(i * size, size)).ToList();
            var firstLength = nineFirst.Count * size;
            var last =
                base64String.Substring(firstLength, base64String.Length - firstLength);
            nineFirst.Add(last);
            return nineFirst;
        }
    }
}
