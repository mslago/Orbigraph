using System;
using CommandLine;
using System.Xml;

namespace Orbigraph
{
    class Program
    {
        class Options
        {
            [Value(0, MetaName = "Code File", Required = true, HelpText = "Input Orbigraph code file name.")]
            public string InputFilePath { get; set; }

            [Value(1, MetaName = "Data File", Required = true, HelpText = "Input data file name.")]
            public string DataFile { get; set; }

            [Option('o', "output", Required = false, Default = "output.png", HelpText = "File output code for png image file.")]
            public string Output { get; set; }
        }
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options =>
            {
                string inputfile = options.InputFilePath;
                string datafile = options.DataFile;
                string output = options.Output;

                GraphBuilder graphBuilder = new(datafile, inputfile, output);

            });

        }
    }
}
