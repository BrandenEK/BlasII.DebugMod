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

    public static void Prefix(int questId, int varId, bool value)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(value), value));
    }
}

[HarmonyPatch]
class QuestManager_SetQuestInt_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(int));
    }

    public static void Prefix(int questId, int varId, int value)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(value), value));
    }
}

[HarmonyPatch]
class QuestManager_SetQuestFloat_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(float));
    }

    public static void Prefix(int questId, int varId, float value)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(value), value));
    }
}

[HarmonyPatch]
class QuestManager_SetQuestVar_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(string));
    }

    public static void Prefix(int questId, int varId, string value)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(value), value));
    }
}

// Getting quest variables


