using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary; //Saving with Binary

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    SerializedData myData = new SerializedData(); //only create once cause of singleton

    public void SaveData()
    {

        myData.ser_health = GameData.Hp;
        myData.ser_score = GameData.Score;

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        // This also overrides the previous file 
        FileStream myFile = File.Create(Application.persistentDataPath + "/SuperCannonData.save");
        binaryFormatter.Serialize(myFile, myData);
        myFile.Close(); // IMPORTANT TO CLOSE

        /* JSON SAVING METHOD
        
        string jsonToSave = JsonUtility.ToJson(myData);
        //Debug.Log(jsonToSave);
        //Debug.Log(Application.persistentDataPath);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/SuperCannonData.json", jsonToSave);
        */
    }

    public void LoadData()
    {

        if (File.Exists(Application.persistentDataPath + "/SuperCannonData.save"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            FileStream myFile = File.Open(Application.persistentDataPath + "/SuperCannonData.save", FileMode.Open);
            myData = (SerializedData)binaryFormatter.Deserialize(myFile); // Parcing aka to convert binary to SerializedData Class
            myFile.Close(); // IMPORTANT TO CLOSE
            GameData.Score = myData.ser_score;
            GameData.Hp = myData.ser_health;
        }

        /* JSON LOAD METHOD
        if (File.Exists(Application.persistentDataPath + "/SuperCannonData.json"))
        {
            string loadedJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/SuperCannonData.json");

            myData = JsonUtility.FromJson<SerializedData>(loadedJson);
            GameData.Score = myData.ser_score;
            GameData.Hp = myData.ser_health;
        }
        */

    }
}
