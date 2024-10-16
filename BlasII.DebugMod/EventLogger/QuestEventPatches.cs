using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Managers;
using System.Reflection;

namespace BlasII.DebugMod.EventLogger;

internal static class QuestHelper
{
    public static string GetQuestName(int questId, int varId)
    {
        var quest = CoreCache.Quest.GetQuestData(questId, string.Empty);
        var variable = quest.GetVariable(varId);

        return $"{quest.Name}.{variable.id}";
    }
}

// Setting quest variables

[HarmonyPatch]
class QuestManager_SetQuestBool_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(bool));
    }

    public static void Postfix(int questId, int varId, bool value)
    {
        ModLog.Debug($"Setting quest: {QuestHelper.GetQuestName(questId, varId)} ({value})");
    }
}

[HarmonyPatch]
class QuestManager_SetQuestInt_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(int));
    }

    public static void Postfix(int questId, int varId, int value)
    {
        Main.Randomizer.LogWarning($"Setting quest: {Main.Randomizer.GetQuestName(questId, varId)} ({value})");
    }
}
