using HarmonyLib;
using Il2CppTGK.Game.Components.Inventory;
using Il2CppTGK.Inventory;

namespace BlasII.DebugMod.EventLogger;

// Adding items

[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddItemAsync))]
class InventoryComponent_AddItemAsync_Patch
{
    public static void Postfix(ItemID itemID)
    {
        Main.DebugMod.LoggerModule.LogEvent(nameof(InventoryComponent), nameof(InventoryComponent.AddItemAsync), EventType.Inventory,
            new EventParameter(nameof(ItemID.name), itemID.name));
    }
}

//[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddFigureAsync))]
//class InventoryComponent_AddFigureAsync_Patch
//{
//    public static void Postfix(FigureItemID itemId)
//    {
//        Main.DebugMod.LoggerModule.LogEvent(nameof(InventoryComponent), nameof(InventoryComponent.AddFigureAsync), EventType.Inventory,
//            new EventParameter(nameof(ItemID.name), itemId.name));
//    }
//}

//[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddPrayerAsync))]
//class InventoryComponent_AddPrayerAsync_Patch
//{
//    public static void Postfix(PrayerItemID itemId)
//    {
//        Main.DebugMod.LoggerModule.LogEvent(nameof(InventoryComponent), nameof(InventoryComponent.AddPrayerAsync), EventType.Inventory,
//            new EventParameter(nameof(ItemID.name), itemId.name));
//    }
//}

//[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.AddRosaryBeadAsync))]
//class InventoryComponent_AddRosaryBeadAsync_Patch
//{
//    public static void Postfix(RosaryBeadItemID itemId)
//    {
//        Main.DebugMod.LoggerModule.LogEvent(nameof(InventoryComponent), nameof(InventoryComponent.AddRosaryBeadAsync), EventType.Inventory,
//            new EventParameter(nameof(ItemID.name), itemId.name));
//    }
//}

// Removing items

[HarmonyPatch(typeof(InventoryComponent), nameof(InventoryComponent.RemoveItem))]
class InventoryComponent_RemoveItem_Patch
{
    public static void Postfix(ItemID itemID)
    {
        Main.DebugMod.LoggerModule.LogEvent(nameof(InventoryComponent), nameof(InventoryComponent.RemoveItem), EventType.Inventory,
            new EventParameter(nameof(ItemID.name), itemID.name));
    }
}
