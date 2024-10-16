using MelonLoader;

namespace BlasII.DebugMod;

internal class Main : MelonMod
{
    public static DebugMod DebugMod { get; private set; }

    public override void OnLateInitializeMelon()
    {
        DebugMod = new DebugMod();
    }
}