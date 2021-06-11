using System.Linq;
using Blazor.Diagrams.Core.Models;

namespace DiagramDemo.Client.Models.Ports
{
    public class FunctionBlockPort : PortModel
    {
        private readonly FunctionBlockNodeConnector _nodeConnector;

        public FunctionBlockPort(NodeModel parent, PortAlignment portAlignment, FunctionBlockNodeConnector nodeConnector) : base(parent, portAlignment) => _nodeConnector = nodeConnector;

        public override bool CanAttachTo(PortModel port)
        {
            if (!base.CanAttachTo(port))
                return false;

            if (port is not FunctionBlockPort fbPort)
                return false;

            // Nur eine Verbindung zwischen zwei Ports erlauben, unabhängig von der Richtung
            if (Links.Any(l => l.TargetPort?.Id == port.Id || l.SourcePort?.Id == port.Id))
                return false;

            return _nodeConnector.CanAttachTo(fbPort._nodeConnector);
        }
    }
}
