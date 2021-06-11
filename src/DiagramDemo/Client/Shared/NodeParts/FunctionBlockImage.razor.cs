using DiagramDemo.Shared;
using Microsoft.AspNetCore.Components;

namespace DiagramDemo.Client.Shared.NodeParts
{
    public partial class FunctionBlockImage : ComponentBase
    {
        // Das könnte später ein In Memory Image sein (mittels 'data:image/png;base64,...' dargestellt) oder ein SVG string
        [Parameter]
        public string ImageSource { get; set; }

        [Parameter]
        public string ImageText { get; set; }

        [Parameter]
        public bool Visible { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            PerfLoggr.Log($"FunctionBlockImage.OnAfterRender({firstRender})");
        }
    }
}
