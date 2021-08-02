using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Threading;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace ClassHelper.ClassHelpers
{
    public static class PetFoodHandler
    {
        public static void Launch()
        {
            try
            {
                do
                {
                    if (PetHappiness > 2 || ObjectManager.Me.InCombat || ObjectManager.Pet.InCombat || ObjectManager.Pet.HaveBuff("Feed Pet Effect") || !ObjectManager.Pet.IsValid || ObjectManager.Pet.IsDead || !SpellManager.KnowSpell("Feed Pet"))
                        return;

                    if (HasFood())
                    {
                        Lua.LuaDoString("ClearCursor();");
                        SpellManager.CastSpellByNameLUA("Feed Pet");
                        Lua.LuaDoString("UseItemByName('" + PetFoodName + "')");
                    }
                    else
                    {
                        Logging.Write("No food is in your bag, you Should go Buy Some.");
                    }
                    Thread.Sleep(4000);
                }
                while (Conditions.InGameAndConnectedAndAliveAndProductStartedNotInPause);
            }
            catch (Exception ex)
            {
                Logging.Write($"PetFoodHandler > Starter > bug > {ex}");
            }
        }

        private static Boolean HasFood()
        {
            PetFoodName = String.Empty;
            try
            {
                foreach (var Petfood in PetFoodDictionary)
                {
                    if (FoodTypes == Petfood.Value.ToString())
                    {
                        if (ItemsManager.GetItemCountByNameLUA(Petfood.Key) > 0)
                        {
                            PetFoodName = Petfood.Key;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Write($"Has Food boolean> bug > {ex}");
                PetFoodName = String.Empty;
            }
            return PetFoodName != String.Empty;
        }


        private static readonly Dictionary<String, Foodtype> PetFoodDictionary = new Dictionary<String, Foodtype>
        {
            {  "Tough Jerky", Foodtype.Meat },
            {  "Haunch of Meat", Foodtype.Meat },
            {  "Mutton Chop", Foodtype.Meat },
            {  "Wild Hog Shank", Foodtype.Meat },
            {  "Cured Ham Steak", Foodtype.Meat },
            {  "Roasted Quail", Foodtype.Meat },
            {  "Smoked Talbuk Venison", Foodtype.Meat },
            {  "Clefthoof Ribs", Foodtype.Meat },
            {  "Salted Venison", Foodtype.Meat },
            {  "Mead Basted Caribou", Foodtype.Meat },
            {  "Mystery Meat", Foodtype.Meat },
            {  "R﻿ed Wolf Mea﻿﻿t", Foodtype.Meat },

            {  "Slitherskin Mackerel", Foodtype.Fish },
            {  "Longjaw Mud Snapper", Foodtype.Fish },
            {  "Bristle Whisker Catfish", Foodtype.Fish },
            {  "Rockscale Cod", Foodtype.Fish },
            {  "Striped Yellowtail", Foodtype.Fish },
            {  "Spinefin Halibut", Foodtype.Fish },
            {  "Sunspring Carp", Foodtype.Fish },
            {  "Zangar Trout", Foodtype.Fish },
            {  "Fillet of Icefin", Foodtype.Fish },
            {  "Poached Emperor Salmon", Foodtype.Fish },

            {  "Shiny Red Apple", Foodtype.Fruit },
            {  "Tel'Abim Banana", Foodtype.Fruit },
            {  "Snapvine Watermelon", Foodtype.Fruit },
            {  "Goldenbark Apple", Foodtype.Fruit },
            {  "Heaven Peach", Foodtype.Fruit },
            {  "Moon Harvest Pumpkin", Foodtype.Fruit },
            {  "Deep Fried Plantains", Foodtype.Fruit },
            {  "Skethyl Berries", Foodtype.Fruit },
            {  "Telaari Grapes", Foodtype.Fruit },
            {  "Tundra Berries", Foodtype.Fruit },
            {  "Savory Snowplum", Foodtype.Fruit },

            {  "Tough Hunk of Bread", Foodtype.Bread },
            {  "Freshly Baked Bread", Foodtype.Bread },
            {  "Moist Cornbread", Foodtype.Bread },
            {  "Mulgore Spice Bread", Foodtype.Bread },
            {  "Soft Banana Bread", Foodtype.Bread },
            {  "Homemade Cherry Pie", Foodtype.Bread },
            {  "Mag'har Grainbread", Foodtype.Bread },
            {  "Crusty Flatbread", Foodtype.Bread },

            { "Raw Black Truffle", Foodtype.Fungus },
        };
        private enum Foodtype
        {
            Meat,
            Fish,
            Fruit,
            Bread,
            Fungus,
        }

        private static String FoodTypes = Lua.LuaDoString<String>("return GetPetFoodTypes();");
        private static Int32 PetHappiness = Lua.LuaDoString<Int32>("local Happiness = GetPetHappiness(); return Happiness;");
        private static String PetFoodName { get; set; }
    }
}
