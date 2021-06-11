using System.Collections.Generic;
using Blazor.Diagrams.Core.Geometry;

namespace DiagramDemo.Client.Models
{
    public class FunctionBlock
    {
        public string ClusterNodeId { get; set; }
        public List<IFunctionBlockConnector> Connectors { get; set; }
        public string ImageName { get; set; }
        public string ImageText { get; set; }
        public string Name { get; set; }
        public Point Position { get; set; }
        public FunctionBlockRunMode RunMode { get; set; }
    }
}
