using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class PersistentDataManager : Manager
    {
        [NonSerialized] public PersistentData persistentData = new();

        private string persistentDataPath;

        public override void Setup()
        {
            base.Setup();
            persistentDataPath = Application.persistentDataPath + "/PersistentData.data";
            Load();
            persistentData.SetupPersistentDataLists();
        }

        public void Save()
        {
            BinaryFormatter binaryFormatter = new();
            string tempPath = persistentDataPath + ".tmp";
            FileStream fileStream = new(tempPath, FileMode.Create);
            binaryFormatter.Serialize(fileStream, persistentData);
            fileStream.Close();
            if (File.Exists(persistentDataPath))
            {
                File.Replace(tempPath, persistentDataPath, null);
            }
            else
            {
                File.Move(tempPath, persistentDataPath);
            }
        }

        private void Load()
        {
            if (File.Exists(persistentDataPath))
            {
                BinaryFormatter binaryFormatter = new();
                FileStream fileStream = new(persistentDataPath, FileMode.Open);
                persistentData = binaryFormatter.Deserialize(fileStream) as PersistentData;
                fileStream.Close();
            }
            else
            {
                SetupDefaultPersistentDataValues();
            }
        }

        public void DeletePersistentData()
        {
            persistentData = new PersistentData();
            Save();
        }

        private void SetupDefaultPersistentDataValues()
        {
            Save();
        }
    }
}