using System;
using System.Linq;
using Blazor.Diagrams.Services;
using DiagramDemo.Client.Models;
using DiagramDemo.Client.Models.Nodes;
using DiagramDemo.Client.Services;
using DiagramDemo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace DiagramDemo.Client.Shared.Nodes
{
    public partial class FunctionBlockComponent : ComponentBase, IDisposable
    {
        private PortCollection _portCollection;

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public FunctionBlockNode Node { get; set; }

        public void Dispose()
        {
            Node.ConnectorSelectionChanged -= OnNodeConnectorSelectionChanged;
            GC.SuppressFinalize(this);
        }

        private bool IsImageVisible() => !UIState.SelectedConnectors.Any(c => c.Node.Id == Node.Id && c.Connector.RowIndex <= 3);

        private bool IsRunModeCycle() => Node.RunMode == "Y";

        protected override void OnAfterRender(bool firstRender)
        {
            PerfLoggr.Log($"FunctionBlockComponent({Node.Id}).OnAfterRender({firstRender})");
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _portCollection = new(JsRuntime);
            Node.ConnectorSelectionChanged += OnNodeConnectorSelectionChanged;
        }

        private void OnNodeConnectorSelectionChanged() => StateHasChanged();

        private static void OnPortClick(MouseEventArgs e, FunctionBlockNodeConnector connector)
        {

        }
    }
}
