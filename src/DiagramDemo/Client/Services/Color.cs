using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DiagramDemo.Client.Services
{
    public static class Color
    {
        private static readonly Regex s_rgbColorStringRegex = new Regex(@"rgb\((\d{1,3}), (\d{1,3}), (\d{1,3})\)");
        private static readonly Dictionary<string, double> s_luminanceByColor = new();

        // https://en.wikipedia.org/wiki/Relative_luminance
        public static double GetRelativeLuminance(string rgbColorString)
        {
            if (s_luminanceByColor.TryGetValue(rgbColorString, out var value))
                return value;

            var match = s_rgbColorStringRegex.Match(rgbColorString);

            var r = Convert.ToInt32(match.Groups[1].Value);
            var g = Convert.ToInt32(match.Groups[2].Value);
            var b = Convert.ToInt32(match.Groups[3].Value);

            var luminance = (0.2126 * r / 255) + (0.7152 * g / 255) + (0.0722 * b / 255);
            s_luminanceByColor[rgbColorString] = luminance;
            return luminance;
        }
    }
}
