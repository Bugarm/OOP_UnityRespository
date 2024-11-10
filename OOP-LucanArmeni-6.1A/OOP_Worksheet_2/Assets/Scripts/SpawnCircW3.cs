using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnCircW3 : MonoBehaviour
{

    public GameObject circlePrefab;
    Rigidbody2D rigidbody2;
    //float mousePosX;
    //float mousePosY;
    int circCount;
    // Start is called before the first frame update
    void Start()
    {
        //mousePosX = 0f;
        //mousePosY = 0f;
        Instantiate(circlePrefab, new Vector2(0f, 0f), Quaternion.identity);


    }

    // Update is called once per frame
    void Update()
    {



    }
}
