using DungeonRush.Cards;
using DungeonRush.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DungeonRush.Saving
{
    public static class SavingSystem
    {
        public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves";

        private const string instantPath = "/instant.txt";
        private const string propertyPath = "/property.txt";
        private const string utilityPath = "/utility.txt";
        private const string itemsPath = "/items.txt";

        public static void Init()
        {
            if(!Directory.Exists(SAVE_FOLDER))
            {
                Directory.CreateDirectory(SAVE_FOLDER);
            }
            Debug.Log(SAVE_FOLDER);
        }

        public static void SaveItems(ItemsData data)
        {
            // BinaryFormatter formatter = new BinaryFormatter();
            // string path = Application.persistentDataPath + itemsPath;
            // FileStream stream = new FileStream(path, FileMode.Create);
            // ItemsData items = new ItemsData(data.purchasedIDs);
            // formatter.Serialize(stream, items);
            // stream.Close();

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(SAVE_FOLDER + itemsPath, json);
        }

        public static void SaveUtilities(int xp, int gold)
        {
            // BinaryFormatter formatter = new BinaryFormatter();
            // string path = Application.persistentDataPath + utilityPath;
            // FileStream stream = new FileStream(path, FileMode.Create);
            // PlayerUtility utilities = new PlayerUtility(xp, gold);
            // formatter.Serialize(stream, utilities);
            // stream.Close();

            string json = JsonUtility.ToJson(new PlayerUtility(xp, gold));
            File.WriteAllText(SAVE_FOLDER + utilityPath, json);
        }

        public static void SaveProperties(int str, int agi, int luck)
        {
            // BinaryFormatter formatter = new BinaryFormatter();
            // string path = Application.persistentDataPath + propertyPath;
            // FileStream stream = new FileStream(path, FileMode.Create);
            // PlayerProperties properties = new PlayerProperties(str, agi, luck);
            // formatter.Serialize(stream, properties);
            // stream.Close();

            string json = JsonUtility.ToJson(new PlayerProperties(str, agi, luck));
            File.WriteAllText(SAVE_FOLDER + propertyPath, json);
        }

        public static void SavePlayerInstantProgress(PlayerCard player)
        {
            // BinaryFormatter formatter = new BinaryFormatter();
            // string path = Application.persistentDataPath + instantPath;
            // FileStream stream = new FileStream(path, FileMode.Create);
            // PlayerData data = new PlayerData(player);
            // formatter.Serialize(stream, data);
            // stream.Close();

            string json = JsonUtility.ToJson(new PlayerData(player));
            File.WriteAllText(SAVE_FOLDER + instantPath, json);
        }

        public static ItemsData LoadItems()
        {
            // string path = Application.persistentDataPath + itemsPath;
            // try
            // {
            //     if (File.Exists(path))
            //     {
            //         BinaryFormatter formatter = new BinaryFormatter();
            //         FileStream stream = new FileStream(path, FileMode.Open);
            //         ItemsData data = formatter.Deserialize(stream) as ItemsData;
            //         stream.Close();
            //         return data;
            //     }
            //     else
            //     {
            //         ItemsData data = new ItemsData(null);
            //         return data;
            //     }
            // }
            // catch (System.Exception)
            // {
            //     ItemsData data = new ItemsData(null);
            //     return data;
            // }

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
            // string path = Application.persistentDataPath + utilityPath;
            // try
            // {
            //     if (File.Exists(path))
            //     {
            //         BinaryFormatter formatter = new BinaryFormatter();
            //         FileStream stream = new FileStream(path, FileMode.Open);
            //         PlayerUtility data = formatter.Deserialize(stream) as PlayerUtility;
            //         stream.Close();
            //         return data;
            //     }
            //     else
            //     {
            //         PlayerUtility data = new PlayerUtility(0, 0);
            //         return data;
            //     }
            // }
            // catch (System.Exception)
            // {
            //     PlayerUtility data = new PlayerUtility(0, 0);
            //     return data;
            // }

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
            // string path = Application.persistentDataPath + propertyPath;
            // Debug.Log(path);
            // try
            // {
            //     if (File.Exists(path))
            //     {
            //         BinaryFormatter formatter = new BinaryFormatter();
            //         FileStream stream = new FileStream(path, FileMode.Open);
            //         PlayerProperties data = formatter.Deserialize(stream) as PlayerProperties;
            //         stream.Close();
            //         return data;
            //     }
            //     else
            //     {
            //         PlayerProperties data = new PlayerProperties(0, 0, 0);
            //         return data;
            //     }
            // }
            // catch (System.Exception)
            // {
            //     PlayerProperties data = new PlayerProperties(0, 0, 0);
            //     return data;
            // }

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
            // string path = Application.persistentDataPath + instantPath;

            // Debug.Log(Application.persistentDataPath);

            // if (File.Exists(path))
            // {
            //     BinaryFormatter formatter = new BinaryFormatter();
            //     FileStream stream = new FileStream(path, FileMode.Open);

            //     PlayerData data = formatter.Deserialize(stream) as PlayerData;
            //     stream.Close();

            //     return data;
            // }
            // else
            // {
            //     Debug.Log("Save file not found in " + path);
            //     return null;
            // }

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
