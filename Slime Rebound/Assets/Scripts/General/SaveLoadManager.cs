using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Unity.VisualScripting;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    SerializedData mydata = new SerializedData();
    SerializedLevelData myLevelData = new SerializedLevelData();
    public bool hasSavedOnce = false;

    public void SaveImportantData()
    {

        mydata.tutorial_highScore = GameData.Tutorial_HighScore;
        mydata.level1_highScore = GameData.Level1_HighScore;


        string jsonToSave = JsonUtility.ToJson(mydata);
        //Debug.Log(jsonToSave);
        //Debug.Log(Application.persistentDataPath);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/ImportantData.json", jsonToSave);

        //BinaryFormatter binaryFormatter = new BinaryFormatter();

        //FileStream myfile = File.Create(Application.persistentDataPath + "/SuperCannonData.save");
        //binaryFormatter.Serialize(myfile, mydata);
        //myfile.Close();

    }

    public void SaveDataCheckPoint(Vector3 playerPos)
    {
        mydata.ser_score = GameData.Score;
        mydata.ser_health = GameData.Hp;
        mydata.chainsLeft = GameData.ChainsInLevel;
        mydata.totalBounces = GameData.TotalBounces;
        mydata.playerPos = playerPos;

        hasSavedOnce = true;

        string jsonToSave = JsonUtility.ToJson(mydata);
        //Debug.Log(jsonToSave);
        //Debug.Log(Application.persistentDataPath);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/CheckPointData.json", jsonToSave);
    }

    public void SaveLevelData(string scene)
    {
        //Debug.Log(scene.name);
        // Updates Chosen Level Data
        SaveSceneData(scene);

        string jsonToSave = JsonUtility.ToJson(myLevelData);
        //Debug.Log(jsonToSave);
        //Debug.Log(Application.persistentDataPath);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/LevelData.json", jsonToSave);
    }

    public void LoadLevelData(string scene)
    {
        // Loads Chosen Level data
        LoadSceneData(scene);

        string loadedJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/LevelData.json");
        //Debug.Log(loadedJson);
        //Debug.Log(Application.persistentDataPath);
        myLevelData = JsonUtility.FromJson<SerializedLevelData>(loadedJson);

    }

    public void LoadImportantData()
    {

        if (File.Exists(Application.persistentDataPath + "/ImportantData.json"))
        {
            string loadedJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/ImportantData.json");
            //Debug.Log(loadedJson);
            //Debug.Log(Application.persistentDataPath);
            mydata = JsonUtility.FromJson<SerializedData>(loadedJson);

            GameData.Tutorial_HighScore = mydata.tutorial_highScore;
            GameData.Level1_HighScore = mydata.level1_highScore;

        }


    }

    public void LoadCheckpointData()
    {
        if (File.Exists(Application.persistentDataPath + "/CheckPointData.json"))
        {
            string loadedJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/CheckPointData.json");
            //Debug.Log(loadedJson);
            //Debug.Log(Application.persistentDataPath);
            mydata = JsonUtility.FromJson<SerializedData>(loadedJson);

            GameData.TotalBounces = mydata.totalBounces;
            GameData.Score = mydata.ser_score;
            GameData.ChainsInLevel = mydata.chainsLeft;
            GameData.PlayerPos = mydata.playerPos;
        }

    }

    public void Reinstantiate(List<SaveableObjects> levelObj)
    {
        for(int i = 0; i < levelObj.Count; i++)
        {
            for (int j = 0; j < FindAllSaveableObj.Instance.placeableObjects.Length; j++) 
            {
                
                if (levelObj[i].id == FindAllSaveableObj.Instance.placeableObjects[j].id)
                {
                    GameObject newObj = Instantiate(FindAllSaveableObj.Instance.placeableObjects[j].prefab);
                    //Debug.Log("A");
                    newObj.transform.position = levelObj[i].ReturnPosition();
                }

            }
        }
    }

    public List<SaveableObjects> GetLevelData(string sceneName)
    {
        switch (sceneName)
        {
            case "TutorialRoom":
                LevelData.SaveableObj_Tutorial = FindAllSaveableObj.Instance.ReturnScriptObj();
                return LevelData.SaveableObj_Tutorial;

            case "TutorialRoom1":
                LevelData.SaveableObj_Tutorial1 = FindAllSaveableObj.Instance.ReturnScriptObj();
                return LevelData.SaveableObj_Tutorial1;

            case "TutorialRoom2":
                LevelData.SaveableObj_Tutorial2 = FindAllSaveableObj.Instance.ReturnScriptObj();
                return LevelData.SaveableObj_Tutorial2;

            default:
                LevelData.SaveableObj_Tutorial = FindAllSaveableObj.Instance.ReturnScriptObj();
                return LevelData.SaveableObj_Tutorial;
        }


    }

    public void SaveSceneData(string sceneName)
    {
        //Debug.Log(sceneName);
        switch (sceneName)
        {
            case "TutorialRoom":
                myLevelData.saveableObj_Tutorial = GetLevelData(sceneName);
                break;

            case "TutorialRoom1":
                myLevelData.saveableObj_Tutorial1 = GetLevelData(sceneName);
                break;

            case "TutorialRoom2":
                myLevelData.saveableObj_Tutorial2 = GetLevelData(sceneName);
                break;
        }
    }

    public void LoadSceneData(string sceneName)
    {
        switch (sceneName)
        {
            case "TutorialRoom":
                LevelData.SaveableObj_Tutorial = myLevelData.saveableObj_Tutorial;
                Reinstantiate(LevelData.SaveableObj_Tutorial);
                break;

            case "TutorialRoom1":
                LevelData.SaveableObj_Tutorial1 = myLevelData.saveableObj_Tutorial1;
                Reinstantiate(LevelData.SaveableObj_Tutorial1);
                break;

            case "TutorialRoom2":
                LevelData.SaveableObj_Tutorial2 = myLevelData.saveableObj_Tutorial2;
                Reinstantiate(LevelData.SaveableObj_Tutorial2);
                break;

        }

        
    }
}