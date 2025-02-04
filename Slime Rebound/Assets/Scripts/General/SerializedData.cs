using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SerializedData
{
    public int ser_score;
    public int ser_health;
    public Vector3 playerPos;
    public int tutorial_highScore;
    public int level1_highScore;
    public int chainsLeft;
    public int totalBounces;
}

[System.Serializable]

public class SerializedLevelData
{
    public List<SaveableObjects> saveableObj_Tutorial;
    public List<SaveableObjects> saveableObj_Tutorial1;
    public List<SaveableObjects> saveableObj_Tutorial2;
}

[System.Serializable]

public class SerializedTransData
{
    public int nextSceneID;
}


