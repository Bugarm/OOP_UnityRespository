using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : Default_Entity
{
    [Header("Group Obj")]
    // Settings
    [SerializeField] private GameObject pointerGroupObj;

    [Header("Pointers")]
    //Points
    public float point1OffsetX;
    public float point2OffsetX;
    // Pointer Y val
    public float setPointY;
    // set up
    [Header("Settings")]
    public bool isStartRight;
    public int speed;

    //
    protected GameObject enemy;
    protected Rigidbody2D rb;
    protected SpriteRenderer enemySr;

    // Enemy Simple AI
    private GameObject set_point1;
    private GameObject set_point2;

    private GameObject pointGroup;

    // Saves the 
    private Transform curPoint;

    private bool setupOnce = true;

    private float startPosX;
    private float startPosY;

    private BoxCollider2D myBoxcoll;

    protected override void Awake()
    {
        base.Awake();

        enemy = this.gameObject;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        enemySr = enemy.GetComponent<SpriteRenderer>();

        // Gizmo Setup //
        setupOnce = false;
        startPosX = enemy.transform.position.x;
        startPosY = enemy.transform.position.y;

        myBoxcoll = enemy.GetComponent<BoxCollider2D>();
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        PointerCreation();

        //This will offset the pointer pos depends on the set position added to the starting enemy pos
        set_point1.transform.position = new Vector3(enemy.transform.position.x + point1OffsetX, setPointY, 0);
        set_point2.transform.position = new Vector3(enemy.transform.position.x + point2OffsetX, setPointY, 0);

        // stops rotation
        rb.freezeRotation = true;

        rb.gravityScale = 0;

        DirectionStart();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPoints();
    }

    private void DirectionStart()
    {
        curPoint = isStartRight ? set_point1.transform : set_point2.transform;
    }

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


    public void FollowPoints()
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

    private void YCordToggle()
    {
        if (setupOnce == true)
        {
            setPointY = this.gameObject.transform.position.y;
        }
        else
        {
            setPointY = startPosY;
        }
    }

    private void OnDrawGizmos()
    {
        YCordToggle();

        Vector3 point1Giz;
        Vector3 point2Giz;

        if (setupOnce == true) // This will follow the current pos
        {
            point1Giz = new Vector3(this.gameObject.transform.position.x + point1OffsetX, setPointY, 0);
            point2Giz = new Vector3(this.gameObject.transform.position.x + point2OffsetX, setPointY, 0);
        }
        else // this will only follow the starting position
        {
            point1Giz = new Vector3(startPosX + point1OffsetX, setPointY, 0);
            point2Giz = new Vector3(startPosX + point2OffsetX, setPointY, 0);
        }

        // Draw //
        Gizmos.DrawWireSphere(point1Giz, 0.5f);
        Gizmos.DrawWireSphere(point2Giz, 0.5f);
        Gizmos.DrawLine(point1Giz, point2Giz);
    }

}
