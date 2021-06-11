using System;
using System.Text.RegularExpressions;
using Blazor.Diagrams.Core.Models;
using DiagramDemo.Client.Models.Nodes;
using DiagramDemo.Client.Services;

namespace DiagramDemo.Client.Models
{
    public class FunctionBlockNodeConnector
    {
        private const string DescriptionColorDefault = "rgb(129, 129, 129)";
        private const string DescriptionColorNonDefault = "rgb(0, 0, 0)";
        private const string DescriptionColorSelected = "rgb(255, 255, 255)";
        private const string PoolingModeColorDefault = "rgb(129, 129, 129)";
        private const string PoolingModeColorNonDefault = "rgb(0, 0, 0)";
        private const string PoolingModeColorSelected = "rgb(0, 0, 0)";
        private const string PoolingModeDefault = "..|";
        private const string PortTextColorDefault = "rgb(0, 0, 0)";
        private const string PortTextColorOnDarkBackground = "rgb(255, 255, 255)";
        private const string TextBackgroundColorDefault = "transparent";
        private const string TextBackgroundColorNonDefault = "rgb(222, 222, 222)";
        private const string TextBackgroundColorSelected = "rgb(0, 120, 215)";

        public IFunctionBlockConnector Connector { get; set; }
        public string Description { get; set; }
        public string DescriptionColor { get; private set; } = DescriptionColorDefault;
        public bool HasDefaultValue { get; private set; } = true;
        public FunctionBlockNode Node { get; set; }
        public string PoolingMode { get; private set; } = PoolingModeDefault;
        public string PoolingModeColor { get; set; } = PoolingModeColorDefault;
        public PortModel Port { get; set; }
        public string PortColor { get; private set; }
        public string PortText { get; set; }
        public string PortTextColor { get; private set; } = PortTextColorDefault;
        public int RowIndex { get; set; }
        public bool Selected { get; private set; }
        public string TextBackgroundColor { get; private set; } = TextBackgroundColorDefault;

        public bool CanAttachTo(FunctionBlockNodeConnector nodeConnector) => DataflowValidator.CanAttachTo(Connector, nodeConnector.Connector);

        private void SetConnectorColors()
        {
            if (Selected)
            {
                DescriptionColor = DescriptionColorSelected;
                PoolingModeColor = PoolingModeColorSelected;
                TextBackgroundColor = TextBackgroundColorSelected;
            }
            else
            {
                DescriptionColor = DescriptionColorDefault;
                PoolingModeColor = PoolingModeColorDefault;
                TextBackgroundColor = TextBackgroundColorDefault;

                // Demoimplementation für die Random Werte
                if (PoolingMode != PoolingModeDefault)
                {
                    PoolingModeColor = PoolingModeColorNonDefault;
                    TextBackgroundColor = TextBackgroundColorNonDefault;
                }

                if (!HasDefaultValue)
                {
                    DescriptionColor = DescriptionColorNonDefault;
                    TextBackgroundColor = TextBackgroundColorNonDefault;
                }
            }
        }

        public void SetHasDefaultValue(bool hasDefaultValue)
        {
            if (hasDefaultValue == HasDefaultValue)
                return;

            HasDefaultValue = hasDefaultValue;
            SetConnectorColors();
        }

        public void SetPoolingMode(string poolingMode)
        {
            if (poolingMode == PoolingMode)
                return;

            PoolingMode = poolingMode;
            SetConnectorColors();
        }

        public void SetPortColor(string color)
        {
            PortColor = color;
            PortTextColor = Color.GetRelativeLuminance(color) < 0.3 ? PortTextColorOnDarkBackground : PortTextColorDefault;
        }

        public void SetSelection(bool isSelected)
        {
            if (isSelected == Selected)
                return;

            Selected = isSelected;
            SetConnectorColors();

            Node.ProcessConnectorSelectionChanged();
        }
    }
}
