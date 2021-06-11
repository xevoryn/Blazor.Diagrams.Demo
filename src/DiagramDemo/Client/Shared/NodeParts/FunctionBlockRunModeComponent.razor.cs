using DiagramDemo.Shared;
using Microsoft.AspNetCore.Components;

namespace DiagramDemo.Client.Shared.NodeParts
{
    public partial class FunctionBlockRunModeComponent : ComponentBase
    {
        [Parameter]
        public string CycleFrequency { get; set; }

        [Parameter]
        public string CycleOffset { get; set; }

        [Parameter]
        public string CyclePriority { get; set; }

        [Parameter]
        public string NodeId { get; set; }

        [Parameter]
        public string RunMode { get; set; }

        private bool IsRunModeCycle() => RunMode == "Y";

        protected override void OnAfterRender(bool firstRender)
        {
            PerfLoggr.Log($"FunctionBlockRunModeComponent({NodeId}).OnAfterRender({firstRender})");
        }
    }
}
