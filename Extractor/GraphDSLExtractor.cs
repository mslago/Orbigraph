using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Orbigraph
{
    public class GraphData
    {
        public string Quantity1 { get; set; }
        public string Quantity2 { get; set; }
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }

    public class GraphDSLExtractor
    {
        public GraphData ExtractTreeData(ParseTreeNode node)
        {
            GraphData graphData = new GraphData();

            if (node.Term.Name == "graph_spec")
            {
                if (node.ChildNodes.Count >= 3)
                {
                    graphData.Quantity1 = node.ChildNodes[1].FindTokenAndGetText();
                    graphData.Quantity2 = node.ChildNodes[2].FindTokenAndGetText();
                }
                else
                {
                    Console.WriteLine("Error: Graph doesn't have two variables.");
                }
            }
            else if (node.Term.Name == "graph_body")
            {
                foreach (var propNode in node.ChildNodes)
                {
                    // We expect each property to follow the structure:
                    // graph_property -> key_value -> [ key, value ]
                    if (propNode.Term.Name == "graph_property" && propNode.ChildNodes.Count > 0)
                    {
                        var keyValueNode = propNode.ChildNodes[0];
                        if (keyValueNode.Term.Name == "key_value" && keyValueNode.ChildNodes.Count >= 2)
                        {
                            string key = keyValueNode.ChildNodes[0].FindTokenAndGetText();

                            string value = string.Empty;
                            if (keyValueNode.ChildNodes[1].ChildNodes.Count >= 1)
                            {
                                value = keyValueNode.ChildNodes[1].ChildNodes[0].FindTokenAndGetText();
                            }
                            else
                            {
                                Console.WriteLine("Error: Value node has no children.");
                            }
                            graphData.Properties[key] = value;
                        }
                    }
                }
            }
            else
            {
                foreach (var child in node.ChildNodes)
                {
                    GraphData childData = ExtractTreeData(child);
                    if (childData != null)
                    {
                        if (string.IsNullOrEmpty(graphData.Quantity1) && !string.IsNullOrEmpty(childData.Quantity1))
                            graphData.Quantity1 = childData.Quantity1;
                        if (string.IsNullOrEmpty(graphData.Quantity2) && !string.IsNullOrEmpty(childData.Quantity2))
                            graphData.Quantity2 = childData.Quantity2;
                        foreach (var kvp in childData.Properties)
                        {
                            if (!graphData.Properties.ContainsKey(kvp.Key))
                                graphData.Properties[kvp.Key] = kvp.Value;
                        }
                    }
                    ExtractTreeData(child);
                }
            }
            return graphData;
        }
    }
}
