using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Drawing;
using Unity.VisualScripting;

// Required
[RequireComponent(typeof(Rigidbody2D))]

public class NiceNPC : EnemyGround
{
    private Coroutine flipRoutine;

    private void OnEnable()
    {
        speed = 3;
    }

    protected override void Awake()
    {
        base.Awake();
        enemySr = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Others //
    private void OnDrawGizmos()
    {
        if (freeRoamMode == false)
        {
            Vector3 point1Giz;
            Vector3 point2Giz;

            if (setupOnce == true) // This will follow the current pos
            {
                point1Giz = new Vector3(this.gameObject.transform.position.x + point1OffsetX, this.gameObject.transform.position.y + yOffset, 0);
                point2Giz = new Vector3(this.gameObject.transform.position.x + point2OffsetX, this.gameObject.transform.position.y + yOffset, 0);
            }
            else // this will only follow the starting position
            {
                point1Giz = new Vector3(startPosX + point1OffsetX, startPosY + yOffset, 0);
                point2Giz = new Vector3(startPosX + point2OffsetX, startPosY + yOffset, 0);
            }

            // Draw //
            Gizmos.DrawWireSphere(point1Giz, 0.5f);
            Gizmos.DrawWireSphere(point2Giz, 0.5f);
            Gizmos.DrawLine(point1Giz, point2Giz);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(disableAI == false && outOfRange == false)
        { 
            isOnGround = true;

            // Wall Dectection
            if (entity.activeInHierarchy == true && collision.CompareTag("Level") && collision.IsTouching(wallDectection))
            {
                
                if (!jumpDectection.IsTouching(collision))
                {
                    // Jumping Force
                    curJumpForce = jumpForce;
                }
            
            }
            else if (entity.activeInHierarchy == true && !wallDectection.IsTouching(collision))
            {
                curJumpForce = 0;
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (disableAI == false && outOfRange == false && entity.activeInHierarchy == true)
        {
            if (entity.activeInHierarchy == true && (collision.CompareTag("Level") || collision.CompareTag("Platforms") || collision.CompareTag("FloorBreakable")) && collision.IsTouching(wallDectection) && collision.IsTouching(jumpDectection))
            {
                if (freeRoamMode == false)
                {
                    if (flipRoutine == null)
                    {
                        flipRoutine = StartCoroutine(TurnFunc());
                    }
                }
                else if (collision.IsTouching(jumpDectection))
                {
                    if (flipRoutine == null)
                    {
                        flipRoutine = StartCoroutine(TurnFunc());
                    }
                }
            }

        }

        if (disableAI == false)
        {
            if (collision.IsTouching(deathCollider))
            {
                if (collision.CompareTag("TextRange"))
                {
                    anim.SetBool("IsMove", true);
                    outOfRange = false;
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (disableAI == false && outOfRange == false)
        {
            isOnGround = false;

            // Ledge Decection
            if ((collision.CompareTag("Level") || collision.CompareTag("Platforms") || collision.CompareTag("FloorBreakable")))
            {
                 if (!collision.IsTouching(pitDectection) && !collision.IsTouching(jumpDectection) && freeRoamMode == true)
                {
                    if (flipRoutine == null)
                    {
                        flipRoutine = StartCoroutine(TurnFunc());
                    }
                }
            }
        }

        if (disableAI == false)
        {
            if (collision.CompareTag("PlayerRange"))
            {
                if (!collision.IsTouching(deathCollider))
                {
                    outOfRange = true;
                }
            }
        }

        if (collision.CompareTag("TextRange"))
        {
            anim.SetBool("IsMove", false);
            outOfRange = true;
        }

    }

    private void OnDisable()
    {
        this.gameObject.GetComponent<NiceNPC>().enabled = false;
    }


}
