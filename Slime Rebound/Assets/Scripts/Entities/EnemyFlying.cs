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
    public List<Vector2> pointers;
    
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
        
    }


    private void OnDrawGizmos()
    {
        Vector3 point1Giz;
        Vector3 point2Giz;

        if (setupOnce == true) // This will follow the current pos
        {
          
        }
        else // this will only follow the starting position
        {
            
        }

        // Draw //
        //Gizmos.DrawWireSphere(point1Giz, 0.5f);
        //Gizmos.DrawWireSphere(point2Giz, 0.5f);
        //Gizmos.DrawLine(point1Giz, point2Giz);
    }

}
