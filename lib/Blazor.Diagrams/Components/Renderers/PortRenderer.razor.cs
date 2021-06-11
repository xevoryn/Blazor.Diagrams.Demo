using System;
using System.Linq;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Components.Renderers
{
    public sealed partial class PortRenderer : ComponentBase, IDisposable
    {
        private bool _shouldRender = true;
        private ElementReference _element;

        [CascadingParameter]
        public Diagram Diagram { get; set; }

        [CascadingParameter]
        public PortCollection PortCollection { get; set; }

        [Parameter]
        public PortModel Port { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public void Dispose()
        {
            Port.Changed -= PortCollection.UpdatePorts;
            PortCollection.OnUpdate -= OnPortChanged;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Port.Changed += PortCollection.UpdatePorts;
            PortCollection.OnUpdate += OnPortChanged;
        }

        protected override bool ShouldRender()
            => _shouldRender;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            _shouldRender = false;

            if (firstRender)
                PortCollection.PortRenderers.Add(this, _element);

            //if (!Port.Initialized)
            //    PortCollection.ReinitializeAll();
        }

        private void OnMouseDown(MouseEventArgs e) => Diagram.OnMouseDown(Port, e);

        private void OnMouseUp(MouseEventArgs e) => Diagram.OnMouseUp(Port, e);

        private void OnTouchStart(TouchEventArgs e) => Diagram.OnTouchStart(Port, e);

        private void OnTouchEnd(TouchEventArgs e)
            => Diagram.OnTouchEnd(FindPortOn(e.ChangedTouches[0].ClientX, e.ChangedTouches[0].ClientY), e);

        private PortModel FindPortOn(double clientX, double clientY)
        {
            var allPorts = Diagram.Nodes.SelectMany(n => n.Ports)
                .Union(Diagram.Groups.SelectMany(g => g.Ports));

            foreach (var port in allPorts)
            {
                if (!port.Initialized)
                    continue;

                var relativePt = Diagram.GetRelativeMousePoint(clientX, clientY);
                if (port.GetBounds().ContainsPoint(relativePt))
                    return port;
            }

            return null;
        }

        internal void UpdateDimensions(Rectangle portRectangle)
        {
            var zoom = Diagram.Zoom;
            var pan = Diagram.Pan;

            Port.Size = new Size(portRectangle.Width / zoom, portRectangle.Height / zoom);
            Port.Position = new Point((portRectangle.Left - Diagram.Container.Left - pan.X) / zoom,
                (portRectangle.Top - Diagram.Container.Top - pan.Y) / zoom);

            Port.Initialized = true;

            // We don't really need to refresh the port again,
            // let's just refresh the links so that they use the new port's position
            Port.RefreshLinks();
        }

        private void OnPortChanged()
        {
            if (Port.Initialized)
            {
                _shouldRender = true;
                StateHasChanged();
            }
            else
            {
                PortCollection.ReinitializeAll();
            }
        }
    }
}
