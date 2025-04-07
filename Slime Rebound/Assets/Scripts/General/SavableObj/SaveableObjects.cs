using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveableObjects 
{

    public string id;
    public float px, py, pz;

    public SaveableObjects(string id, Vector3 position)
    {
        this.id = id;

        px = position.x;
        py = position.y;
        pz = position.z;
    }

    public Vector3 ReturnPosition()
    {
        Vector3 pos = new Vector3(px, py, pz);
        return pos;
    }

}

[System.Serializable]
public class Identification
{
    public string id;
    public GameObject prefab;
}

