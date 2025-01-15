using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] GameObject bullet;
    public int fireRate;

    //
    protected GameObject enemy;
    protected Rigidbody2D rb;
    protected SpriteRenderer enemySr;

    // Enemy Simple AI
    private List<GameObject> setPointList;

    private GameObject pointGroup;

    // Saves the 
    private Vector3 moveToPoint;
    private Transform nextPos;
    private int moveToIndex;

    private BoxCollider2D myBoxcoll;
    private CircleCollider2D playerRange;
    private GameObject shootPointObj;

    private Coroutine bulletRoutine;

    protected override void Awake()
    {
        base.Awake();

        enemy = this.gameObject;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        enemySr = enemy.GetComponent<SpriteRenderer>();

        setPointList = new List<GameObject>();

        // Gizmo Setup //


        myBoxcoll = enemy.GetComponent<BoxCollider2D>();
        shootPointObj = transform.GetChild(1).gameObject.GetComponent<GameObject>();
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

        moveToPoint = pointers[0];

        // Flips Enemy
        if (Mathf.Sign(enemy.transform.position.x - moveToPoint.x) == -1)
        {
            enemySr.flipX = false;
        }
        else
        {
            enemySr.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FollowPoints();
        
    }

    void ShootPlayer()
    { 
        if(bulletRoutine == null)
        {
            if (bullet.name == "NormalBullet")
            {
                bulletRoutine = StartCoroutine(NormalBullet());
            }
            else if (bullet.name == "HomingBullet")
            {
                bulletRoutine = StartCoroutine(HomingBullet());
            }
        }
        
    }

    IEnumerator HomingBullet()
    {
        // Spawn bullet from pool
        yield return new WaitForSeconds(fireRate);
    }

    IEnumerator NormalBullet()
    {
        // Spawn bullet from pool
        yield return new WaitForSeconds(fireRate);
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

    void FollowPoints()
    {
        if (enemy.transform.position == moveToPoint)
        {
            moveToIndex++;
            // A check to make sure it won't go higher than the length
            if (moveToIndex >= pointers.Count)
            {
                moveToIndex = 0;
            }
            moveToPoint = pointers[moveToIndex];

            // Flips The enemy based on direction
            if(Mathf.Sign(enemy.transform.position.x - moveToPoint.x) == -1)
            {
                enemySr.flipX = false;
            }
            else
            {   
                enemySr.flipX = true;
            }
        }
        else
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, moveToPoint, speed * Time.deltaTime);
            //Debug.Log(Mathf.Sign(enemy.transform.position.x - moveToPoint.x));
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

        Gizmos.DrawWireCube(transform.GetChild(0).gameObject.transform.position, new Vector3(1,0.3f,0));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (playerRange.IsTouching(collision))
        //{
        //    ShootPlayer();
        //}
        
    }

}
