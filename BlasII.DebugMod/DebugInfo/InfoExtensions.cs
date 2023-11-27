using System;

namespace BlasII.DebugMod.DebugInfo
{
    public static class InfoExtensions
    {
        public static string RoundToPrecision(this float num)
        {
            int precision = Math.Max(Main.DebugMod.DebugSettings.infoDisplayPrecision, 1);
            return num.ToString("F" + precision);
        }
    }
}
