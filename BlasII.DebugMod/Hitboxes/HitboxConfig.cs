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
            showGeometry = true;
            updateDelay = 1;

            inactiveColor = "#7F7F7F";
            geometryColor = "#00FF00";
            playerColor = "#00FFFF";
            enemyColor = "#FF0000";
            hazardColor = "#FF00FF";
            triggerColor = "#7F7FFF";
            otherColor = "#FFEB04";
        }
    }
}
