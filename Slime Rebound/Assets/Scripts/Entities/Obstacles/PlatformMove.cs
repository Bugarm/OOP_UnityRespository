using System.Collections;
using System.Collections.Generic;
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

    private float distancePoint;

    private bool setupOnce = true;

    private void Awake()
    {
        platform = this.gameObject;
        rb = GetComponent<Rigidbody2D>();

        startPosX = platform.transform.position.x;
        startPosY = platform.transform.position.y;

        if(pointerGroupObj == null )
        { 
            pointerGroupObj = GameObject.Find("Platform Pointers Group");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PointerCreation();

        if (direction == false)
        {
            set_point1.transform.position = new Vector3(platform.transform.position.x + point1OffsetX, platform.transform.position.y, 0);
            set_point2.transform.position = new Vector3(platform.transform.position.x + point2OffsetX, platform.transform.position.y, 0);

            distancePoint = 1.4f;
        }
        else
        {
            set_point1.transform.position = new Vector3(platform.transform.position.x, platform.transform.position.y + point1OffsetY, 0);
            set_point2.transform.position = new Vector3(platform.transform.position.x, platform.transform.position.y + point2OffsetY, 0);

            distancePoint = 2.4f;
        }

        curPoint = set_point1.transform;

        setupOnce = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPoints();
    }

    private void PointerCreation()
    {
        // Create Pointers //
        set_point1 = new GameObject(platform.name.ToString() + " point1");
        set_point2 = new GameObject(platform.name.ToString() + " point2");

        pointGroup = new GameObject(platform.name.ToString() + " Group");

        // Parenting //
        pointGroup.transform.SetParent(pointerGroupObj.transform);

        set_point1.transform.SetParent(pointGroup.transform);
        set_point2.transform.SetParent(pointGroup.transform);
    }

    private void FollowPoints()
    {
        // Only Moves when is Touches Ground or There's no Gravity


        if (curPoint == set_point1.transform)
        {
            // This makes sure it moves to the right direction by checking the point neg or pos //
            if(direction == false)
            {
                MoveDirection(speed, point1OffsetX);
            }
            else
            {
                MoveDirection(speed, point1OffsetY);
            }
            
        }
        else
        {
            // This makes sure it moves to the right direction by checking the point neg or pos//
            if (direction == false)
            {
                MoveDirection(speed, point2OffsetX);
            }
            else
            {
                MoveDirection(speed, point2OffsetY);
            }
        }
        

        // Switch Point
        if (Vector2.Distance(transform.position, curPoint.position) < distancePoint && curPoint == set_point1.transform)
        {
            PointSwitch(set_point2.transform);
        }

        if (Vector2.Distance(transform.position, curPoint.position) < distancePoint && curPoint == set_point2.transform)
        {
            PointSwitch(set_point1.transform);
        }

        //Debug.Log(platform.name + " " + Vector2.Distance(transform.position, curPoint.position));
        //Debug.Log(isOnGround);
    }

    void MoveDirection(int moveSpeed, float offsetPoint)
    {
        if (direction == false)
        {
            rb.velocity = offsetPoint < 0 ? new Vector2(-moveSpeed, 0) : new Vector2(moveSpeed, 0);
        }
        else
        {
            rb.velocity = offsetPoint < 0 ? new Vector2(0, -moveSpeed) : new Vector2(0, moveSpeed);
        }
    }

    private void PointSwitch(Transform point)
    {
        curPoint = point;

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
