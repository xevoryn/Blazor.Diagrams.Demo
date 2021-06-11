using System.Collections.Generic;
using DiagramDemo.Client.Models;

namespace DiagramDemo.Client.Services
{
    public static class UIState
    {
        public static List<FunctionBlockNodeConnector> SelectedConnectors { get; } = new();

        public static void DeselectConnectors() => SetSelectedConnector(null);

        public static void SetSelectedConnector(FunctionBlockNodeConnector nodeConnector)
        {
            foreach (var conn in SelectedConnectors)
                conn.SetSelection(false);
            SelectedConnectors.Clear();

            if (nodeConnector != null)
            {
                nodeConnector.SetSelection(true);
                SelectedConnectors.Add(nodeConnector);
            }
        }

        public static void ToggleSelectedConnector(FunctionBlockNodeConnector nodeConnector)
        {
            if (nodeConnector.Selected)
            {
                nodeConnector.SetSelection(false);
                SelectedConnectors.Remove(nodeConnector);
            }
            else
            {
                nodeConnector.SetSelection(true);
                SelectedConnectors.Add(nodeConnector);
            }
        }
    }
}
