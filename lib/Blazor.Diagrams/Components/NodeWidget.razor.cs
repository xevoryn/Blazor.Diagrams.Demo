using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Diagrams.Components
{
    public sealed partial class NodeWidget : ComponentBase
    {
        private PortCollection _portCollection;

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _portCollection = new PortCollection(JsRuntime);
        }
    }
}
