using HarmonyLib;
using Il2CppTGK.Game.Components.Interactables;

namespace BlasII.DebugMod.EventLogger;

//[HarmonyPatch(typeof(IInteractable), nameof(IInteractable.Use))]
//class IInteractable_Use_Patch
//{
//    public static void Postfix(IInteractable __instance)
//    {
//        Main.DebugMod.LoggerModule.LogEvent(nameof(IInteractable), nameof(IInteractable.Use), EventType.Interactable,
//            new EventParameter("object", __instance.GetInteractionTransform().name));
//    }
//}
