using System;
using System.Text;
using NUnit.Framework;

namespace LongestCommonPrefix.Tests
{
    [TestFixture]
    public class LongestCommonPrefixCalculatorTests
    {
        private const int StartChar = 'a';
        private const int EndChar = 'z';
        
        [TestCase(0)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void GetCommonPrefix_ExactlyLongest(int size)
        {
            var random = new Random(123);
            var stringBuilder = new StringBuilder(size);
            for (var i = 0; i < size; ++i)
            {
                stringBuilder.Append((char)((random.Next() % (EndChar - StartChar + 1)) + StartChar));
            }

            var str = stringBuilder.ToString();
            var longestCommonPrefixCalculator = new LongestCommonPrefixCalculator(str);

            for (var i = 0; i < str.Length; ++i)
            {
                for (var j = 0; j < str.Length; ++j)
                {
                    var maxSize = longestCommonPrefixCalculator.Get(i, j);
                    Assert.That(str.Substring(i, maxSize), Is.EqualTo(str.Substring(j, maxSize)));
                }

            }
        }
    }
}