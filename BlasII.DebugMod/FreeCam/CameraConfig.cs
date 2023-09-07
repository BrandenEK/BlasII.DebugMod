
namespace BlasII.DebugMod.FreeCam
{
    public class CameraConfig
    {
        public readonly float movementSpeed;
        public readonly float movementModifier;

        public CameraConfig(float movementSpeed, float movementModifier)
        {
            this.movementSpeed = movementSpeed;
            this.movementModifier = movementModifier;
        }
    }
}
