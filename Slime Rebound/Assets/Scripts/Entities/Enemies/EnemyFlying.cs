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
    public ObjectPooling bulletPool;
    public int fireRate;

    // set up
    protected GameObject enemy;
    protected Rigidbody2D rb;
    protected SpriteRenderer enemySr;
    private GameObject player;

    // Enemy Simple AI
    private List<GameObject> setPointList;

    private GameObject pointGroup;

    private Vector3 moveToPoint;
    private Transform nextPos;
    private int moveToIndex;

    private Quaternion clampRotationLow, clampRotationHigh;

    private Coroutine bulletRoutine;

    private bool seesPlayer;

    // Colliders
    private BoxCollider2D myBoxcoll;

    [Header("Colliders")]
    public GameObject playerRangeObj;
    private CircleCollider2D playerRangeTrigger;

    public GameObject shootPointObj;
    private BoxCollider2D shootPointTrigger;

    protected override void Awake()
    {
        base.Awake();

        enemy = this.gameObject;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        enemySr = enemy.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        setPointList = new List<GameObject>();

        myBoxcoll = enemy.GetComponent<BoxCollider2D>();
        playerRangeTrigger = playerRangeObj.GetComponent<CircleCollider2D>();
        shootPointTrigger = shootPointObj.GetComponent<BoxCollider2D>();   

    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        PointerCreation();

        clampRotationLow = Quaternion.Euler(0, 0, -70f);
        clampRotationHigh = Quaternion.Euler(0, 0, +70f);

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

        if(seesPlayer == true)
        {
            // Homing Function
            if (bulletPool.name == "ObjectsToPool Homing Bullet")
            {
                PointAtPlayer();
            }

            if (bulletRoutine == null)
            {
                bulletRoutine = StartCoroutine(ShootBullet(bulletPool));
            }
        }
        else 
        {
            if (bulletRoutine != null)
            {
                StopCoroutine(bulletRoutine);
                bulletRoutine = null;
            }
        }

        
    }

    IEnumerator ShootBullet(ObjectPooling bulletPool)
    {
        while (true)
        {
            // Spawn bullet from pool
            GameObject getBullet = bulletPool.GetPoolObject();

            getBullet.transform.position = shootPointObj.transform.position;
            getBullet.transform.rotation = shootPointObj.transform.rotation;
            
            getBullet.SetActive(true);
            yield return new WaitForSeconds(fireRate);
        }
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

    private void PointAtPlayer()
    {

        Vector3 relativePos = shootPointObj.transform.position - player.transform.position;
        Quaternion newrotation = Quaternion.LookRotation(relativePos, Vector3.back);
        newrotation.x = 0;
        newrotation.y = 0;
        newrotation.z = Mathf.Clamp(newrotation.z, clampRotationLow.z, clampRotationHigh.z);
        newrotation.w = Mathf.Clamp(newrotation.w, clampRotationLow.w, clampRotationHigh.w);
        shootPointObj.transform.rotation = Quaternion.Slerp(shootPointObj.transform.rotation, newrotation, Time.deltaTime * 3);

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enemy.activeInHierarchy == true && collision.CompareTag("Player"))
        {
            if (playerRangeTrigger.IsTouching(collision))
            {
                seesPlayer = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemy.activeInHierarchy == true && collision.CompareTag("Player"))
        {
            if (!playerRangeTrigger.IsTouching(collision))
            {
                seesPlayer = false;
            }
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

    

}
