using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkStats
{
    public class Pmf
    {
        private readonly Dictionary<decimal, decimal> _data;
        private readonly int _precision;

        private Pmf(Hist<decimal> hist, int precision)
        {
            _precision = precision;
            _data = new Dictionary<decimal, decimal>(hist.Comparer);
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
            return Normalize(_data.Sum(x => x.Value), _precision);
        }

        public IEnumerable<decimal> Values()
        {
            return _data.Select(x => x.Key);
        }

        public IEnumerable<decimal> Frequencies()
        {
            return _data.Select(x => Normalize(x.Value, _precision));
        }

        public decimal Prob(decimal value)
        {
            return Normalize(GetFrequencyOrZero(value), _precision);
        }

        private decimal GetFrequencyOrZero(decimal value)
        {
            decimal freq;
            if (!_data.TryGetValue(value, out freq))
            {
                freq = 0;
            }

            return Normalize(freq, _precision);
        }

        public void Incr(decimal value, decimal delta)
        {
            decimal current = GetFrequencyOrZero(value);
            _data[value] = current + delta;
        }

        public void Mult(decimal value, decimal multiple)
        {
            decimal current = GetFrequencyOrZero(value);
            _data[value] = current * multiple;
        }

        public decimal Mean()
        {
            return Normalize(_data.Sum(data => (data.Key*data.Value)), _precision);
        }

        public static Pmf MakePmfFromList(IEnumerable<decimal> values, IEqualityComparer<decimal> comparer = null, int precision = c_defaultPrecision)
        {
            return MakePmfFromHist(new Hist<decimal>(values, comparer ?? EqualityComparer<decimal>.Default), precision);
        }

        public static Pmf MakePmfFromHist(Hist<decimal> hist, int precision = c_defaultPrecision)
        {
            return new Pmf(hist, precision);
        }

        private static decimal Normalize(decimal value, int precision)
        {
            decimal multiple = (decimal)Math.Pow(10, precision);
            return Math.Round(value * multiple) / multiple;
        }

        private const int c_defaultPrecision = 20;
    }
}
