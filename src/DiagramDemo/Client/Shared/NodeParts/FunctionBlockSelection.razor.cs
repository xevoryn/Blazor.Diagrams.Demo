using DiagramDemo.Shared;
using Microsoft.AspNetCore.Components;

namespace DiagramDemo.Client.Shared.NodeParts
{
    public partial class FunctionBlockSelection : ComponentBase
    {
        [Parameter]
        public bool IsSelected { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            PerfLoggr.Log($"FunctionBlockSelection({IsSelected}).OnAfterRender({firstRender})");
        }
    }
}
