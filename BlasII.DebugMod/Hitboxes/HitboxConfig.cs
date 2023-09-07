using Newtonsoft.Json;

namespace BlasII.DebugMod.Hitboxes
{
    public class HitboxConfig
    {
        public readonly bool showGeometry;
        public readonly float updateDelay;

        public readonly string inactiveColor;
        public readonly string geometryColor;
        public readonly string playerColor;
        public readonly string enemyColor;
        public readonly string hazardColor;
        public readonly string triggerColor;
        public readonly string otherColor;

        [JsonConstructor]
        public HitboxConfig(bool showGeometry, float updateDelay, string inactiveColor, string geometryColor, string playerColor, string enemyColor, string hazardColor, string triggerColor, string otherColor)
        {
            this.showGeometry = showGeometry;
            this.updateDelay = updateDelay;
            this.inactiveColor = inactiveColor;
            this.geometryColor = geometryColor;
            this.playerColor = playerColor;
            this.enemyColor = enemyColor;
            this.hazardColor = hazardColor;
            this.triggerColor = triggerColor;
            this.otherColor = otherColor;
        }

        public HitboxConfig()
        {
            showGeometry = false;
            updateDelay = 1;

            inactiveColor = "#000000";
            geometryColor = "#000000";
            playerColor = "#000000";
            enemyColor = "#000000";
            hazardColor = "#000000";
            triggerColor = "#000000";
            otherColor = "#000000";
        }
    }
}
