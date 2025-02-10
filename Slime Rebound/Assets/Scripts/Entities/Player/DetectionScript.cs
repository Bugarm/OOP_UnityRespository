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
    private bool touchingWall;

    [Header("Triggers")]
    public BoxCollider2D wallTrigCol;
    public BoxCollider2D floorTrigCol;
    public BoxCollider2D topTriggerCol;
    public BoxCollider2D fakeWallTriggerCol;

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
        if (collision.CompareTag("Level") || collision.CompareTag("SwitchDoor") || collision.CompareTag("Platforms"))
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
        if (collision.CompareTag("Box"))
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

        //

        if (collision.IsTouching(fakeWallTriggerCol))
        {
            if (collision.CompareTag("Level") )
            {
                touchingWall = true;
            }
            else
            {
                touchingWall = false;
            }

            if (collision.CompareTag("FakeWall") && touchingWall == false)
            {
                touchingWall = false;
                PlayerState.IsFakeWallAllowed = true;
            }
            Debug.Log(touchingWall);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //Debug.Log(collision.IsTouchingLayers(LayerMask.NameToLayer("Level")));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Level") || collision.CompareTag("SwitchDoor")) || collision.CompareTag("Box") && DetectionScript.Instance.IsDestroyed() == false)
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

        if (collision.CompareTag("Platforms"))
        {
            if (!collision.IsTouching(floorTrigCol))
            {

                PlayerState.IsTouchingPlatform = false;
            }

            if (!collision.IsTouching(wallTrigCol))
            {
                PlayerState.IsTouchingWall = false;
            }
        }

        if(collision.CompareTag("FakeWall"))
        { 
            if (!collision.IsTouching(fakeWallTriggerCol) )
            {
                PlayerState.IsFakeWallAllowed = false;
            }
        }
       

        //Debug.Log(collision.IsTouchingLayers(LayerMask.NameToLayer("Level")));
    }
    

}
