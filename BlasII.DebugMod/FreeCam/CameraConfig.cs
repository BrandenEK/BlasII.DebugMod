using Newtonsoft.Json;

namespace BlasII.DebugMod.FreeCam
{
    public class CameraConfig
    {
        public readonly float movementSpeed;
        public readonly float movementModifier;

        [JsonConstructor]
        public CameraConfig(float movementSpeed, float movementModifier)
        {
            this.movementSpeed = movementSpeed;
            this.movementModifier = movementModifier;
        }

        public CameraConfig()
        {
            movementSpeed = 0.1f;
            movementModifier = 2.4f;
        }
    }
}
