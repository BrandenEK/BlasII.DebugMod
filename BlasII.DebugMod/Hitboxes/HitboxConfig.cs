
namespace BlasII.DebugMod.Hitboxes
{
    public class HitboxConfig
    {
        public readonly bool useColor;
        public readonly bool showGeometry;
        public readonly float updateDelay;

        public readonly string inactiveColor;
        public readonly string geometryColor;
        public readonly string playerColor;
        public readonly string enemyColor;
        public readonly string hazardColor;
        public readonly string triggerColor;
        public readonly string otherColor;

        public HitboxConfig(bool useColor, bool showGeometry, float updateDelay)
        {
            this.useColor = useColor;
            this.showGeometry = showGeometry;
            this.updateDelay = updateDelay;
        }
    }
}
