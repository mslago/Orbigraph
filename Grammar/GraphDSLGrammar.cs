using System;
using Irony.Parsing;

namespace Orbigraph
{
    public class GraphDSLGrammar : Grammar
    {
        public GraphDSLGrammar()
        {
            // Terminals
            var PlotType = ToTerm("plot", "PLOT");
            var vs = ToTerm("vs", "VS");
            var leftbracket = ToTerm("[");
            var rightbracket = ToTerm("]");
            var leftcurlybracket = ToTerm("{");
            var rightcurlybracket = ToTerm("}");
            var leftparenthesis = ToTerm("(");
            var rightparenthesis = ToTerm(")");
            var semicolon = ToTerm(";");
            var assign = ToTerm(":");
            var Number = new NumberLiteral("NUMERIC_VALUE", NumberOptions.AllowSign);
            var ID = new IdentifierTerminal("ID");
            var trueValue = ToTerm("true");
            var falseValue = ToTerm("false");
            var hexColor = new RegexBasedTerminal("HEX_COLOR", "#[0-9A-Fa-f]{6}");
            // Non-terminals
            var start = new NonTerminal("start");
            var program = new NonTerminal("program");
            var graph = new NonTerminal("graph");
            var graph_type = new NonTerminal("graph_type");
            var graph_spec = new NonTerminal("graph_spec");
            var quantity = new NonTerminal("quantity");
            var graph_body = new NonTerminal("graph_body");
            var graph_property = new NonTerminal("graph_property");
            var key_value = new NonTerminal("key_value");
            var value = new NonTerminal("value");
            var hexvalue = new NonTerminal("hexvalue");
            var graphTail = new NonTerminal("graphTail");

            // Rules
            start.Rule = program;
            program.Rule = MakePlusRule(program, graph);
            graph.Rule =
                graph_spec + graphTail;
            graphTail.Rule = semicolon | leftcurlybracket + graph_body + rightcurlybracket;
            MarkTransient(graphTail);
            graph_spec.Rule = graph_type + quantity + vs + quantity;
            graph_type.Rule = ID;
            quantity.Rule =
                leftbracket + ID + rightbracket
                | leftbracket + ID + "%" + rightbracket
                | leftbracket + ID + leftparenthesis + ID + rightparenthesis + rightbracket;
            graph_body.Rule = MakeStarRule(graph_body, graph_property);
            graph_property.Rule = key_value + semicolon;
            key_value.Rule = ID + assign + value;
            value.Rule = ID | trueValue | falseValue | Number | hexColor;

            // Set grammar root
            this.Root = start;

            // Mark punctuation
            MarkPunctuation(
                "plot",
                "[",
                "]",
                "(",
                ")",
                "{",
                "}",
                ";",
                "=",
                ":",
                "%",
                "vs",
                "by",
                "\n",
                "\t",
                " "
            );
        }

    }
}
