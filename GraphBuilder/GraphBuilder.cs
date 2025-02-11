using ScottPlot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Orbigraph
{
    class GraphBuilder
    {
        public GraphBuilder(string FilePath, string input, string output)
        {
            CsvConverter csvConverter = new CsvConverter(FilePath);
            var records = csvConverter.Records;

            double[] time = records.Select(r => (double)r.Time).ToArray();
            double[] temperature = records.Select(r => (double)r.Temperature).ToArray();
            double[] pressure = records.Select(r => (double)r.Pressure).ToArray();
            double[] altitude = records.Select(r => (double)r.Altitude).ToArray();

            var parser = new GraphDSLParser();

            input = File.ReadAllText(input);
            GraphData graphData = parser.Parse(input);

            double[] x = null;
            double[] y = null;

            string quantity1 = graphData.Quantity1.ToLower();
            string quantity2 = graphData.Quantity2.ToLower();

            switch (quantity1)
            {
                case "time":
                    x = time;
                    break;
                case "temperature":
                    x = temperature;
                    break;
                case "pressure":
                    x = pressure;
                    break;
                case "altitude":
                    x = altitude;
                    break;
            }

            switch (quantity2)
            {
                case "time":
                    y = time;
                    break;
                case "temperature":
                    y = temperature;
                    break;
                case "pressure":
                    y = pressure;
                    break;
                case "altitude":
                    y = altitude;
                    break;

            }

            Plot plt = new();

            foreach (var kvp in graphData.Properties)
            {
                string key = kvp.Key.ToLower();
                string value = kvp.Value.ToLower();

                switch (key)
                {
                    case "bgcolor":
                        plt.FigureBackground.Color = Color.FromHex(value);
                        plt.DataBackground.Color = Color.FromHex(value).Darken(0.1);
                        plt.Grid.MajorLineColor = Color.FromHex(value).Lighten(0.1);
                        break;

                    case "axes":
                        if (kvp.Value == "log")
                        {
                            double[] logYs = y.Select(Math.Log10).ToArray();
                            y = logYs;
                        }
                        break;
                    case "regression":
                        if (kvp.Value == "linear")
                        {
                            ScottPlot.Statistics.LinearRegression reg = new(x, y);
                            Coordinates pt1 = new(x.First(), reg.GetValue(x.First()));
                            Coordinates pt2 = new(x.Last(), reg.GetValue(x.Last()));
                            var line = plt.Add.Line(pt1, pt2);
                            plt.Title(reg.FormulaWithRSquared);
                        }
                        break;
                    case "title":
                        plt.Title(kvp.Key);
                        break;
                    case "hidegrid":
                        if (kvp.Value == "true")
                        {
                            plt.HideGrid();
                        }
                        break;
                    case "gridthickness":
                        plt.Grid.MajorLineWidth = Int32.Parse(kvp.Value);
                        break;



                }

            }

            plt.ScaleFactor = 2.5; // Make things bigger, at 1500x1000 resolution everything got very small

            plt.Add.Scatter(x, y);

            plt.Axes.Bottom.Label.Text = quantity1;
            plt.Axes.Left.Label.Text = quantity2;


            plt.SavePng(output, 1500, 1000);
            Console.WriteLine("Graph generated at " + output);
        }
    }
}