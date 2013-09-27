using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkStats
{
    public class Pmf<T>
        where T: IComparable<T>
    {
        private readonly Dictionary<T, decimal> _data;
        private readonly int _precision;

        private Pmf(Hist<T> hist, int precision)
        {
            _precision = precision;
            _data = new Dictionary<T, decimal>(hist.Comparer);
            foreach (var item in hist.Items)
            {
                _data[item.Key] = item.Value;
            }

            Normalize();
        }

        public void Normalize(decimal? denominator = null)
        {
            decimal denom = denominator ?? Total();
            foreach (var item in _data.ToList())
            {
                _data[item.Key] = _data[item.Key] / denom;
            }
        }

        public decimal Total()
        {
            return _data.Sum(x => x.Value);
        }

        public IEnumerable<T> Values()
        {
            return _data.Select(x => x.Key);
        }

        public IEnumerable<decimal> Frequencies()
        {
            return _data.Select(x => x.Value);
        }

        public decimal Prob(T value)
        {
            return GetFrequencyOrZero(value);
        }

        private decimal GetFrequencyOrZero(T value)
        {
            decimal freq;
            if (!_data.TryGetValue(value, out freq))
            {
                freq = 0;
            }

            return freq;
        }

        public void Incr(T value, decimal delta)
        {
            decimal current = GetFrequencyOrZero(value);
            _data[value] = current + delta;
        }

        public void Mult(T value, decimal multiple)
        {
            decimal current = GetFrequencyOrZero(value);
            _data[value] = current * multiple;
        }

        public static Pmf<T> MakePmfFromList(IEnumerable<T> values, IEqualityComparer<T> comparer = null, int precision = 300)
        {
            return MakePmfFromHist(new Hist<T>(values, comparer ?? EqualityComparer<T>.Default), precision);
        }

        public static Pmf<T> MakePmfFromHist(Hist<T> hist, int precision = 300)
        {
            return new Pmf<T>(hist, precision);
        }
    }
}
