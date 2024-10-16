using System;

namespace BlasII.DebugMod.DebugInfo;

internal static class InfoExtensions
{
    public static string RoundToPrecision(this float num)
    {
        int precision = Math.Max(Main.DebugMod.DebugSettings.InfoDisplay.Precision, 1);
        return num.ToString("F" + precision);
    }
}
