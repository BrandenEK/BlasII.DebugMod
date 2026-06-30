
namespace BlasII.DebugMod.InfoDisplay;

public class AttackData(string id, int damage, string physical, string elemental)
{
    public string ID { get; } = id;

    public int Damage { get; } = damage;

    public string PhysicalAttacks { get; } = physical;

    public string ElementalAttacks { get; } = elemental;
}
