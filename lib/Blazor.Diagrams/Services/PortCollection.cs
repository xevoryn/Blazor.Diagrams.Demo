using System;
using System.Collections.Generic;
using System.Linq;
using Blazor.Diagrams.Components.Renderers;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Diagrams.Services
{
    public sealed class PortCollection
    {
        private readonly IJSRuntime _jsRuntime;
        private bool _updating;

        public event Action OnUpdate;

        public PortCollection(IJSRuntime jsRuntime)
            => _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));

        public Dictionary<PortRenderer, ElementReference> PortRenderers { get; } = new Dictionary<PortRenderer, ElementReference>();

        public void ReinitializeAll()
        {
            if (_updating)
                return;

            _updating = true;
            var portRectangles = _jsRuntime.GetAllBoundingClientRect(PortRenderers.Select(pr => pr.Value)).ToList();

            if (portRectangles.Count() != PortRenderers.Count)
                throw new InvalidOperationException("Unequal count of PortRenderers to bounding rectangles.");

            var count = 0;
            foreach (var (port, reference) in PortRenderers)
            {
                port.Port.Initialized = false;
                port.UpdateDimensions(portRectangles[count++]);
            }
        }

        public void UpdatePorts()
        {
            if (!_updating)
                OnUpdate?.Invoke();
        }
    }
}
