using System;
using System.Linq;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using DiagramDemo.Client.Models;
using DiagramDemo.Client.Models.Nodes;
using DiagramDemo.Client.Models.Ports;
using DiagramDemo.Client.Services;
using DiagramDemo.Client.Shared.Nodes;
using DiagramDemo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DiagramDemo.Client.Pages
{
    public partial class Demo : ComponentBase, IDisposable
    {
        private const double DefaultZoom = 1;
        private const int DefaultGridSize = 10;

        private int _blockSpawnAmount = 50;
        private Diagram _diagram;
        private GridMode _gridMode;
        private double _gridDimensions = DefaultGridSize * DefaultZoom;
        private Point _gridPosition = Point.Zero;
        private bool _gridVisible = true;
        private bool _hasMouseDownShiftKey;
        private int _spawnPointX = 20 - 200;
        private int _spawnPointY = 30;

        private void DeleteSelectedNodes()
        {
            var nodes = _diagram.GetSelectedModels().OfType<NodeModel>().ToList();
            _diagram.Nodes.Remove(nodes);
        }

        public void Dispose()
        {
            _diagram.Links.Added -= OnDiagramLinksAdded;
            _diagram.MouseClick -= OnDiagramMouseClick;
            _diagram.SelectionChanged -= OnDiagramSelectionChanged;
            GC.SuppressFinalize(this);
        }

        private Point GetSpawnPoint()
        {
            if ((_spawnPointX += 200) > 2000)
            {
                _spawnPointX = 20 + _spawnPointY;
                if ((_spawnPointY += 80) > 800)
                    _spawnPointY = 30;
            }
            return new Point(_spawnPointX, _spawnPointY);
        }

        private void InitializeDiagram()
        {
            _diagram = new(new()
            {
                AllowMultiSelection = true,
                EnableVirtualization = false,
                GridSize = DefaultGridSize,
                Links = new()
                {
                    DefaultPathGenerator = PathGenerators.Smooth,
                    DefaultSelectedColor = "rgb(0, 120, 215)"
                },
                Zoom = new()
                {
                    Inverse = true,
                    Maximum = 8
                }
            });

            _diagram.RegisterModelComponent<FunctionBlockNode, FunctionBlockComponent>();

            _diagram.Links.Added += OnDiagramLinksAdded;
            _diagram.MouseClick += OnDiagramMouseClick;
            _diagram.PanChanged += OnDiagramPanChanged;
            _diagram.SelectionChanged += OnDiagramSelectionChanged;
            _diagram.ZoomChanged += OnDiagramZoomChanged;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            PerfLoggr.Log($"Demo.OnAfterRender({firstRender})");
        }

        private void OnContainerMouseDown(MouseEventArgs args)
        {
            if (args.Button != 0)
                return;

            _hasMouseDownShiftKey = args.ShiftKey;
        }

        private void OnDiagramLinksAdded(BaseLinkModel link)
        {
            link.SourceMarker = LinkMarker.NewSquare(6);
            link.TargetMarker = LinkMarker.Arrow;
        }

        private void OnDiagramMouseClick(Model model, MouseEventArgs e)
        {
            if (model is FunctionBlockPort)
                _diagram.UnselectAll();
            else if (model == null)
                UIState.DeselectConnectors();
        }

        private void OnDiagramPanChanged()
        {
            _gridPosition = _diagram.Pan;
            InvokeAsync(StateHasChanged);
        }

        private void OnDiagramSelectionChanged(SelectableModel model)
        {
            if (model != null)
                UIState.DeselectConnectors();
        }

        private void OnDiagramZoomChanged()
        {
            _gridPosition = _diagram.Pan;
            _gridDimensions = DefaultGridSize * _diagram.Zoom;
            _gridVisible = _diagram.Zoom > 0.5;

            InvokeAsync(StateHasChanged);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitializeDiagram();
        }

        private void PerfTestAddSonos()
        {
            PerfLoggr.Start();
            PerfLoggr.Log("");
            PerfLoggr.Log($"### Demo.PerfTestAddSonos({_blockSpawnAmount})");
            
            for (var i = 0; i < _blockSpawnAmount; i++)
            {
                var fb = FunctionBlockGenerator.CreateSonos(GetSpawnPoint());
                PerfLoggr.Log($"  FunctionBlockGenerator.CreateSonos(GetSpawnPoint())");

                var node = FunctionBlockNodeModelGenerator.Run(fb);
                PerfLoggr.Log($"  FunctionBlockNodeModelGenerator.Run(fb_{i})");
                
                _diagram.Nodes.Add(node);
                PerfLoggr.Log($"  _diagram.Nodes.Add(node_{i})");
            }
        }

        private void PerfTestAddSwitch()
        {
            PerfLoggr.Start();
            PerfLoggr.Log("");
            PerfLoggr.Log($"### Demo.PerfTestAddSwitch({_blockSpawnAmount})");

            for (var i = 0; i < _blockSpawnAmount; i++)
            {
                var fb = FunctionBlockGenerator.CreateSwitch(GetSpawnPoint());
                PerfLoggr.Log($"  FunctionBlockGenerator.CreateSwitch(GetSpawnPoint())");

                var node = FunctionBlockNodeModelGenerator.Run(fb);
                PerfLoggr.Log($"  FunctionBlockNodeModelGenerator.Run(fb_{i})");

                _diagram.Nodes.Add(node);
                PerfLoggr.Log($"  _diagram.Nodes.Add(node_{i})");
            }
        }

        private void PerfTestAddSwitchAndSonos()
        {
            PerfLoggr.Start();
            PerfLoggr.Log("");
            PerfLoggr.Log($"### Demo.PerfTestAddSwitchAndSonos({_blockSpawnAmount * 2})");

            for (var i = 0; i < _blockSpawnAmount; i++)
            {
                // create switch blocks

                var fb = FunctionBlockGenerator.CreateSwitch(GetSpawnPoint());
                PerfLoggr.Log($"  FunctionBlockGenerator.CreateSwitch(GetSpawnPoint())");

                var node = FunctionBlockNodeModelGenerator.Run(fb);
                PerfLoggr.Log($"  FunctionBlockNodeModelGenerator.Run(fb_{i})");

                _diagram.Nodes.Add(node);
                PerfLoggr.Log($"  _diagram.Nodes.Add(node_{i})");

                // create sonos blocks

                fb = FunctionBlockGenerator.CreateSonos(GetSpawnPoint());
                PerfLoggr.Log($"  FunctionBlockGenerator.CreateSonos(GetSpawnPoint())");

                node = FunctionBlockNodeModelGenerator.Run(fb);
                PerfLoggr.Log($"  FunctionBlockNodeModelGenerator.Run(fb_{i})");

                _diagram.Nodes.Add(node);
                PerfLoggr.Log($"  _diagram.Nodes.Add(node_{i})");
            }
        }

        private void ToggleGridMode()
        {
            if (_gridMode == GridMode.Line)
                _gridMode = GridMode.Point;
            else
                _gridMode = GridMode.Line;
        }

        private static void WriteLog()
        {
            PerfLoggr.Write();
        }
    }
}
