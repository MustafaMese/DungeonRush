using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DungeonRush.Saving
{
    public static class SavingSystem
    {
        private const string playerPath = "/playerInstant.sav";
        public static void SavePlayerInstantProgress(Card player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + playerPath;
            Debug.Log(path);
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static PlayerData LoadPlayerInstantProgress()
        {
            string path = Application.persistentDataPath + playerPath;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.Log("Save file not found in " + path);
                return null;
            }
        }

        public static void DeletePlayerInstantSaveFile()
        {
            string path = Application.persistentDataPath + playerPath;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
