using UnityEngine;

namespace BlasII.DebugMod.Hitboxes
{
    public class HitboxColors
    {
        private static readonly Color defaultColor = Color.white;

        public readonly Color inactive;
        public readonly Color geometry;
        public readonly Color player;
        public readonly Color enemy;
        public readonly Color hazard;
        public readonly Color trigger;
        public readonly Color other;

        public HitboxColors(string inactive, string geometry, string player, string enemy, string hazard, string trigger, string other)
        {
            if (!ColorUtility.TryParseHtmlString(inactive, out this.inactive))
                this.inactive = defaultColor;
            if (!ColorUtility.TryParseHtmlString(geometry, out this.geometry))
                this.geometry = defaultColor;
            if (!ColorUtility.TryParseHtmlString(player, out this.player))
                this.player = defaultColor;
            if (!ColorUtility.TryParseHtmlString(enemy, out this.enemy))
                this.enemy = defaultColor;
            if (!ColorUtility.TryParseHtmlString(hazard, out this.hazard))
                this.hazard = defaultColor;
            if (!ColorUtility.TryParseHtmlString(trigger, out this.trigger))
                this.trigger = defaultColor;
            if (!ColorUtility.TryParseHtmlString(other, out this.other))
                this.other = defaultColor;
        }
    }
}
