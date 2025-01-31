using UnityEngine;

namespace BlasII.DebugMod.InfoDisplay;

internal class FpsTracker
{
    private int frameCount = 0;
    private float dt = 0f;

    public float CurrentFps { get; private set; } = 0f;

    public void Reset()
    {
        frameCount = 0;
        dt = 0f;
        CurrentFps = 0f;
    }

    public void Update()
    {
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / UPDATE_RATE)
        {
            CurrentFps = frameCount / dt;
            frameCount = 0;
            dt -= 1f / UPDATE_RATE;
        }
    }

    private const float UPDATE_RATE = 4f;  // 4 updates per sec.
}
