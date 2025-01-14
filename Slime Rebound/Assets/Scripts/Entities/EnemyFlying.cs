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
    public int speed;

    //
    protected GameObject enemy;
    protected Rigidbody2D rb;
    protected SpriteRenderer enemySr;

    // Enemy Simple AI
    private List<GameObject> setPointList;

    private GameObject pointGroup;

    // Saves the 
    private Transform moveToPoint;
    private Transform nextPos;
    private int moveToIndex;

    private BoxCollider2D myBoxcoll;

    private Coroutine flyToPoints;

    protected override void Awake()
    {
        base.Awake();

        enemy = this.gameObject;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        enemySr = enemy.GetComponent<SpriteRenderer>();

        setPointList = new List<GameObject>();

        // Gizmo Setup //


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

        //nextPos = ;
    }

    // Update is called once per frame
    void Update()
    {
        //FollowPoints();
    }

    private void PointerCreation()
    {
        GameObject setPoint;

        int pointCount = 0;
        pointGroup = new GameObject(enemy.name.ToString() + " Group");

        // Parenting //
        pointGroup.transform.SetParent(pointerGroupObj.transform);

        // Create Pointers //
        foreach (Vector2 point in pointers)
        {
            pointCount++;
            setPoint = new GameObject(enemy.name.ToString() + " Point " + pointCount);
            setPoint.transform.position = point;
            setPointList.Add(setPoint);

            setPointList[pointCount - 1].transform.SetParent(pointGroup.transform);
        }

        

        
    }

    // WIP //
    void FollowPoints()
    {
        if (enemy.transform.position == moveToPoint.position)
        {
            moveToIndex++;
            // A check to make sure it won't go higher than the length
            if (moveToIndex >= pointers.Count)
            {
                moveToIndex = 0;
            }

        }
        else
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, moveToPoint.position, speed * Time.deltaTime);
        }
    }


    private void OnDrawGizmos()
    {
        Vector3 oldPoint = this.gameObject.transform.position;

        foreach (Vector3 point in pointers)
        {
             // Draw //
            Gizmos.DrawWireSphere(point, 0.5f);

            Gizmos.DrawLine(oldPoint,point);
            
            oldPoint = point;
        }
    }

}
