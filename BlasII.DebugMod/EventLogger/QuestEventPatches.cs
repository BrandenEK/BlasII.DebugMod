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

    public static string GetQuestName(int questId)
    {
        return CoreCache.Quest.GetQuestData(questId, string.Empty).Name;
    }
}

// Setting quest variables

[HarmonyPatch]
class QuestManager_SetVarBool_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(bool));
    }

    public static void Postfix(int questId, int varId, bool value)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(value), value));
    }
}

[HarmonyPatch]
class QuestManager_SetVarInt_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(int));
    }

    public static void Postfix(int questId, int varId, int value)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(value), value));
    }
}

[HarmonyPatch]
class QuestManager_SetVarFloat_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(float));
    }

    public static void Postfix(int questId, int varId, float value)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(value), value));
    }
}

[HarmonyPatch]
class QuestManager_SetVarString_Patch
{
    public static MethodInfo TargetMethod()
    {
        return typeof(QuestManager).GetMethod("SetQuestVarValue").MakeGenericMethod(typeof(string));
    }

    public static void Postfix(int questId, int varId, string value)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(value), value));
    }
}

// Getting quest variables

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue))]
class QuestManager_GetVarBool_Patch
{
    public static void Postfix(int questId, int varId, bool __result)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        // Don't log all of the symbol quest events
        if (quest.StartsWith("ST18"))
            return;

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.GetQuestVarBoolValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter("result", __result));
    }
}

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarIntValue))]
class QuestManager_GetVarInt_Patch
{
    public static void Postfix(int questId, int varId, int __result)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.GetQuestVarIntValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter("result", __result));
    }
}

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarFloatValue))]
class QuestManager_GetVarFloat_Patch
{
    public static void Postfix(int questId, int varId, float __result)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.GetQuestVarFloatValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter("result", __result));
    }
}

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestVarValue))]
class QuestManager_GetVarString_Patch
{
    public static void Postfix(int questId, int varId, string __result)
    {
        string quest = QuestHelper.GetQuestName(questId, varId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.GetQuestVarValue), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter("result", __result));
    }
}

// Quest status

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.SetQuestStatus), typeof(int), typeof(int))]
class QuestManager_SetQuestStatusInt_Patch
{
    public static void Postfix(int questId, int status)
    {
        string quest = QuestHelper.GetQuestName(questId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestStatus), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(status), status));
    }
}

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.SetQuestStatus), typeof(int), typeof(string))]
class QuestManager_SetQuestStatusString_Patch
{
    public static void Postfix(int questId, string status)
    {
        string quest = QuestHelper.GetQuestName(questId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.SetQuestStatus), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter(nameof(status), status));
    }
}

[HarmonyPatch(typeof(QuestManager), nameof(QuestManager.GetQuestStatus))]
class QuestManager_GetQuestStatus_Patch
{
    public static void Postfix(int questId, string __result)
    {
        string quest = QuestHelper.GetQuestName(questId);

        Main.DebugMod.LoggerModule.LogEvent(nameof(QuestManager), nameof(QuestManager.GetQuestStatus), EventType.Quest,
            new EventParameter(nameof(quest), quest),
            new EventParameter("result", __result));
    }
}
