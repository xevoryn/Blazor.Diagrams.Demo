using DiagramDemo.Client.Models;
using DiagramDemo.Client.Services;
using DiagramDemo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DiagramDemo.Client.Shared.NodeParts
{
    public partial class FunctionBlockOutputConnectorComponent : ComponentBase
    {
        [Parameter]
        public FunctionBlockNodeConnector NodeConnector { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            PerfLoggr.Log($"FunctionBlockOutputConnectorComponent({NodeConnector.Description}).OnAfterRender({firstRender})");
        }

        private void OnPortClick(MouseEventArgs e)
        {
            if (e.CtrlKey)
                UIState.ToggleSelectedConnector(NodeConnector);
            else
                UIState.SetSelectedConnector(NodeConnector);
        }
    }
}
