using System;
using JetBrains.Annotations;

namespace LongestCommonPrefix
{
    public class LongestCommonPrefixCalculator
    {
        [NotNull]
        private readonly SuffixesArrayCalculator _suffixesArrayCalculator;

        private readonly int _stringLength;

        // Complexity - O( |str| * ln(|str|) )
        public LongestCommonPrefixCalculator([NotNull] string str)
        {
            _suffixesArrayCalculator = new SuffixesArrayCalculator(str);
            _stringLength = str.Length;
        }

        //Complexity - O( ln(|str|) )
        public int Get(int i, int j)
        {
            var result = 0;
            for (var k = GetMaxLnSize(_stringLength - Math.Max(i, j)); k >= 0; --k)
            {
                var currentSize = 1 << k;
                if (Math.Max(i, j) + currentSize > _stringLength)
                {
                    continue;
                }

                var orderedClasses = _suffixesArrayCalculator.GetOrderedClasses(k);
                if (orderedClasses[i] == orderedClasses[j])
                {
                    result += currentSize;
                    i += currentSize;
                    j += currentSize;
                }
            }

            return result;
        }

        //Complexity - O(1)
        private static int GetMaxLnSize(int n)
        {
            int i;
            // ReSharper disable once EmptyEmbeddedStatement
            for (i = 0; (n & 1) != 0; n >>= 1, ++i) ;
            return i;
        }
    }
}