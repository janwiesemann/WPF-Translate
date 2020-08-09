using System;
using System.Collections.Generic;

namespace de.LandauSoftware.WPFTranslate
{
    internal static class ExtensionMethods
    {
        public static bool Contains<T>(this IEnumerable<T> enumerable, Predicate<T> check)
        {
            if (enumerable == null)
                return true;

            if (check == null)
                throw new ArgumentNullException(nameof(check));

            foreach (T item in enumerable)
            {
                if (check(item))
                    return true;
            }

            return false;
        }

        public static unsafe bool ContainsChar(this string str, params char[] chars)
        {
            if (str == null)
                return false;

            if (chars == null)
                throw new ArgumentNullException(nameof(chars));

            if (chars.Length == 0)
                throw new ArgumentException("List is Empty!");

            fixed (char* strPtr = str)
            {
                char* strStartPtr = strPtr;
                char* strEndPtr = strPtr + str.Length;

                while (strStartPtr < strEndPtr)
                {
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (*strStartPtr == chars[i])
                            return true;
                    }

                    strStartPtr++;
                }
            }

            return false;
        }
    }
}