using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace ClassHelper
{
    public static class WeaponEnchants
    {
        public static void EnchantWeapon()
        {
            Boolean HasMainHandEnchant = int.Parse(hasMainHandEnchant[0]) == 1;
            Boolean HasOffHandEnchant = int.Parse(hasMainHandEnchant[4]) == 1;
            try
            {
                if (HasMainHandEnchant || Fight.InFight || ObjectManager.Me.InCombatFlagOnly || ObjectManager.Me.IsDead /*|| HasOffHandEnchant */ )
                    return;

                IEnumerable<uint> MainHandEnchant = EnchantList
                       .Where(i => i.Key <= ObjectManager.Me.Level && ItemsManager.GetItemCountByIdLUA(i.Value) >= 1)
                       .OrderByDescending(i => i.Key)
                       .Select(i => i.Value);

                if (MainHandEnchant.Any())
                {
                    var Enchant = MainHandEnchant.FirstOrDefault();
                    ItemsManager.UseItem(Enchant);
                    Thread.Sleep(10);
                    Lua.LuaDoString("PickupInventoryItem(16)");
                    /*Offhand
                    Lua.LuaDoString("PickupInventoryItem(16)");
                    */
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                Logging.Write($"{ex}");
            }
        }
        private static Dictionary<int, uint> EnchantList = new Dictionary<int, uint>()
        {
            { 79, 41196 },
        };
        private static String[] hasMainHandEnchant => Lua.LuaDoString<String[]>("local GetWeaponEnchant = {GetWeaponEnchantInfo()}; return GetWeaponEnchant; ");
    }
}
