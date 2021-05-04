using DungeonRush.Cards;
using DungeonRush.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DungeonRush.Saving
{
    public static class SavingSystem
    {
        public static readonly string SAVE_FOLDER = Application.persistentDataPath;

        private const string instantPath = "/instant.txt";
        private const string propertyPath = "/property.txt";
        private const string utilityPath = "/utility.txt";
        private const string itemsPath = "/items.txt";

        public static void SaveItems(ItemsData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(SAVE_FOLDER + itemsPath, json);
        }

        public static void SaveUtilities(int xp, int gold)
        {
            string json = JsonUtility.ToJson(new PlayerUtility(xp, gold));
            File.WriteAllText(SAVE_FOLDER + utilityPath, json);
        }

        public static void SaveProperties(int str, int agi, int luck)
        {
            string json = JsonUtility.ToJson(new PlayerProperties(str, agi, luck));
            File.WriteAllText(SAVE_FOLDER + propertyPath, json);
        }

        public static void SavePlayerInstantProgress(PlayerCard player)
        {
            string json = JsonUtility.ToJson(new PlayerData(player));
            File.WriteAllText(SAVE_FOLDER + instantPath, json);
        }

        public static ItemsData LoadItems()
        {
            string path = SAVE_FOLDER + itemsPath;
            if(File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<ItemsData>(json);
            }
            else
                return new ItemsData(null);
        }

        public static PlayerUtility LoadUtilities()
        {
            string path = SAVE_FOLDER + utilityPath;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<PlayerUtility>(json);
            }
            else
                return new PlayerUtility(0, 0);
        }

        public static PlayerProperties LoadPlayerProperties()
        {

            string path = SAVE_FOLDER + propertyPath;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<PlayerProperties>(json);
            }
            else 
                return new PlayerProperties(0, 0, 0);
            
        }

        public static PlayerData LoadPlayerInstantProgress()
        {

            string path = SAVE_FOLDER + instantPath;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<PlayerData>(json);
            }
            else 
                return null;
        }

        public static void DeletePlayerInstantSaveFile()
        {
            string path = SAVE_FOLDER + instantPath;
            string json = JsonUtility.ToJson(null);
            File.WriteAllText(SAVE_FOLDER + instantPath, json);
        }
    }
}
