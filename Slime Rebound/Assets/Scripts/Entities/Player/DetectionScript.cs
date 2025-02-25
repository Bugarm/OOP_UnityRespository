using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class DetectionScript : Singleton<DetectionScript>
{
    private Rigidbody2D playerRB;
    private Vector3 playerPos;
    private int touchingBoxes;

    [Header("Triggers")]
    public BoxCollider2D wallTrigCol;
    public BoxCollider2D floorTrigCol;
    public BoxCollider2D topTriggerCol;

    protected override void Awake()
    {
        base.Awake();

        playerRB = this.gameObject.GetComponent<Rigidbody2D>();
        playerPos = this.gameObject.transform.position;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject platform;
    public GameObject GetPlatform()
    {
        return platform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (this.gameObject.activeInHierarchy == true)
        {
            // Damaging the Player
            if (collision.IsTouching(Player.Instance.idleCollider) || collision.IsTouching(Player.Instance.crouchCollider) || collision.IsTouching(Player.Instance.bounceCollider))
            {

                if (collision.CompareTag("Enemies") || collision.CompareTag("Obsticales") || collision.CompareTag("EnemyBullet"))
                {
                    if (GameManager.Instance.damageRoutine == null)
                    {
                        GameManager.Instance.damageRoutine = StartCoroutine(GameManager.Instance.DamagePlayer());
                    }
                }
            }
        }

        // Level Collision
        if (collision.CompareTag("Level") || collision.CompareTag("SwitchDoor"))
        {
            if (collision.IsTouching(wallTrigCol))
            {
                PlayerState.IsTouchingWall = true;
            }

            if (collision.IsTouching(floorTrigCol))
            {
                PlayerState.IsTouchingGround = true;
            }

            if (collision.IsTouching(topTriggerCol))
            {
                PlayerState.IsTouchingTop = true;
            }   
        }

        // Box Collision
        if (collision.CompareTag("Box") || collision.CompareTag("FloorBreakable"))
        {
            if (collision.IsTouching(wallTrigCol) && collision.GetComponent<BoxScript>().isDestroyed == false && PlayerState.IsBounceMode == false)
            {
                PlayerState.IsTouchingWall = true;
            }

            if (collision.IsTouching(floorTrigCol) && PlayerState.IsPound == false)
            {
                PlayerState.IsTouchingGround = true;
            }

            if (collision.IsTouching(topTriggerCol) && collision.GetComponent<BoxScript>().isDestroyed == false)
            {
                PlayerState.IsTouchingTop = true;
            }
        }


        if (collision.CompareTag("OneWay"))
        {
                     
            if (collision.IsTouching(floorTrigCol))
            {
                PlayerState.IsTouchingGround = true;
            }
        }

        if (collision.CompareTag("Platforms"))
        {
            if (collision.IsTouching(floorTrigCol))
            {
                platform = collision.gameObject;
                PlayerState.IsTouchingPlatform = true;
                //Debug.Log("true");
            }

            if (collision.IsTouching(wallTrigCol))
            {
                PlayerState.IsTouchingWall = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box") || collision.CompareTag("FloorBreakable"))
        {
            if (collision.IsTouching(topTriggerCol) || collision.IsTouching(wallTrigCol))
            {
                touchingBoxes++;

                if (touchingBoxes > 0)
                {
                    PlayerState.IsDestroyedObj = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Level") ||  collision.CompareTag("Box") || collision.CompareTag("FloorBreakable") || collision.CompareTag("Platforms") || collision.CompareTag("SwitchDoor")) || collision.CompareTag("Box") || collision.CompareTag("FloorBreakable") && DetectionScript.Instance.IsDestroyed() == false)
        {
            if (!collision.IsTouching(wallTrigCol))
            {
                PlayerState.IsTouchingWall = false;
            }

            if (!collision.IsTouching(floorTrigCol))
            {
                PlayerState.IsTouchingGround = false;
            }

            if (!collision.IsTouching(topTriggerCol))
            {
                PlayerState.IsTouchingTop = false;
            }
        }

        if (collision.CompareTag("FloorBreakable") || collision.CompareTag("Box"))
        {
            if (!collision.IsTouching(topTriggerCol))
            {
                touchingBoxes--;

                if(touchingBoxes <= 0)
                {     
                    PlayerState.IsDestroyedObj = false;
                }
            }
        }

        if (collision.CompareTag("Platforms") )
        {
            if (!collision.IsTouching(floorTrigCol))
            {
                PlayerState.IsTouchingPlatform = false;
                //Debug.Log("false");
                
            }

            if (!collision.IsTouching(wallTrigCol))
            {
                PlayerState.IsTouchingWall = false;
            }
        }

        if (collision.CompareTag("OneWay"))
        {
            if (!collision.IsTouching(floorTrigCol))
            {
                PlayerState.IsTouchingGround = false;
            }
        }

        //Debug.Log(collision.IsTouchingLayers(LayerMask.NameToLayer("Level")));
    }
    

}
