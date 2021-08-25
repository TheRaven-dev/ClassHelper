using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Threading;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace ClassHelper.ClassHelpers
{
    public static class MageFoodHandler
    {
        public static void Launch()
        {
            if (ObjectManager.Me.InCombat || Fight.InFight || ObjectManager.Me.IsDead || !SpellManager.KnowSpell(GetSpellInfo[0]) || !Conditions.InGameAndConnectedAndAliveAndProductStartedNotInPause)
                return;

            try
            {
                foreach (var i in Food)
                {
                    if(Int32.Parse(GetSpellInfo[1]) == i.Value)
                    {
                        FoodName = i.Key;
                        FoodCount = ItemsManager.GetItemCountByNameLUA(i.Key);
                        break;
                    }
                }
                if(FoodCount < 1)
                {
                    while(FoodCount <= 20)
                    {
                        SpellManager.CastSpellByNameLUA(GetSpellInfo[0]);
                        Usefuls.WaitIsCasting();
                        FoodCount = ItemsManager.GetItemCountByNameLUA(FoodName);
                    }
                }
                wManager.wManagerSetting.CurrentSetting.FoodName = FoodName;
            }
            catch(Exception ex)
            {
                Logging.WriteError($"Mage Food Handler > Error > {ex}");
            }
            finally
            {
                ClearOldFood();
            }
        }

        private static void ClearOldFood()
        {
            try
            {
                foreach (var T in Food)
                {
                    if (T.Key != FoodName)
                    {
                        if (ItemsManager.GetItemCountByNameLUA(T.Key) > 0)
                        {
                            Bag.PickupContainerItem((int)ItemsManager.GetIdByName(T.Key));
                            Lua.LuaDoString("ClearCursor();");
                            Lua.LuaDoString("DeleteCursorItem();");
                        }
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteError($"Clearing old Food > Bug > {ex}");
            }
            finally
            {
                Logging.WriteDebug("Food Cleaner Finished.");
            }
        }

        private static Dictionary<String, Int32> Food = new Dictionary<string, int>
        {
            { "Conjured Muffin", 1 },
            { "Conjured Bread", 2 },
            { "Conjured Rye", 3 },
            { "Conjured Pumpernickel", 4 },
            { "Conjured Sourdough", 5 },
            { "Conjured Sweet Roll", 6 },
            { "Conjured Cinnamon Roll", 7 },
            { "Conjured Croissant", 8 },
        };
        private static Int32 FoodCount { get; set; }
        private static String FoodName { get; set; }
        private static readonly String[] GetSpellInfo = Lua.LuaDoString<String[]>("return {GetSpellInfo('Conjure Food')};");
    }
}
