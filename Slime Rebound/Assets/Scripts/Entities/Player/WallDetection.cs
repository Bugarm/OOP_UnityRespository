using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    public static WallDetection instance;

    private BoxCollider2D wallCollision;
    private bool isTouchingWall;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isTouchingWall = false;
        wallCollision = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ReturnWallDetection()
    {
        return isTouchingWall;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platforms") || collision.CompareTag("Level"))
        {

            isTouchingWall = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platforms") || collision.CompareTag("Level"))
        {
            isTouchingWall = false;
            
        }
    }
}
