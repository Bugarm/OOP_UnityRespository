using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class EnemyGround : Default_Entity
{
    protected GameObject enemy;
    protected Rigidbody2D rb;
    protected SpriteRenderer enemySr;

    // Settings
    [SerializeField] private GameObject pointerGroupObj;

    public bool isStartRight;

    // Enemy Simple AI
    private GameObject set_point1;
    private GameObject set_point2;

    private GameObject startPoint;

    private GameObject pointGroup;

    private Transform curPoint;

    public float point1_OffsetX;
    public float point2_OffsetX;

    public float setPointY;

    public int speed;

    public bool doGizPosOnce = true;

    protected override void Awake()
    {
        base.Awake();
        enemy = this.gameObject;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        enemySr = enemy.GetComponent<SpriteRenderer>();

        doGizPosOnce = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        // Create Pointers //
        set_point1 = new GameObject(enemy.name.ToString() + " point1");
        set_point2 = new GameObject(enemy.name.ToString() + " point2");

        pointGroup = new GameObject(enemy.name.ToString() + " Group");

        // Parenting //
        pointGroup.transform.SetParent(pointerGroupObj.transform);

        set_point1.transform.SetParent(pointGroup.transform);
        set_point2.transform.SetParent(pointGroup.transform);

        //This will offset the pointer pos depends on the set position added to the starting enemy pos
        set_point1.transform.position = new Vector3(enemy.transform.position.x + point1_OffsetX, setPointY, 0);
        set_point2.transform.position = new Vector3(enemy.transform.position.x + point2_OffsetX, setPointY, 0);

        rb.freezeRotation = true;

        DirectionStart();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPoints();
    }

    private void DirectionStart()
    {
        if (isStartRight == true)
        {
            curPoint = set_point1.transform;
        }
        else
        {
            curPoint = set_point2.transform;
        }
    }

    protected void FollowPoints()
    {
        if (curPoint == set_point1.transform)
        {
            rb.velocity = new Vector2(-speed, 0); // go Right
            enemySr.flipX = true;
        }
        else
        {
            rb.velocity = new Vector2(speed, 0); // go Left
            enemySr.flipX = false;
        }

        if(curPoint.name == enemy.name.ToString() + " point1")
        { 
            if (Vector2.Distance(transform.position, curPoint.position) < 1.4f && curPoint == set_point1.transform)
            {
                curPoint = set_point2.transform;
            }
        }

        if (curPoint.name == enemy.name.ToString() + " point2")
        {
            if (Vector2.Distance(transform.position, curPoint.position) < 1.4f && curPoint == set_point2.transform)
            {
                curPoint = set_point1.transform;
            }
        }

        Debug.Log(enemy.name + " " + Vector2.Distance(transform.position, curPoint.position));

    }

    private void OnDrawGizmos()
    {
        Vector3 point1Giz = new Vector3(this.gameObject.transform.position.x + point1_OffsetX, setPointY, 0);
        Vector3 point2Giz = new Vector3(this.gameObject.transform.position.x + point2_OffsetX, setPointY, 0);

        Gizmos.DrawWireSphere(point1Giz, 0.5f);
        Gizmos.DrawWireSphere(point2Giz, 0.5f);
        Gizmos.DrawLine(point1Giz, point2Giz);
    }

}
