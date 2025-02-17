using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetect : MonoBehaviour
{

    private BoxCollider2D fakeWallTriggerCol;
    private int touchingWall;

    // Start is called before the first frame update
    void Start()
    {
        fakeWallTriggerCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (touchingWall <= 0)
        {
            if (collision.CompareTag("FakeWall"))
            {
                //touchingWall = 0;
                PlayerState.IsFakeWallAllowed = true;
            }
        }
        else if(PlayerState.InFakeWall == false)
        {
            PlayerState.IsFakeWallAllowed = false;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Level"))
        {
            touchingWall++;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Level") && touchingWall > 0)
        {
            touchingWall--;   
        }        

    }
}
