using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Extensions;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Blazor.Diagrams.Components.Renderers
{
    public sealed partial class NodeRenderer : ComponentBase, IDisposable
    {
        private bool _becameVisible;
        private Type _componentType;
        private bool _shouldRender;
        private bool _isVisible = true;
        private ElementReference _element;
        private DotNetObjectReference<NodeRenderer> _reference;

        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [Parameter]
        public NodeModel Node { get; set; }

        [Inject]
        private IJSRuntime JsRuntime { get; set; }

        public void Dispose()
        {
            Diagram.PanChanged -= CheckVisibility;
            Diagram.ZoomChanged -= CheckVisibility;
            Diagram.ContainerChanged -= CheckVisibility;
            Node.Changed -= ReRender;

            if (_element.Id != null)
                _ = JsRuntime.UnobserveResizes(_element);

            _reference?.Dispose();
        }

        private Type GetComponentType()
        {
            if (Node.GetType() != _componentType)
            {
                _componentType = Diagram.GetComponentForModel(Node) ??
                    Diagram.Options.DefaultNodeComponent ??
                    (Node.Layer == RenderLayer.HTML ? typeof(NodeWidget) : typeof(SvgNodeWidget));
            }

            return _componentType;
        }

        [JSInvokable]
        public void OnResize(Size size)
        {
            // When the node becomes invisible (a.k.a unrendered), the size is zero
            if (Size.Zero.Equals(size))
                return;

            size = new Size(size.Width / Diagram.Zoom, size.Height / Diagram.Zoom);
            if (Node.Size != null && Node.Size.Width.AlmostEqualTo(size.Width) && Node.Size.Height.AlmostEqualTo(size.Height))
                return;

            Node.Size = size;
            Node.Refresh();
            Node.RefreshLinks();
            Node.ReinitializePorts();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _reference = DotNetObjectReference.Create(this);
            Diagram.PanChanged += CheckVisibility;
            Diagram.ZoomChanged += CheckVisibility;
            Diagram.ContainerChanged += CheckVisibility;
            Node.Changed += ReRender;
        }

        protected override bool ShouldRender()
        {
            if (_shouldRender)
            {
                _shouldRender = false;
                return true;
            }

            return false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender || _becameVisible)
            {
                _becameVisible = false;
                await JsRuntime.ObserveResizes(_element, _reference);
            }
        }

        private async void CheckVisibility()
        {
            // _isVisible must be true in case virtualization gets disabled and some nodes are hidden
            if (!Diagram.Options.EnableVirtualization && _isVisible)
                return;

            if (Node.Size == null)
                return;

            var left = Node.Position.X * Diagram.Zoom + Diagram.Pan.X;
            var top = Node.Position.Y * Diagram.Zoom + Diagram.Pan.Y;
            var right = left + Node.Size.Width * Diagram.Zoom;
            var bottom = top + Node.Size.Height * Diagram.Zoom;

            var isVisible = right > 0 && left < Diagram.Container.Width && bottom > 0 &&
                top < Diagram.Container.Height;

            if (_isVisible != isVisible)
            {
                _isVisible = isVisible;
                _becameVisible = isVisible;

                if (!_isVisible)
                {
                    await JsRuntime.UnobserveResizes(_element);
                }

                ReRender();
            }
        }

        private void ReRender()
        {
            _shouldRender = true;
            StateHasChanged();
        }

        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Node, e);

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Node, e);

        private void OnTouchStart(TouchEventArgs e) => Diagram.OnTouchStart(Node, e);

        private void OnTouchEnd(TouchEventArgs e) => Diagram.OnTouchEnd(Node, e);
    }
}
