using DiagramDemo.Client.Models;
using Microsoft.AspNetCore.Components;

namespace DiagramDemo.Client.Components
{
    public partial class Grid : ComponentBase
    {
        [Parameter] public double Dimensions { get; set; }
        [Parameter] public GridMode Mode { get; set; }
        [Parameter] public double PosX { get; set; }
        [Parameter] public double PosY { get; set; }
        [Parameter] public bool Visible { get; set; }
    }
}
