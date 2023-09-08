using Newtonsoft.Json;

namespace BlasII.DebugMod.NoClip
{
    public class NoclipConfig
    {
        public readonly float movementSpeed;
        public readonly float movementModifier;

        [JsonConstructor]
        public NoclipConfig(float movementSpeed, float movementModifier)
        {
            this.movementSpeed = movementSpeed;
            this.movementModifier = movementModifier;
        }

        public NoclipConfig()
        {
            movementSpeed = 0.1f;
            movementModifier = 1.5f;
        }
    }
}
