using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace LongestCommonPrefix
{
    internal class SuffixesArrayCalculator
    {
        [NotNull]
        private readonly int[] _suffixesArray;

        [NotNull]
        [ItemNotNull]
        private readonly List<List<int>> _orderedClasses;

        private int _classesCount = 1;

        //Complexity - O( |str| * ln(|str|) )
        public SuffixesArrayCalculator([NotNull] string str)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var bytes = str.Select(Convert.ToByte).Append((byte)0).ToArray();
            _suffixesArray = new int[bytes.Length];
            _orderedClasses = new List<List<int>>();

            DoInitialIteration(bytes);
            DoIterations(bytes);
        }

        [NotNull]
        public IReadOnlyList<int> SuffixesArray => _suffixesArray;

        [NotNull]
        public IReadOnlyList<int> GetOrderedClasses(int lnSize) => _orderedClasses[lnSize];

        private void DoInitialIteration([NotNull] byte[] bytes)
        {
            var cnt = new int[Math.Max(bytes.Length, 256)];
            foreach (var b in bytes)
            {
                ++cnt[b];
            }

            for (var i = 1; i < 256; ++i)
            {
                cnt[i] += cnt[i - 1];
            }

            for (var i = 0; i < bytes.Length; ++i)
            {
                _suffixesArray[--cnt[bytes[i]]] = i;
            }

            _orderedClasses.Add(new List<int>(Enumerable.Repeat(0, bytes.Length)));
            for (var i = 1; i < _suffixesArray.Length; ++i)
            {
                if (bytes[_suffixesArray[i]] != bytes[_suffixesArray[i - 1]])
                {
                    ++_classesCount;
                }

                _orderedClasses[0][_suffixesArray[i]] = _classesCount - 1;
            }
        }

        private void DoIterations([NotNull] byte[] bytes)
        {
            var newSuffixesArray = new int[bytes.Length];
            for (var h = 0; 1 << h < bytes.Length; ++h)
            {
                _orderedClasses.Add(new List<int>(Enumerable.Repeat(0, bytes.Length)));
                for (var i = 0; i < bytes.Length; ++i)
                {
                    newSuffixesArray[i] = _suffixesArray[i] - (1 << h);
                    if (newSuffixesArray[i] < 0)
                    {
                        newSuffixesArray[i] += bytes.Length;
                    }
                }

                var cnt = new int[_classesCount];
                for (var i = 0; i < bytes.Length; ++i)
                {
                    ++cnt[_orderedClasses[h][newSuffixesArray[i]]];
                }

                for (var i = 1; i < _classesCount; ++i)
                {
                    cnt[i] += cnt[i - 1];
                }

                for (var i = bytes.Length - 1; i >= 0; --i)
                {
                    _suffixesArray[--cnt[_orderedClasses[h][newSuffixesArray[i]]]] = newSuffixesArray[i];
                }

                _orderedClasses[h + 1][_suffixesArray[0]] = 0;
                _classesCount = 1;
                for (var i = 1; i < bytes.Length; ++i)
                {
                    var mid1 = (_suffixesArray[i] + (1 << h)) % bytes.Length;
                    var mid2 = (_suffixesArray[i - 1] + (1 << h)) % bytes.Length;
                    if (_orderedClasses[h][_suffixesArray[i]] != _orderedClasses[h][_suffixesArray[i - 1]] ||
                        _orderedClasses[h][mid1] != _orderedClasses[h][mid2])
                    {
                        ++_classesCount;
                    }

                    _orderedClasses[h + 1][_suffixesArray[i]] = _classesCount - 1;
                }
            }
        }
    }
}