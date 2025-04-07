using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraFollow : Singleton<CameraFollow>
{
    private GameObject cam;

    private bool isActiveOnce;

    [Header("X Point")]
    public float point1X;
    public float point2X;

    [Header("Y Point")]
    public float point1Y;
    public float point2Y;

    private Vector3 point1Pos;
    private Vector3 point2Pos;

    private Player player;
    private Vector3 savePlayerPos;
    private Vector3 velocity = Vector3.zero;
    private Vector3 offset = new Vector3(0f,0.5f,-10f);

    float camStart1X;
    float camStart2X;

    float camStart1Y;
    float camStart2Y;

    private float smoothTime;

    protected override void Awake()
    {
        base.Awake();
        cam = this.gameObject;


    }

    // Start is called before the first frame update
    void Start()
    {

        smoothTime = 0.25f;
        point1Pos = new Vector3(cam.transform.position.x + point1X, cam.transform.position.y, -10);
        point2Pos = new Vector3(cam.transform.position.x + point2X, cam.transform.position.y, -10);
        
        isActiveOnce = true;

        camStart1X = cam.transform.position.x + point1X;
        camStart2X = cam.transform.position.x + point2X;

        camStart1Y = cam.transform.position.y + point1Y;
        camStart2Y = cam.transform.position.y + point2Y;

        
        
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<Player>();

        if(player != null)
        { 
            savePlayerPos = player.transform.position;

            if ((transform.position.x > camStart1X && transform.position.x < camStart2X) && (transform.position.y > camStart1Y && transform.position.y < camStart2Y))
            {
                transform.position = Vector3.SmoothDamp(transform.position, savePlayerPos + offset, ref velocity, smoothTime);
            }
            else // when players moves to the mid of the cam
            {
                //X
                if (Mathf.RoundToInt(player.GetComponent<Rigidbody2D>().position.x) == Mathf.RoundToInt(transform.position.x)) // Move cam back to player
                {
                    float valOffsetX = transform.position.x > 0 ? -5.25f : 5.25f;
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x + valOffsetX, transform.position.y, -10f), ref velocity, smoothTime);
                }
                //Y
                if (Mathf.RoundToInt(player.GetComponent<Rigidbody2D>().position.y) == Mathf.RoundToInt(transform.position.y)) // Move cam back to player
                {
                    float valOffsetY = transform.position.y > 0 ? -1.25f : 1.25f;
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y + valOffsetY, -10f), ref velocity, smoothTime);
                }
            }
            
        }
    }

    private void OnDrawGizmos()
    {

        if(isActiveOnce == false)
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + point1X, transform.position.y, -10), 0.5f);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x + point2X, transform.position.y, -10), 0.5f);

            Gizmos.DrawWireSphere(new Vector3(transform.position.x , transform.position.y + point1Y, -10), 0.5f);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x , transform.position.y + point2Y, -10), 0.5f);
        }
      
    }

}
