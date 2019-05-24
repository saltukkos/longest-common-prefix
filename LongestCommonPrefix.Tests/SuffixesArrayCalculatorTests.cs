using System;
using System.Text;
using NUnit.Framework;

namespace LongestCommonPrefix.Tests
{
    [TestFixture]
    public class SuffixesArrayCalculatorTests
    {
        private const int StartChar = 'a';
        private const int EndChar = 'z';

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(1000)]
        [TestCase(100000)]
        public void Build_NotFault(int size)
        {
            var random = new Random(123);
            var stringBuilder = new StringBuilder(size);
            for (var i = 0; i < size; ++i)
            {
                stringBuilder.Append((char)((random.Next() % (EndChar - StartChar + 1)) + StartChar));
            }

            var _ = new SuffixesArrayCalculator(stringBuilder.ToString());
        }

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(1000)]
        public void Build_GetPrefixesArray_StringsSorted(int size)
        {
            var random = new Random(123);
            var stringBuilder = new StringBuilder(size);
            for (var i = 0; i < size; ++i)
            {
                stringBuilder.Append((char)((random.Next() % (EndChar - StartChar + 1)) + StartChar));
            }

            var str = stringBuilder.ToString();
            var suffixesArrayCalculator = new SuffixesArrayCalculator(str);
            for (var i = 1; i < str.Length; ++i)
            {
                var first = str.Substring(suffixesArrayCalculator.SuffixesArray[i - 1]);
                var second = str.Substring(suffixesArrayCalculator.SuffixesArray[i]);
                Assert.That(first, Is.LessThanOrEqualTo(second));
            }
        }
    }
}