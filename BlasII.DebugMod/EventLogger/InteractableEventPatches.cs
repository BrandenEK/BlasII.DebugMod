using HarmonyLib;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Components.Persistence;

namespace BlasII.DebugMod.EventLogger;

[HarmonyPatch(typeof(RoomInteractable), nameof(RoomInteractable.Use))]
class RoomInteractable_Use_Patch
{
    public static void Postfix(RoomInteractable __instance)
    {
        UniqueId id = __instance.GetComponentInChildren<UniqueId>();

        Main.DebugMod.LoggerModule.LogEvent(nameof(RoomInteractable), nameof(RoomInteractable.Use), EventType.Interactable,
            new EventParameter(nameof(RoomInteractable.name), __instance.name),
            new EventParameter(nameof(UniqueId.uniqueId), id?.uniqueId ?? "Unknown"));
    }
}

[HarmonyPatch(typeof(RoomInteractable), nameof(RoomInteractable.ForceUse))]
class RoomInteractable_ForceUse_Patch
{
    public static void Postfix(RoomInteractable __instance)
    {
        UniqueId id = __instance.GetComponentInChildren<UniqueId>();

        Main.DebugMod.LoggerModule.LogEvent(nameof(RoomInteractable), nameof(RoomInteractable.ForceUse), EventType.Interactable,
            new EventParameter(nameof(RoomInteractable.name), __instance.name),
            new EventParameter(nameof(UniqueId.uniqueId), id?.uniqueId ?? "Unknown"));
    }
}
