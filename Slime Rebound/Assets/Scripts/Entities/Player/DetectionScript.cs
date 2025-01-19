using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionScript : MonoBehaviour 
{

    private bool isTouching;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        isTouching = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ReturnDetection()
    {
        return isTouching;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platforms") || collision.CompareTag("Level"))
        {

            isTouching = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platforms") || collision.CompareTag("Level"))
        {
            isTouching = false;
        }
    }
}
