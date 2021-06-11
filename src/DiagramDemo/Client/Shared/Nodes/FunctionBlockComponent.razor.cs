using System;
using System.Linq;
using DiagramDemo.Client.Models.Nodes;
using DiagramDemo.Client.Services;
using DiagramDemo.Shared;
using Microsoft.AspNetCore.Components;

namespace DiagramDemo.Client.Shared.Nodes
{
    public partial class FunctionBlockComponent : ComponentBase, IDisposable
    {
        [Parameter]
        public FunctionBlockNode Node { get; set; }

        public void Dispose()
        {
            Node.ConnectorSelectionChanged -= OnNodeConnectorSelectionChanged;
            GC.SuppressFinalize(this);
        }

        private bool IsImageVisible() => !UIState.SelectedConnectors.Any(c => c.Node.Id == Node.Id && c.Connector.RowIndex <= 3);

        protected override void OnAfterRender(bool firstRender)
        {
            PerfLoggr.Log($"FunctionBlockComponent({Node.Id}).OnAfterRender({firstRender})");
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Node.ConnectorSelectionChanged += OnNodeConnectorSelectionChanged;
        }

        private void OnNodeConnectorSelectionChanged() => StateHasChanged();

    }
}
