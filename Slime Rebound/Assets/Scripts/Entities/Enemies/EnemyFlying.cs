using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

// Required
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]

public class EnemyFlying : Default_Entity
{
    //[Header("Group Obj")]
    //// Settings
    //[SerializeField]
    [Header("Pointers")]
    //Points
    public List<Vector2> pointers;
    
    // set up
    [Header("Settings")]
    public bool doAttack = true;
    public bool isHoming = false;
    public float fireRateDelay;

    // Colliders
    [Header("Colliders")]
    public GameObject playerRangeObj;
    public GameObject shootPointObj;

    [Header("Audio")]
    public AudioSource fireSFX;

    // set up
    private ObjectPooling bulletPool;
    
    // Enemy Simple AI
    private Vector3 moveToPoint;
    private int moveToIndex;

    private bool seesPlayer;


    private Coroutine bulletRoutine, playerLoadRoutine;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {

        rb.gravityScale = 0;

        moveToPoint = new Vector3(startPosX + pointers[0].x, startPosY + pointers[0].y, 0);

        // Flips Enemy
        if (Mathf.Sign(entity.transform.position.x - moveToPoint.x) == -1)
        {
            enemySr.flipX = false;
        }
        else
        {
            enemySr.flipX = true;
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        TryGetComponent<ObjectPooling>(out ObjectPooling hasPool);

        if (bulletPool == null && doAttack == true)
        {
            if (isHoming == true)
            {
                GameObject homing = GameObject.Find("ObjectsToPool Homing Bullet");
                if (homing != null)
                {
                    bulletPool = homing.GetComponent<ObjectPooling>();
                }
            }
            else
            {
                GameObject normal = GameObject.Find("ObjectsToPool Normal Bullet");
                if(normal != null)
                { 
                    bulletPool = normal.GetComponent<ObjectPooling>();
                }
            }

            
        }

        if (disableAI == false && outOfRange == false )
        {
                
            FollowPoints();

            if (doAttack == true)
            { 
                if (seesPlayer == true )
                {
                    // Homing Function
                    if (bulletPool.name == "ObjectsToPool Homing Bullet")
                    {
                        PointAtPlayer();
                    }
                }

                if (bulletRoutine == null)
                {
                    bulletRoutine = StartCoroutine(ShootBullet(bulletPool));
                }
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
            if (seesPlayer == true)
            { 
                // Spawn bullet from pool
                GameObject getBullet = bulletPool.GetPoolObject();

                if (getBullet != null)
                {
                    getBullet.transform.position = shootPointObj.transform.position;
                    getBullet.transform.rotation = shootPointObj.transform.rotation;

                    getBullet.SetActive(true);

                    StartCoroutine(AudioManager.Instance.PlaySFXManual(fireSFX,entity.transform.position));
                }
            }
            yield return new WaitForSeconds(fireRateDelay);
            
        }
    }

    void FollowPoints()
    {
        if (entity.transform.position == moveToPoint)
        {
            moveToIndex++;
            // A check to make sure it won't go higher than the length
            if (moveToIndex >= pointers.Count)
            {
                moveToIndex = 0;
            }
            moveToPoint = new Vector3(startPosX + pointers[moveToIndex].x, startPosY + pointers[moveToIndex].y, 0);

            // Flips The enemy based on direction
            if(Mathf.Sign(entity.transform.position.x - moveToPoint.x) == -1)
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
            entity.transform.position = Vector3.MoveTowards(entity.transform.position, moveToPoint, enemiesData.speed * Time.deltaTime);
            //Debug.Log(Mathf.Sign(enemy.transform.position.x - moveToPoint.x));
        }
    }

    private void PointAtPlayer()
    {

        Vector2 relativePos = shootPointObj.transform.position - Player.Instance.gameObject.transform.position;
        Quaternion newrotation = Quaternion.LookRotation(relativePos, Vector3.back);
        newrotation.x = 0;
        newrotation.y = 0;

        shootPointObj.transform.rotation = Quaternion.Slerp(shootPointObj.transform.rotation, newrotation, Time.deltaTime * 3);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (disableAI == false && outOfRange == false)
        {
            if (collision.CompareTag("PlayerAttack"))
            {
                if (collision.IsTouching(deathCollider))
                {
                    StartCoroutine(EnemyDead());
                }
            }
        }

        if (disableAI == false)
        {
            if (collision.CompareTag("PlayerRange"))
            {
                if (collision.IsTouching(deathCollider))
                {
                    outOfRange = false;
                }
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (disableAI == false && outOfRange == false)
        {
            if (entity.activeInHierarchy == true && collision.CompareTag("PlayerBody"))
            {
                seesPlayer = true;
                //Debug.Log("Looked");
            }
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (disableAI == false && outOfRange == false)
        {
            if (entity.activeInHierarchy == true && collision.CompareTag("PlayerBody"))
            {

                seesPlayer = false;

            }
        }

        if(disableAI == false)
        { 
            if (collision.CompareTag("PlayerRange"))
            {
                if (!collision.IsTouching(deathCollider))
                {
                    outOfRange = true;
                }
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
