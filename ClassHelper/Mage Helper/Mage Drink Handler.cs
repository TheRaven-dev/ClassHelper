using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Threading;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using wManager;

namespace ClassHelper.ClassHelpers
{
    public static class DrinkHandler
    {
        public static void Launch()
        {
            if (ObjectManager.Me.InCombat || Fight.InFight || ObjectManager.Me.IsDead || !SpellManager.KnowSpell(GetSpellInfo[0]))
                return;

            try
            {
                foreach (var i in Water)
                {
                    if (Int32.Parse(GetSpellInfo[1]) == i.Value)
                    {
                        DrinkName = i.Key;
                        DrinkCount = ItemsManager.GetItemCountByNameLUA(i.Key);
                        break;
                    }
                }
                if (DrinkCount < 1)
                {
                    while (DrinkCount <= 20)
                    {
                        SpellManager.CastSpellByNameLUA(GetSpellInfo[0]);
                        Usefuls.WaitIsCasting();
                        DrinkCount = ItemsManager.GetItemCountByNameLUA(DrinkName);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError($"Mage Water Handler > Error > {ex}");
            }
            finally
            {
                wManagerSetting.CurrentSetting.DrinkName = DrinkName;
                ClearOldWater();
            }
        }

        private static void ClearOldWater()
        {
            try
            {
                foreach (var C in Water)
                {
                    if (C.Key != DrinkName)
                    {
                        if (ItemsManager.GetItemCountByNameLUA(C.Key) > 0)
                        {
                            Bag.PickupContainerItem((int)ItemsManager.GetIdByName(C.Key));
                            Lua.LuaDoString("ClearCursor();");
                            Lua.LuaDoString("DeleteCursorItem();");
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError($"Clearing old Water > Bug > {ex}");
            }
            finally
            {
                Logging.WriteDebug("Water Cleaner Finished.");
            }
        }
        private static Dictionary<String, Int32> Water = new Dictionary<string, int>
        {
            { "Conjured Water", 1},
            { "Conjured Fresh Water", 2},
            { "Conjured Purified Water", 3},
            { "Conjured Spring Water", 4},
            { "Conjured Mineral Water", 5},
            { "Conjured Sparkling Water", 6},
            { "Conjured Crystal Water", 7},
            { "Conjured Mountain Spring Water", 8},
            { "Conjured Glacier Water", 9},
        };

        private static Int32 DrinkCount { get; set; }
        private static String DrinkName { get; set; }
        private static readonly String[] GetSpellInfo = Lua.LuaDoString<String[]>("return {GetSpellInfo('Conjure Water')};");
    }
}
