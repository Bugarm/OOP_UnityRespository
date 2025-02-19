using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : Singleton<CameraFollow>
{
    private GameObject cam;

    private bool isTouching;
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

    float camStart1X;
    float camStart2X;

    float camStart1Y;
    float camStart2Y;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {

        cam = this.gameObject;
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

        if (player != null)
        { 
            savePlayerPos = player.transform.position;

            float camX = cam.transform.position.x;
            float camY = cam.transform.position.y;

            //X
            if (camX > camStart1X && camX < camStart2X)
            {
                this.transform.position = new Vector3(savePlayerPos.x, this.transform.position.y + 1, -10);
            }
            else if (player.transform.position.x > camStart1X && player.transform.position.x < camStart2X)
            {
                this.transform.position = new Vector3(cam.transform.position.x + -(Mathf.Sign(cam.transform.position.x)), savePlayerPos.y + 1, -10);
            }

            //Y
            if (camY > camStart1Y && camY < camStart2Y)
            {
                this.transform.position = new Vector3(this.transform.position.x, savePlayerPos.y + 1, -10);
            }
            else if (player.transform.position.y > camStart1Y && player.transform.position.y < camStart2Y)
            {
                this.transform.position = new Vector3(cam.transform.position.x, savePlayerPos.y + -(Mathf.Sign(cam.transform.position.x)), -10);
            }
        }

    }

    public void UpdateCam()
    {
        player = FindObjectOfType<Player>();

        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, -10);
    }

    private void OnDrawGizmos()
    {

        if(isActiveOnce == false)
        {
            Gizmos.DrawWireSphere(new Vector3(this.gameObject.transform.position.x + point1X, this.gameObject.transform.position.y, -10), 0.5f);
            Gizmos.DrawWireSphere(new Vector3(this.gameObject.transform.position.x + point2X, this.gameObject.transform.position.y, -10), 0.5f);

            Gizmos.DrawWireSphere(new Vector3(this.gameObject.transform.position.x , this.gameObject.transform.position.y + point1Y, -10), 0.5f);
            Gizmos.DrawWireSphere(new Vector3(this.gameObject.transform.position.x , this.gameObject.transform.position.y + point2Y, -10), 0.5f);
        }
      
    }

}
