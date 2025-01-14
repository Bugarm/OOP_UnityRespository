using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Drawing;
using Unity.VisualScripting;

public class EnemyGround : Default_Entity
{
    [Header("Group Obj")]
    // Settings
    [SerializeField] private GameObject pointerGroupObj;

    [Header("Pointers")]
    //Points
    public float point1OffsetX;
    public float point2OffsetX;

    [Header("FreeRoam Mode")]
    public bool freeRoamMode;

    // set up
    [Header("Settings")]
    public int speed;
    public int gravityPower;
    public float jumpForce;

    //
    protected GameObject enemy;
    protected Rigidbody2D rb;
    protected SpriteRenderer enemySr;

    // Enemy Simple AI
    private GameObject set_point1;
    private GameObject set_point2;

    private GameObject pointGroup;

    private float startPosX;
    private float startPosY;
    private float yOffset = -0.5f;
    private float curJumpForce = 0;

    private Transform curPoint;

    // Others
    private bool setupOnce = true;
    private bool isOnGround = false;
    private bool directionRoam = false;
   
    // Triggers
    private BoxCollider2D groundDectection;
    private CapsuleCollider2D wallDectection;
    private BoxCollider2D pitDectection;
    private BoxCollider2D jumpDectection;

    protected override void Awake()
    {
        base.Awake();
        // Set up //
        enemy = this.gameObject;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        enemySr = enemy.GetComponent<SpriteRenderer>();

        // Gizmo Setup //
        setupOnce = false;
        startPosX = enemy.transform.position.x;
        startPosY = enemy.transform.position.y;

        groundDectection = enemy.GetComponent<BoxCollider2D>();
        wallDectection = transform.GetChild(0).gameObject.GetComponent<CapsuleCollider2D>();
        pitDectection = transform.GetChild(1).gameObject.GetComponent<BoxCollider2D>();
        jumpDectection = transform.GetChild(2).gameObject.GetComponent<BoxCollider2D>();

        if(freeRoamMode == true)
        {
            Destroy(groundDectection);
        }
        else
        {
            Destroy(pitDectection);
        }

        //Debug.Log(wallDectection);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        // stops rotation
        rb.freezeRotation = true;

        //Gravity
        rb.gravityScale = gravityPower;

        if (freeRoamMode == false)
        {
            // Create
            PointerCreation();

            //This will offset the pointer pos depends on the set position added to the starting enemy pos
            set_point1.transform.position = new Vector3(enemy.transform.position.x + point1OffsetX, this.gameObject.transform.position.y + yOffset, 0);
            set_point2.transform.position = new Vector3(enemy.transform.position.x + point2OffsetX, this.gameObject.transform.position.y + yOffset, 0);

            // Decides what direction to start // 
            DirectionStart();
  
            curPoint = set_point1.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {        
        if(freeRoamMode == false)
        {
            FollowPoints();    
        }
        else if (freeRoamMode == true)
        {
            FreeRoam();
        }
        
    }

    // Pointer Mode Functions //
    private void PointerCreation()
    {
        // Create Pointers //
        set_point1 = new GameObject(enemy.name.ToString() + " point1");
        set_point2 = new GameObject(enemy.name.ToString() + " point2");

        pointGroup = new GameObject(enemy.name.ToString() + " Group");

        // Parenting //
        pointGroup.transform.SetParent(pointerGroupObj.transform);

        set_point1.transform.SetParent(pointGroup.transform);
        set_point2.transform.SetParent(pointGroup.transform);
    }

    // This Function will decide what direction will move at first //
    private void DirectionStart()
    {
        enemy.transform.localScale =
            point1OffsetX < 0 ? new Vector2((Mathf.Sign(rb.velocity.x)), enemy.transform.localScale.y) : new Vector2(-(Mathf.Sign(rb.velocity.x)), enemy.transform.localScale.y);
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
                rb.velocity = point1OffsetX < 0 ? new Vector2(-speed, 0) : new Vector2(speed, 0);
            }
            else
            {
                // This makes sure it moves to the right direction by checking the point neg or pos//
                rb.velocity = point2OffsetX < 0 ? new Vector2(-speed, 0) : new Vector2(speed, 0);
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
        enemy.transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), enemy.transform.localScale.y);
    }

    private void TurnFunc()
    {
        // Flips GameObject not just Sprite
        enemy.transform.localScale = new Vector2((Mathf.Sign(rb.velocity.x)), enemy.transform.localScale.y);

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
    }

    // FreeRoam Mode Functions //
    private void FreeRoam()
    {
                                                // Right                                  //Left
        rb.velocity = directionRoam == true ? new Vector2(speed, curJumpForce) : new Vector2(-speed, curJumpForce);
    }

    // Others //
    private void OnDrawGizmos()
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        isOnGround = true;

        // Wall Dectection
        if (enemy.activeInHierarchy == true && collision.CompareTag("Level") && wallDectection.IsTouching(collision))
        {
            if(freeRoamMode == false)
            {
                TurnFunc();
            }
            else
            {
                if (!jumpDectection.IsTouching(collision))
                {
                    // Jumping Force
                    curJumpForce = jumpForce;
                }
                
            }
            
        }
        else if (enemy.activeInHierarchy == true && !wallDectection.IsTouching(collision))
        {
            curJumpForce = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemy.activeInHierarchy == true && collision.CompareTag("Level") && wallDectection.IsTouching(collision) && jumpDectection.IsTouching(collision))
        {
            TurnFunc();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isOnGround = false;

        // Ledge Decection
        if (enemy.activeInHierarchy == true)
        { 
            if (freeRoamMode == false)
            {
                if (!groundDectection.IsTouching(collision))
                {
                    TurnFunc();
                }
            }
            else if (freeRoamMode == true)
            {
                if (!pitDectection.IsTouching(collision))
                {
                    TurnFunc();
                }
            }
        }

    }
}
