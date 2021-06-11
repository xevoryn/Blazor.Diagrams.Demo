using DiagramDemo.Shared;
using Microsoft.AspNetCore.Components;

namespace DiagramDemo.Client.Shared.NodeParts
{
    public partial class FunctionBlockTitleComponent : ComponentBase
    {
        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public string NameBackgroundColor { get; set; }

        [Parameter]
        public string NameForeColor { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            PerfLoggr.Log($"FunctionBlockSelection({Name}).OnAfterRender({firstRender})");
        }
    }
}
