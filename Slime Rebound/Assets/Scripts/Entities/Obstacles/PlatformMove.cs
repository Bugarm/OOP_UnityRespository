using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [Header("Group Obj")]
    // Settings
    private GameObject pointerGroupObj;

    [Header("Pointers")]
    //Points X
    public float point1OffsetX;
    public float point2OffsetX;
    //Points Y
    public float point1OffsetY;
    public float point2OffsetY;

    // set up
    [Header("Settings")]
    public int speed;
    public bool direction;
    public bool randomizeSpeed;

    [Header("Random Range")]
    public int randomSpeedMin;
    public int randomSpeedMax;

    //Pos
    private float startPosX;
    private float startPosY;

    // Platform Move
    private GameObject set_point1;
    private GameObject set_point2;

    private GameObject pointGroup;

    private GameObject platform;
    private Rigidbody2D rb;
    private Transform curPoint;

    private Vector3 moveToPoint;
    private int moveToIndex;

    List<float> pointsXList = new List<float>();
    List<float> pointsYList = new List<float>();

    float dirY;
    float dirX;

    private float distancePoint;

    private bool setupOnce = true;

    private void Awake()
    {
        platform = this.gameObject;
        rb = GetComponent<Rigidbody2D>();

        startPosX = platform.transform.position.x;
        startPosY = platform.transform.position.y;
    }

    // Start is called before the first frame update
    void Start()
    {
        //PointerCreation();

        pointsXList.Add(startPosX + point1OffsetX);
        pointsXList.Add(startPosX + point2OffsetX);

        pointsYList.Add(startPosY + point1OffsetY);
        pointsYList.Add(startPosY + point2OffsetY);

        if (direction == false)
        {
            dirY = platform.transform.position.y;

            dirX = pointsXList[moveToIndex];
        }
        else
        {
            dirX = platform.transform.position.x;

            dirY = pointsYList[moveToIndex];
        }

        moveToPoint = new Vector3(dirX, dirY, 0);

        setupOnce = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPoints();
    }

    private void Update()
    {
        PointSwitch();
    }

    private void FollowPoints()
    {

        this.gameObject.transform.position = Vector3.MoveTowards(this.transform.position, moveToPoint, speed * Time.deltaTime);

        //Debug.Log(platform.name + " " + Vector2.Distance(transform.position, curPoint.position));
        //Debug.Log(isOnGround);
    }

    private void PointSwitch()
    {
        if (this.gameObject.transform.position == moveToPoint)
        {
            moveToIndex++;
            // A check to make sure it won't go higher than the length
            if (moveToIndex >= 2)
            {
                moveToIndex = 0;
            }

            if(direction == false)
            {
                dirY = platform.transform.position.y;
                
                dirX = pointsXList[moveToIndex];
            }
            else
            {
                dirX = platform.transform.position.x;

                dirY = pointsYList[moveToIndex];
            }

            moveToPoint = new Vector3(dirX, dirY,0);

            if (randomizeSpeed == true)
            {
                if (randomSpeedMin == randomSpeedMax || (randomSpeedMin > randomSpeedMax))
                {
                    Debug.LogException(new Exception("Both Random Values are either the same or the values are ordered incorrectly"));
                }
                else
                {
                    speed = UnityEngine.Random.Range(randomSpeedMin, randomSpeedMax);
                }
            }

        }

    }


    private void OnDrawGizmos()
    {
        Vector3 point1Giz;
        Vector3 point2Giz;

        if (setupOnce == true) // This will follow the current pos
        {
            if (direction == false)
            {
                point1Giz = new Vector3(this.gameObject.transform.position.x + point1OffsetX, this.gameObject.transform.position.y, 0);
                point2Giz = new Vector3(this.gameObject.transform.position.x + point2OffsetX, this.gameObject.transform.position.y, 0);
            }
            else
            {
                point1Giz = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + point1OffsetY, 0);
                point2Giz = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + point2OffsetY, 0);
            }
        }
        else // this will only follow the starting position
        {
            if (direction == false)
            {
                point1Giz = new Vector3(startPosX + point1OffsetX, startPosY, 0);
                point2Giz = new Vector3(startPosX + point2OffsetX, startPosY, 0);
            }
            else
            {
                point1Giz = new Vector3(startPosX, startPosY + point1OffsetY, 0);
                point2Giz = new Vector3(startPosX, startPosY + point2OffsetY, 0);
            }
        }

        // Draw //
        Gizmos.DrawWireSphere(point1Giz, 0.5f);
        Gizmos.DrawWireSphere(point2Giz, 0.5f);
        Gizmos.DrawLine(point1Giz, point2Giz);
    }
}
