using System.Collections.Generic;
using UnityEngine;

namespace WoodenGUI
{
    public class Menu : MonoBehaviour
    {
        public static Dictionary<Menus, string> MenuPaths = new Dictionary<Menus, string>
        {
            {Menus.Inventory, "WoodenGUI/InventoryMenu"},
            {Menus.Main, "WoodenGUI/MainMenu"},
            {Menus.Leaders, "WoodenGUI/LeadersMenu"},
            {Menus.Settings, "WoodenGUI/SettingsMenu"},
            {Menus.Shop, "WoodenGUI/ShopMenu"},
            {Menus.Loading, "WoodenGUI/LoadingMenu"},
            {Menus.Reward, "WoodenGUI/RewardMenu"},
            {Menus.Complete, "WoodenGUI/CompleteMenu"},
            {Menus.Level, "WoodenGUI/LevelMenu"},
            {Menus.Confirmation, "WoodenGUI/ConfirmationMenu"},
            {Menus.Prefabs, "WoodenGUI/Prefabs"}
        };
        
        public enum Menus
        {
            Main,
            Inventory,
            Shop,
            Settings,
            Leaders,
            Loading,
            Reward,
            Complete,
            Level,
            Confirmation,
            Prefabs
        }
    }
}