using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppLightbug.Kinematic2D.Implementation;
using Il2CppTGK.Game.Achievements;
using Il2CppTGK.Game.Achievements.Components;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.Attack;
using Il2CppTGK.Game.Components.Attack.Data;
using Il2CppTGK.Game.Components.Defense;
using Il2CppTGK.Game.Components.Health;
using Il2CppTGK.Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BlasII.DebugMod.InfoDisplay;

[HarmonyPatch(typeof(AttackReceivedBlackboard), nameof(AttackReceivedBlackboard.OnAttackReceived))]
class x_Patch
{
    public static void Postfix(AttackReceivedBlackboard __instance, AttackInfo attackInfo)
    {
        ModLog.Info("Received hit for  blackboard");

        string id = attackInfo.attackID?.name;
        int damage = attackInfo.accumulatedDamage;
        ModLog.Error(damage);

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

        var result = new AttackData(id, damage, physical, elemental);
        Main.DebugMod.InfoModule.OnOffense(result);
    }
}

// Crashes
//[HarmonyPatch(typeof(HitFeedback), nameof(HitFeedback.AttackReceived))]
//class u
//{
//    public static void Postfix(HitFeedback __instance, AttackInfo attack)
//    {
//        ModLog.Error(__instance.name);
//        ModLog.Info(attack.accumulatedDamage);
//    }
//}

[HarmonyPatch(typeof(PlayerDeathEventBroadcaster), nameof(PlayerDeathEventBroadcaster.OnAttackReceived))]
class v
{
    public static void Postfix(AttackInfo attackInfo)
    {
        ModLog.Info("Received hit for playerdeath");

        string id = attackInfo.attackID?.name;
        int damage = attackInfo.accumulatedDamage;
        ModLog.Error(damage);

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

        var result = new AttackData(id, damage, physical, elemental);
        Main.DebugMod.InfoModule.OnDefense(result);
    }
}

//[HarmonyPatch(typeof(AttackAbility), nameof(AttackAbility.AttackHit))]
[HarmonyPatch(typeof(AttackResolutionSystem), nameof(AttackResolutionSystem.BuildAttackInfo))]
class y_Patch
{
    public static void Postfix(AttackResolutionSystem __instance, CharacterController2D owner)
    {
        //ModLog.Info("Received hit for " + owner.gameObject.name);
        //var attackInfo = __instance.attackInfo;

        //string id = attackInfo.attackID?.name;
        //int damage = attackInfo.accumulatedDamage;
        //ModLog.Error(damage);

        //var data = attackInfo.attack;

        //string physical = string.Empty;
        //if (data.physicalAttackSources != null)
        //{
        //    for (int i = 0; i < data.physicalAttackSources.Count; i++)
        //    {
        //        physical += $"{data.physicalAttackSources[i].attackType?.name} ";
        //    }
        //}
        //physical = physical.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);
        //ModLog.Warn(physical);

        //string elemental = string.Empty;
        //if (data.elementalAttackSources != null)
        //{
        //    for (int i = 0; i < data.elementalAttackSources.Count; i++)
        //    {
        //        elemental += $"{data.elementalAttackSources[i].attackType?.name} ";
        //    }
        //}
        //elemental = elemental.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);
        //ModLog.Warn(elemental);

        //var result = new AttackData(id, damage, physical, elemental);
        //if (owner.gameObject.name == "#Main Player")
        //    Main.DebugMod.InfoModule.OnDefense(result);
        //else
        //    Main.DebugMod.InfoModule.OnOffense(result);
    }
}

//[HarmonyPatch(typeof(AttackReceiverComponent), nameof(AttackReceiverComponent.OnAttackReceive), typeof(AttackHit), typeof(Collision2D))]
//class y_Patch
//{
//    public static void Postfix(AttackReceiverComponent __instance)
//    {
//        ModLog.Info("Received hit for " + __instance.gameObject.name);
//        var attack = __instance.GetSystem().attackInfo;

//        string id = attack.attackID?.name;
//        int damage = attack.accumulatedDamage;
//        ModLog.Error(damage);

//        var data = attack.attack;

//        string physical = string.Empty;
//        if (data.physicalAttackSources != null)
//        {
//            for (int i = 0; i < data.physicalAttackSources.Count; i++)
//            {
//                physical += $"{data.physicalAttackSources[i].attackType?.name} ";
//            }
//        }
//        physical = physical.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);
//        ModLog.Warn(physical);

//        string elemental = string.Empty;
//        if (data.elementalAttackSources != null)
//        {
//            for (int i = 0; i < data.elementalAttackSources.Count; i++)
//            {
//                elemental += $"{data.elementalAttackSources[i].attackType?.name} ";
//            }
//        }
//        elemental = elemental.Replace(" Attack", string.Empty).Replace("Damage", string.Empty);
//        ModLog.Warn(elemental);

//        var result = new AttackData(id, damage, physical, elemental);
//        if (__instance.gameObject.name == "#Main Player")
//            Main.DebugMod.InfoModule.OnDefense(result);
//        else
//            Main.DebugMod.InfoModule.OnOffense(result);
//    }
//}
