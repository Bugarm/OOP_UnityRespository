using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    private GameObject thisObj;
    private SpriteRenderer thisRenderer;

    public int customScore;

    // Start is called before the first frame update
    void Start()
    {
        thisObj = this.gameObject;
        thisRenderer = thisObj.GetComponent<SpriteRenderer>();

        // Used for Debugging reasons
        if (thisRenderer != null)
        {
            Destroy(thisRenderer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItemOnce()
    {

        GameObject obj = Instantiate(prefab, thisObj.transform.position, Quaternion.identity);

        GameObject group = GameObject.Find("Objects");

        // Custom Score Types
        if(obj.name.StartsWith("ScoreIncrease"))
        {
            obj.GetComponent<ScoreItem>().score = customScore;

            if (customScore >= 100)
            {
                obj.GetComponent<SpriteRenderer>().color = Color.green;
                obj.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
            }
            else if (customScore >= 50)
            { 
                obj.GetComponent<SpriteRenderer>().color = Color.magenta;
                obj.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
            else if (customScore <= 5)
            {
                obj.GetComponent<SpriteRenderer>().color = Color.blue;
                obj.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
            }

        }

        if (group != null)
        {
            obj.transform.parent = group.transform;
        }

    }

}
