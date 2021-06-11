using System;
using System.Collections.Generic;
using System.Linq;
using Blazor.Diagrams.Core.Models;
using DiagramDemo.Client.Models;
using DiagramDemo.Client.Models.Nodes;
using DiagramDemo.Client.Models.Ports;

namespace DiagramDemo.Client.Services
{
    public static class FunctionBlockNodeModelGenerator
    {
        private static List<FunctionBlockNodeConnector[]> GenerateConnectors(FunctionBlock functionBlock, FunctionBlockNode node)
        {
            var result = new List<FunctionBlockNodeConnector[]>();

            var rowCount = 0;
            var connectorCount = 0;
            while (connectorCount < functionBlock.Connectors.Count || rowCount < 8)
            {
                var row = new FunctionBlockNodeConnector[2];
                var inputConnector = functionBlock.Connectors.FirstOrDefault(c => c.RowIndex == rowCount && c.IsInput);
                var outputConnector = functionBlock.Connectors.FirstOrDefault(c => c.RowIndex == rowCount && !c.IsInput);
                row[0] = GetNodeConnector(node, inputConnector);
                row[1] = GetNodeConnector(node, outputConnector);

                Randomize(row[0], true);
                Randomize(row[1], false);

                result.Add(row);

                connectorCount += row[0] == null ? 0 : 1;
                connectorCount += row[1] == null ? 0 : 1;
                rowCount++;
            }

            return result;
        }

        private static FunctionBlockNodeConnector GetNodeConnector(FunctionBlockNode node, IFunctionBlockConnector connector)
        {
            if (connector == null)
                return null;

            var conn = new FunctionBlockNodeConnector()
            {
                Connector = connector,
                Description = connector.Description,
                Node = node,
                PortText = "",
                RowIndex = connector.RowIndex
            };

            conn.SetPoolingMode(GetPoolingModeSymbol(connector.PoolingMode));
            conn.SetPortColor(FunctionBlockConnectorColor.Get(connector));

            return conn;
        }

        private static string GetPoolingModeSymbol(FunctionBlockPoolingMode mode) => mode switch
        {
            FunctionBlockPoolingMode.And => "∧",
            FunctionBlockPoolingMode.Any => "..|",
            FunctionBlockPoolingMode.Avg => "Ø",
            FunctionBlockPoolingMode.Max => "▲",
            FunctionBlockPoolingMode.Min => "▼",
            FunctionBlockPoolingMode.Nand => "⊼",
            FunctionBlockPoolingMode.Nor => "⊽",
            FunctionBlockPoolingMode.Not => "¬",
            FunctionBlockPoolingMode.Or => "∨",
            FunctionBlockPoolingMode.Sequence => "⦀",
            FunctionBlockPoolingMode.Sum => "∑",
            _ => throw new NotImplementedException()
        };

        private static string GetRunMode(FunctionBlockRunModeType mode) => mode switch
        {
            FunctionBlockRunModeType.Always => "A",
            FunctionBlockRunModeType.Change => "C",
            FunctionBlockRunModeType.Cycle => "Y",
            FunctionBlockRunModeType.Never => "N",
            _ => throw new NotImplementedException()
        };

        private static void Randomize(FunctionBlockNode node)
        {
            var rnd = new Random();

            if (rnd.NextDouble() < 0.1)
            {
                node.NameBackgroundColor = $"rgb({rnd.Next(256)}, {rnd.Next(256)}, {rnd.Next(256)})";
            }

            if (rnd.NextDouble() < 0.1)
            {
                node.NameForeColor = $"rgb({rnd.Next(256)}, {rnd.Next(256)}, {rnd.Next(256)})";
            }

            var names = new[] { "Mensch, werde wesentlich", "denn wann die Welt vergeht", "So fällt der Zufall weg", "das Wesen, das besteht.", "ᶘᵒᴥᵒᶅ", "(੭ ᐕ)੭*⁾⁾", "(⁎˃ᆺ˂)", "~(=^‥^)ノ☆", "(^._.^)ﾉ", "【・ヘ・?】", "(⌐■_■)", "(つ▀¯▀)つ", "(≼⓪≽◟⋌⋚⋛⋋◞≼⓪≽)" };
            while (rnd.NextDouble() < 0.2)
            {
                node.Name += $" {names[rnd.Next(names.Length)]}";
            }
        }

        private static void Randomize(FunctionBlockNodeConnector conn, bool isInputConnector)
        {
            if (conn == null)
                return;

            var rnd = new Random();

            if (rnd.NextDouble() < 0.05)
            {
                conn.PortText = new[] { "E", "L", "EL" }[rnd.Next(3)];
            }

            if (isInputConnector && rnd.NextDouble() < 0.05)
            {
                conn.SetHasDefaultValue(false);
            }

            if (isInputConnector && rnd.NextDouble() < 0.1)
            {
                conn.SetPoolingMode(GetPoolingModeSymbol((FunctionBlockPoolingMode)rnd.Next(11)));
            }
        }

        public static FunctionBlockNode Run(FunctionBlock functionBlock)
        {
            var node = new FunctionBlockNode(functionBlock.Position)
            {
                ClusterNodeId = functionBlock.ClusterNodeId,
                CycleFrequency = functionBlock.RunMode.CycleFrequency >= 10 ? "*" : functionBlock.RunMode.CycleFrequency.ToString(),
                CycleOffset = functionBlock.RunMode.CycleOffset >= 10 ? "*" : functionBlock.RunMode.CycleOffset.ToString(),
                CyclePriority = functionBlock.RunMode.CyclePriority >= 10 ? "*" : functionBlock.RunMode.CyclePriority.ToString(),
                ImageSrc = string.IsNullOrEmpty(functionBlock.ImageName) ? null : $"/assets/{functionBlock.ImageName}",
                ImageText = functionBlock.ImageText,
                Name = functionBlock.Name,
                RunMode = GetRunMode(functionBlock.RunMode.Mode)
            };
            node.Connectors = GenerateConnectors(functionBlock, node);

            Randomize(node);

            foreach (var connRow in node.Connectors)
            {
                if (connRow[0] != null)
                {
                    connRow[0].Port = new FunctionBlockPort(node, PortAlignment.Left, connRow[0]);
                    node.AddPort(connRow[0].Port);
                }
                if (connRow[1] != null)
                {
                    connRow[1].Port = new FunctionBlockPort(node, PortAlignment.Right, connRow[1]);
                    node.AddPort(connRow[1].Port);
                }
            }

            return node;
        }
    }
}
