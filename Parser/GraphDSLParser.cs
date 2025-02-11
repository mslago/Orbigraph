using System;
using System.Xml.Linq;
using Irony.Parsing;

namespace Orbigraph
{
    public class GraphDSLParser
    {
        private readonly Parser parser;
        public GraphDSLParser()
        {
            var grammar = new GraphDSLGrammar();
            parser = new Parser(grammar);
        }

        public GraphData Parse(string input)
        {
            var parseTree = parser.Parse(input);

            if (parseTree.HasErrors())
            {
                Console.WriteLine("Error: " + parseTree.ParserMessages[0].Message);
                return null;
            }
            else
            {
                Console.WriteLine("Parsing successful, generating graph");
                PrintParseTree(parseTree.Root, 0);
                var extractor = new GraphDSLExtractor();
                GraphData data = extractor.ExtractTreeData(parseTree.Root);
                return data;
            }
        }
        private void PrintParseTree(ParseTreeNode node, int indentLevel) //Currently unused, only for debug purposes
        {
            if (node == null)
                return;

            Console.Write(new string(' ', indentLevel * 2));

            Console.WriteLine(node.ToString());

            foreach (var child in node.ChildNodes)
            {
                PrintParseTree(child, indentLevel + 1);
            }
        }


    }
}
