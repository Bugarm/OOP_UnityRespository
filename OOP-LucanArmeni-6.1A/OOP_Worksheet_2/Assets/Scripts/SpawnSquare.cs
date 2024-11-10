using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSquare : MonoBehaviour
{
    public GameObject squareObj;
    // Start is called before the first frame update
    void Start()
    {
        Camera camera = Camera.main;
        float camPosZ = camera.transform.position.z;

<<<<<<< HEAD
        Instantiate(squareObj,new Vector3(0.52f, 0.52f, camPosZ + 5f),Quaternion.identity);
=======
        Instantiate(squareObj,new Vector3(0.521f, 0.479f, camPosZ + 5f),Quaternion.identity);
>>>>>>> 5f7b9cd514eed3761cfaf7b1f0bcacc54383d574
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
