using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FindAllSaveableObj : Singleton<FindAllSaveableObj>
{
    public Identification[] placeableObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<SaveableObjects> ReturnScriptObj()
    {
        List<SaveableObjects> saveableObjects = new List<SaveableObjects>();
        GameObject[] scoreItems = GameObject.FindGameObjectsWithTag("Collectables");
        for (int i = 0; i < scoreItems.Length; i++)
        {
            int returnId = 0;
            if(scoreItems[i].name.StartsWith("Food") == true)
            {
                returnId = 0;
            }
            else if(scoreItems[i].name.StartsWith("Score") == true)
            {
                returnId = 1;
            }

            saveableObjects.Add(new SaveableObjects(placeableObjects[returnId].id, scoreItems[i].transform.position));
        }

        return saveableObjects;
    }
}
