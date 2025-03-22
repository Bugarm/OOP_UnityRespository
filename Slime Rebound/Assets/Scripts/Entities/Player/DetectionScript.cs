using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

// Required
[RequireComponent(typeof(Rigidbody2D))]

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

    private void Update()
    {
        if(PlayerState.IsCrouch == true ||  PlayerState.IsSlide == true)
        {
            wallTrigCol.offset = new Vector2(0.4836431f, -0.55f);
        }
        else if (PlayerState.IsBounceMode == true)
        {
            wallTrigCol.offset = new Vector2(0.72f, -0.2430587f);
        }
        else
        {
            wallTrigCol.offset = new Vector2(0.4836431f, -0.2430587f);
        }
        
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

        if ((collision.CompareTag("Level") || collision.CompareTag("Box") || collision.CompareTag("SwitchDoor")) && DetectionScript.Instance.IsDestroyed() == false)
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
                PlayerState.IsTouchingPlatform = true;
                platform = collision.gameObject;
            }

            if (collision.IsTouching(wallTrigCol))
            {
                PlayerState.IsTouchingWall = true;
                PlayerState.IsTouchingPlatformSide = true;
                platform = collision.gameObject;

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

        if(collision.CompareTag("Enemies"))
        {
           if(collision.IsTouching(Player.Instance.bounceAttackTrigger))
           {
               Player.Instance.failedBounces += 2;
           }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Level") ||  collision.CompareTag("Box") || collision.CompareTag("SwitchDoor")) && DetectionScript.Instance.IsDestroyed() == false)
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

            if (!collision.IsTouching(floorTrigCol) )
            {
                PlayerState.IsTouchingGround = false;
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
                PlayerState.IsTouchingPlatformSide = false;
                //Debug.Log("false");
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
