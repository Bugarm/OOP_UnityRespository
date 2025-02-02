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

    [Header("Triggers")]
    public GameObject wallTrigger;
    public GameObject floorTrigger;
    public GameObject topTrigger;

    private BoxCollider2D wallTrigCol;
    private BoxCollider2D floorTrigCol;
    private BoxCollider2D topTriggerCol;

    protected override void Awake()
    {
        playerRB = this.gameObject.GetComponent<Rigidbody2D>();
        playerPos = this.gameObject.transform.position;

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        wallTrigCol = wallTrigger.GetComponent<BoxCollider2D>();
        floorTrigCol = floorTrigger.GetComponent<BoxCollider2D>();
        topTriggerCol = topTrigger.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Level") || collision.CompareTag("SwitchDoor"))
        {
            if(collision.IsTouching(wallTrigCol) )
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

        if (collision.CompareTag("Platforms"))
        {
            if (collision.IsTouching(floorTrigCol))
            {
                PlayerState.IsTouchingPlatform = true;
            }

            if (collision.IsTouching(wallTrigCol))
            {
                PlayerState.IsTouchingWall = true;
            }
        }

        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemies"))
        {
            if (collision.IsTouching(Player.Instance.idleTrigger) || collision.IsTouching(Player.Instance.crouchTrigger) || collision.IsTouching(Player.Instance.bounceTrigger))
            {
                if (GameManager.Instance.damageRoutine == null)
                {
                    GameManager.Instance.damageRoutine = StartCoroutine(GameManager.Instance.DamagePlayer());
                }
            }
        }

        if (collision.CompareTag("Obsticales") || collision.CompareTag("EnemyBullet"))
        {
            if (GameManager.Instance.damageRoutine == null)
            {
                GameManager.Instance.damageRoutine = StartCoroutine(GameManager.Instance.DamagePlayer());
            }
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Level") || collision.CompareTag("SwitchDoor"))
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

    }
}
