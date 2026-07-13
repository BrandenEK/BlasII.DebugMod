using HarmonyLib;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.Attack.Data;

namespace BlasII.DebugMod.InfoDisplay;

[HarmonyPatch(typeof(AttackReceivedBlackboard), nameof(AttackReceivedBlackboard.OnAttackReceived))]
class AttackReceivedBlackboard_OnAttackReceived_Patch
{
    public static void Postfix(AttackInfo attackInfo)
    {
        //ModLog.Info("Received hit from AttackReceivedBlackboard");

        string id = attackInfo.attackID?.name;
        int damage = attackInfo.accumulatedDamage;

        var data = attackInfo.attack;

        string physical = string.Empty;
        if (data.physicalAttackSources != null)
        {
            for (int i = 0; i < data.physicalAttackSources.Count; i++)
            {
                physical += $"{data.physicalAttackSources[i].attackType?.name} ";
            }
        }
        physical = physical.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);

        string elemental = string.Empty;
        if (data.elementalAttackSources != null)
        {
            for (int i = 0; i < data.elementalAttackSources.Count; i++)
            {
                elemental += $"{data.elementalAttackSources[i].attackType?.name} ";
            }
        }
        elemental = elemental.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);

        var result = new AttackData(id, damage, physical, elemental);
        Main.DebugMod.InfoModule.OnOffense(result);
    }
}

[HarmonyPatch(typeof(PlayerDeathEventBroadcaster), nameof(PlayerDeathEventBroadcaster.OnAttackReceived))]
class PlayerDeathEventBroadcaster_OnAttackReceived_Patch
{
    public static void Postfix(AttackInfo attackInfo)
    {
        //ModLog.Info("Received hit from PlayerDeathEventBroadcaster");

        string id = attackInfo.attackID?.name;
        int damage = attackInfo.accumulatedDamage;

        var data = attackInfo.attack;

        string physical = string.Empty;
        if (data.physicalAttackSources != null)
        {
            for (int i = 0; i < data.physicalAttackSources.Count; i++)
            {
                physical += $"{data.physicalAttackSources[i].attackType?.name} ";
            }
        }
        physical = physical.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);

        string elemental = string.Empty;
        if (data.elementalAttackSources != null)
        {
            for (int i = 0; i < data.elementalAttackSources.Count; i++)
            {
                elemental += $"{data.elementalAttackSources[i].attackType?.name} ";
            }
        }
        elemental = elemental.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);

        var result = new AttackData(id, damage, physical, elemental);
        Main.DebugMod.InfoModule.OnDefense(result);
    }
}
