using System;
using System.Collections.Generic;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace DiagramDemo.Client.Models.Nodes
{
    public class FunctionBlockNode : NodeModel
    {
        public FunctionBlockNode(Point point = null) : base(point) { }

        public event Action ConnectorSelectionChanged;

        public string ClusterNodeId { get; set; }
        public List<FunctionBlockNodeConnector[]> Connectors { get; set; }
        public string CycleFrequency { get; set; }
        public string CycleOffset { get; set; }
        public string CyclePriority { get; set; }
        public string ImageSrc { get; set; }
        public string ImageText { get; set; }
        public string Name { get; set; }
        public string NameBackgroundColor { get; set; } = "transparent";
        public string NameForeColor { get; set; } = "black";
        public string RunMode { get; set; }

        public void ProcessConnectorSelectionChanged() => ConnectorSelectionChanged?.Invoke();
    }
}
