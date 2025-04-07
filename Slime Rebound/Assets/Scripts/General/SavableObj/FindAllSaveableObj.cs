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
        int returnId = 0;

        for (int i = 0; i < scoreItems.Length; i++)
        {
            
            if(scoreItems[i].name.StartsWith("Food") == true)
            {
                returnId = 0;
                saveableObjects.Add(new SaveableObjects(placeableObjects[returnId].id, scoreItems[i].transform.position));
            }
            else if(scoreItems[i].name.StartsWith("5Score") == true)
            {
                returnId = 3;
                saveableObjects.Add(new SaveableObjects(placeableObjects[returnId].id, scoreItems[i].transform.position));
            }
            else if (scoreItems[i].name.StartsWith("10Score") == true)
            {
                returnId = 4;
                saveableObjects.Add(new SaveableObjects(placeableObjects[returnId].id, scoreItems[i].transform.position));
            }
            else if (scoreItems[i].name.StartsWith("50Score") == true)
            {
                returnId = 5;
                saveableObjects.Add(new SaveableObjects(placeableObjects[returnId].id, scoreItems[i].transform.position));
            }
            else if (scoreItems[i].name.StartsWith("100Score") == true)
            {
                returnId = 6;
                saveableObjects.Add(new SaveableObjects(placeableObjects[returnId].id, scoreItems[i].transform.position));
            }
        }


        GameObject[] chainsInLevel = GameObject.FindGameObjectsWithTag("SkeleChain");
        for (int i = 0; i < chainsInLevel.Length; i++)
        {
            saveableObjects.Add(new SaveableObjects(placeableObjects[1].id, chainsInLevel[i].transform.position));
        }

        GameObject[] boxesInLevel = GameObject.FindGameObjectsWithTag("Box");
        for(int i = 0; i < boxesInLevel.Length; i++)
        {
            saveableObjects.Add(new SaveableObjects(placeableObjects[2].id, boxesInLevel[i].transform.position));
        }

        return saveableObjects;
    }

}
