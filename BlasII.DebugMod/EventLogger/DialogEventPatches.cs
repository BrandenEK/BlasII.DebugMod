using HarmonyLib;
using Il2CppTGK.Game.DialogSystem;
using Il2CppTGK.Game.Managers;

namespace BlasII.DebugMod.EventLogger;

/// <summary>
/// Logs the id whenever a dialog is started
/// </summary>
//[HarmonyPatch(typeof(DialogManager), nameof(DialogManager.ShowDialogWithObject))]
//class Dialog_Show_Patch
//{
//    public static void Prefix(Dialog dialog)
//    {
//        Main.Randomizer.Log("Starting dialog: " + dialog.name);
//    }
//}
