
namespace BlasII.DebugMod.EventLogger;

/// <summary>
/// Represents a parameter or result to a logging event
/// </summary>
public class EventParameter(string name, object value)
{
    /// <summary>
    /// The name of the parameter
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// The value of the parameter
    /// </summary>
    public object Value { get; } = value;
}
