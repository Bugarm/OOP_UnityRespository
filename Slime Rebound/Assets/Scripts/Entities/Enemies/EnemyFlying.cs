using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    protected Rigidbody2D rb;
    protected SpriteRenderer enemySr;
    private bool setupOnce = true;

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

        clampRotationLow = Quaternion.Euler(0, 0, 0f);
        clampRotationHigh = Quaternion.Euler(0, 0, +360f);

        // stops rotation
        rb.freezeRotation = true;

        rb.gravityScale = 0;

        moveToPoint = new Vector3(startPosX + pointers[0].x, startPosY + pointers[0].y, 0);

        // Flips Enemy
        if (Mathf.Sign(enemy.transform.position.x - moveToPoint.x) == -1)
        {
            enemySr.flipX = false;
        }
        else
        {
            enemySr.flipX = true;
        }

        setupOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (disableAI == false)
        {
            FollowPoints();

            if (seesPlayer == true)
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
            moveToPoint = new Vector3(startPosX + pointers[moveToIndex].x, startPosY + pointers[moveToIndex].y, 0);

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

        shootPointObj.transform.rotation = Quaternion.Slerp(shootPointObj.transform.rotation, newrotation, Time.deltaTime * 3);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            if(collision.IsTouching(deathCollider))
            { 
             StartCoroutine(EnemyDead());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (disableAI == false)
        {
            if (enemy.activeInHierarchy == true && collision.CompareTag("Player"))
            {

                seesPlayer = true;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (disableAI == false)
        {
            if (enemy.activeInHierarchy == true && collision.CompareTag("Player"))
            {

                seesPlayer = false;

            }
        }
    }

    private void OnDrawGizmos()
    {
        GameObject enemy = this.gameObject;
        Vector3 oldPoint = this.gameObject.transform.position;
        Vector3 newPoint;

        foreach (Vector3 point in pointers)
        {
            if (setupOnce == true) // This will follow the current pos
            {
                newPoint = new Vector3(enemy.transform.position.x + point.x, enemy.transform.position.y + point.y, 0);
            }
            else // this will only follow the starting position
            {
                newPoint = new Vector3(startPosX + point.x, startPosY + point.y, 0);
            }

            // Draw //
            Gizmos.DrawWireSphere(newPoint, 0.5f);

            Gizmos.DrawLine(oldPoint, newPoint);
            
            oldPoint = newPoint;
        }

        Gizmos.DrawWireCube(transform.GetChild(0).gameObject.transform.position, new Vector3(1,0.3f,0));
    }

    

}
