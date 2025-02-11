using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Orbigraph
{
    public class CsvConverter
    {
        public List<Foo> Records { get; private set; }
        public CsvConverter(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            Records = new List<Foo>(csv.GetRecords<Foo>());
        }
        public class Foo
        {
            public double Time { get; set; }
            public double Pressure { get; set; }
            public decimal Temperature { get; set; }
            public double Altitude { get; set; }
        }
    }
}