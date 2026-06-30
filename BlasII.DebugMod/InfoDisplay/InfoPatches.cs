using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.Attack;
using Il2CppTGK.Game.Components.Attack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlasII.DebugMod.InfoDisplay;

[HarmonyPatch(typeof(AttackReceivedBlackboard), nameof(AttackReceivedBlackboard.OnAttackReceived))]
class x_Patch
{
    public static void Postfix(AttackReceivedBlackboard __instance, AttackInfo attackInfo)
    {
        //ModLog.Info("Received hit for " + __instance.gameObject.name);

        //int damage = attackInfo.accumulatedDamage;
        //ModLog.Warn(damage);

        //if (attackInfo.physicalAttacks != null)
        //{
        //    foreach (var x in attackInfo.physicalAttacks)
        //    {
        //        ModLog.Error(x?.Key?.name);
        //    }
        //}

        //if (attackInfo.elementalAttacks != null)
        //{
        //    foreach (var x in attackInfo.elementalAttacks)
        //    {
        //        ModLog.Error(x?.Key?.name);
        //    }
        //}

        //string physical = attackInfo.physicalAttacks == null || attackInfo.physicalAttacks.Length == 0
        //    ? null
        //    : string.Join(", ", attackInfo.physicalAttacks.Select(x => x?.Key?.name).Where(x => x != null)).Replace(" Attack", string.Empty).Replace("Damage", string.Empty);
        //string elemental = attackInfo.elementalAttacks == null || attackInfo.elementalAttacks.Length == 0
        //    ? null
        //    : string.Join(", ", attackInfo.elementalAttacks.Select(x => x?.Key?.name).Where(x => x != null)).Replace(" Attack", string.Empty).Replace("Damage", string.Empty);

        //Main.DebugMod.InfoModule.OnOffense(new AttackData(damage, physical, elemental));
    }
}

[HarmonyPatch(typeof(AttackAbility), nameof(AttackAbility.AttackHit))]
class y_Patch
{
    public static void Postfix(AttackAbility __instance, AttackInfo attack)
    {
        ModLog.Info("Received hit for " + __instance.gameObject.name);

        string id = attack.attackID?.name;
        int damage = attack.accumulatedDamage;
        ModLog.Error(damage);

        var data = attack.attack;

        string physical = string.Empty;
        if (data.physicalAttackSources != null)
        {
            for (int i = 0; i < data.physicalAttackSources.Count; i++)
            {
                physical += $"{data.physicalAttackSources[i].attackType?.name} ";
            }
        }
        physical = physical.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);
        ModLog.Warn(physical);

        string elemental = string.Empty;
        if (data.elementalAttackSources != null)
        {
            for (int i = 0; i < data.elementalAttackSources.Count; i++)
            {
                elemental += $"{data.elementalAttackSources[i].attackType?.name} ";
            }
        }
        elemental = elemental.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);
        ModLog.Warn(elemental);

        //if (attack.physicalAttacks != null)
        //{
        //    foreach (var x in attack.physicalAttacks)
        //    {
        //        ModLog.Error(x?.Key?.name ?? "none");
        //    }
        //}

        //if (attack.elementalAttacks != null)
        //{
        //    foreach (var x in attack.elementalAttacks)
        //    {
        //        ModLog.Error(x?.Key?.name ?? "none");
        //    }
        //}

        //string physical = attack.physicalAttacks == null || attack.physicalAttacks.Length == 0
        //    ? null
        //    : string.Join(", ", attack.physicalAttacks.Select(x => x?.Key?.name).Where(x => x != null)).Replace(" Attack", string.Empty).Replace("Damage", string.Empty);
        //string elemental = attack.elementalAttacks == null || attack.elementalAttacks.Length == 0
        //    ? null
        //    : string.Join(", ", attack.elementalAttacks.Select(x => x?.Key?.name).Where(x => x != null)).Replace(" Attack", string.Empty).Replace("Damage", string.Empty);

        var result = new AttackData(id, damage, physical, elemental);
        if (__instance.gameObject.name == "attack_ability")
            Main.DebugMod.InfoModule.OnDefense(result);
        else
            Main.DebugMod.InfoModule.OnOffense(result);
    }
}
