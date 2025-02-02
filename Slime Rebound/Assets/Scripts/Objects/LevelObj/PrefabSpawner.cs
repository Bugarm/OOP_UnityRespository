using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    private GameObject thisObj;

    // Start is called before the first frame update
    void Start()
    {
        thisObj = this.gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItemOnce()
    {

        Instantiate(prefab, thisObj.transform.position, Quaternion.identity);

    }

}
