using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnCirc : MonoBehaviour
{

    public GameObject circlePrefab;
    Vector3 mouseVec;
    //float mousePosX;
    //float mousePosY;
    int circCount;
    // Start is called before the first frame update
    void Start()
    {
        //mousePosX = 0f;
        //mousePosY = 0f;

        circCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;

        //mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Mouse mouse = Mouse.current;
        float camPosZ = camera.transform.position.z;


        if (mouse.leftButton.wasPressedThisFrame && circCount < 5)
        {
            // Old Attempt

            //mousePosX = camera.ScreenToWorldPoint(Input.mousePosition).x;
            //mousePosY = camera.ScreenToWorldPoint(Input.mousePosition).y;

            //Instantiate(circlePrefab, new Vector3(mousePosX, mousePosY, camPosZ), Quaternion.identity);

            // New Code

            mouseVec = camera.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0f, 0f, camPosZ);

            Instantiate(circlePrefab, mouseVec, Quaternion.identity);

            circCount++;
        }
        
      
    }
}
