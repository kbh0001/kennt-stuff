using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NinjaTrader.Data;

namespace KenNinja
{
    public class SimpleDataSeries : IDataSeries
    {

        private Dictionary<int, double> _data = new Dictionary<int, double>();

        public SimpleDataSeries(IEnumerable<double> data)
        {
            var values = data.ToArray();
            _data = Enumerable.Range(0, values.Length).Select(z => new
            {
                Idx = values.Length - 1 - z,
                value = values[z]
            }).ToDictionary(z => z.Idx, k => k.value);



        }

        public bool IsValidPlot(int barIdx)
        {
                
            return _data.ContainsKey(barIdx);
        }

        public int Count
        {
            get { return _data.Count; }
        }

        public double this[int barsAgo]
        {
            get { return _data[barsAgo]; }
        }
    }
}