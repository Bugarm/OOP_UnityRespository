using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    SerializedData myData;
    protected override void Awake()
    {
        base.Awake();
        myData = new SerializedData();
    }
    

    public void SaveData()
    {
        myData.ser_health = GameData.Hp;
        myData.ser_score = GameData.Score;

        string jsonToSave = JsonUtility.ToJson(myData);
        //Debug.Log(jsonToSave);
        //Debug.Log(Application.persistentDataPath);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/SuperCannonData.json", jsonToSave);
    }

    public void LoadData()
    {
        
        if(File.Exists(Application.persistentDataPath + "/SuperCannonData.json"))
        {
            string loadedJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/SuperCannonData.json");

            myData = JsonUtility.FromJson<SerializedData>(loadedJson);
            GameData.Score = myData.ser_score;
            GameData.Hp = myData.ser_health;
        }
        

    }
}
