using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Drawing;
using Unity.VisualScripting;

public class NiceNPC : MonoBehaviour
{
    // Settings
    private GameObject pointerGroupObj;

    [Header("Pointers")]
    //Points
    public float point1OffsetX;
    public float point2OffsetX;

    [Header("FreeRoam Mode")]
    public bool freeRoamMode;

    // set up
    [Header("Settings")]
    public int gravityPower;
    public float jumpForce;
    public CircleCollider2D deathCollider;
    //
    private Rigidbody2D rb;
    private SpriteRenderer enemySr;
    private GameObject npc;
    private Animator anim;

    // Enemy Simple AI
    private GameObject set_point1;
    private GameObject set_point2;

    private GameObject pointGroup;

    private float yOffset = -0.5f;
    private float curJumpForce = 0;

    private Transform curPoint;

    private bool outOfRange = false;
    // Others
    private bool setupOnce = true;
    private bool isOnGround = false;
    private bool directionRoam = false;
    private bool disableAI = false;

    private float startPosX;
    private float startPosY;

    // Triggers
    public CapsuleCollider2D wallDectection;
    public BoxCollider2D pitDectection;
    public BoxCollider2D jumpDectection;

    private Coroutine flipRoutine;


    public void PrintOut()
    {
        print("AA");
    }

    void Awake()
    {
        // Set up //
        npc = this.gameObject;

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        enemySr = npc.GetComponent<SpriteRenderer>();

        pointerGroupObj = GameObject.Find("EnemyPointsGroup");

        // Gizmo Setup //
        setupOnce = false;

        anim = this.gameObject.GetComponentInChildren<Animator>();

        if (freeRoamMode == false)
        {
            // Create
            PointerCreation();
        }

        startPosX = npc.transform.position.x;
        startPosY = npc.transform.position.y;

        //Debug.Log(wallDectection);
    }

    // Start is called before the first frame update
    void Start()
    {
        // stops rotation
        rb.freezeRotation = true;

        //Gravity
        rb.gravityScale = gravityPower;

        if (freeRoamMode == false)
        {
            
            //This will offset the pointer pos depends on the set position added to the starting enemy pos
            set_point1.transform.position = new Vector3(npc.transform.position.x + point1OffsetX, this.gameObject.transform.position.y + yOffset, 0);
            set_point2.transform.position = new Vector3(npc.transform.position.x + point2OffsetX, this.gameObject.transform.position.y + yOffset, 0);

            // Decides what direction to start // 
            DirectionStart();
  
            curPoint = set_point1.transform;

            outOfRange = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (disableAI == false && outOfRange == false)
        {
            if (freeRoamMode == false)
            {
                FollowPoints();
            }
            else if (freeRoamMode == true)
            {
                FreeRoam();
            }
        }
        else if(disableAI == false)
        {
            rb.velocity = Vector3.zero;
        }

    }

    // Pointer Mode Functions //
    private void PointerCreation()
    {
        // Create Pointers //
        set_point1 = new GameObject(npc.name.ToString() + " point1");
        set_point2 = new GameObject(npc.name.ToString() + " point2");

        pointGroup = new GameObject(npc.name.ToString() + " Group");

        // Parenting //
        pointGroup.transform.SetParent(pointerGroupObj.transform);

        set_point1.transform.SetParent(pointGroup.transform);
        set_point2.transform.SetParent(pointGroup.transform);
    }

    // This Function will decide what direction will move at first //
    private void DirectionStart()
    {
        npc.transform.localScale =
            point1OffsetX < 0 ? new Vector2((Mathf.Sign(rb.velocity.x)), npc.transform.localScale.y) : new Vector2(-(Mathf.Sign(rb.velocity.x)), npc.transform.localScale.y);
    }

    // Follows the Points //
    private void FollowPoints()
    {
        // Only Moves when is Touches Ground or There's no Gravity

        if (isOnGround == true)
        {
            if (curPoint == set_point1.transform)
            {
                // This makes sure it moves to the right direction by checking the point neg or pos //
                rb.velocity = point1OffsetX < 0 ? new Vector2(-5 * (Time.deltaTime * 54), 0) : new Vector2(5 * (Time.deltaTime * 54), 0);
            }
            else
            {
                // This makes sure it moves to the right direction by checking the point neg or pos//
                rb.velocity = point2OffsetX < 0 ? new Vector2(-5 * (Time.deltaTime * 54), 0) : new Vector2(5 * (Time.deltaTime * 54), 0);
            }
        }

        // Switch Point
        if (Vector2.Distance(transform.position, curPoint.position) < 1.4f && curPoint == set_point1.transform)
        {
            PointSwitch(set_point2.transform);
        }

        if (Vector2.Distance(transform.position, curPoint.position) < 1.4 && curPoint == set_point2.transform)
        {
            PointSwitch(set_point1.transform);
        }

        //Debug.Log(enemy.name + " " + Vector2.Distance(transform.position, curPoint.position));
        //Debug.Log(isOnGround);
    }

    private void PointSwitch(Transform point)
    {
        curPoint = point;
        npc.transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), npc.transform.localScale.y);
    }

    private IEnumerator TurnFunc()
    {
        // Flips GameObject not just Sprite
        npc.transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), npc.transform.localScale.y);

        if (freeRoamMode == true)
        {
            // FreeRoam Mode //
            directionRoam = !directionRoam;
        }
        else
        {
            // Pointer Mode //
            if (curPoint == set_point1.transform)
            {
                curPoint = set_point2.transform;
            }
            else
            {
                curPoint = set_point1.transform;
            }
        }
        yield return new WaitForSeconds(1);
        flipRoutine = null;
    }

    // FreeRoam Mode Functions //
    private void FreeRoam()
    {
                                                // Right                                  //Left
        rb.velocity = directionRoam == true ? new Vector2(2 * (Time.deltaTime * 54), curJumpForce) : new Vector2(-2 * (Time.deltaTime * 54), curJumpForce);
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
            if (npc.activeInHierarchy == true && collision.CompareTag("Level") && collision.IsTouching(wallDectection))
            {
                
                if (!jumpDectection.IsTouching(collision))
                {
                    // Jumping Force
                    curJumpForce = jumpForce;
                }
            
            }
            else if (npc.activeInHierarchy == true && !wallDectection.IsTouching(collision))
            {
                curJumpForce = 0;
            }
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (disableAI == false && outOfRange == false && npc.activeInHierarchy == true)
        {
            if (npc.activeInHierarchy == true && (collision.CompareTag("Level") || collision.CompareTag("Platforms") || collision.CompareTag("FloorBreakable")) && collision.IsTouching(wallDectection) && collision.IsTouching(jumpDectection))
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
