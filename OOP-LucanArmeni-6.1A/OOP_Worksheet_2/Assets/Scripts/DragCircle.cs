using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragCircle : MonoBehaviour
{
    //public GameObject circlePrefab;
    Vector3 mousePos;
    public bool draggable;
    bool dragging;

    // Start is called before the first frame update
    void Start()
    {
        dragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(dragging == true && draggable == true) 
        { 
            Camera camera = Camera.main;
            float camPosZ = camera.transform.position.z;
            mousePos = camera.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0f, 0f, camPosZ);

            this.gameObject.transform.position = mousePos;
            
        }

        
    }

    private void OnMouseDown()
    {
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
