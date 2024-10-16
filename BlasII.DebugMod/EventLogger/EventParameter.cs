
namespace BlasII.DebugMod.EventLogger;

/// <summary>
/// Represents a parameter or result to a logging event
/// </summary>
public class EventParameter
{
    /// <summary>
    /// The name of the parameter
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// The value of the parameter
    /// </summary>
    public string Value { get; init; }
}
