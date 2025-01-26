using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        
        cam = this.gameObject;
        point1Pos = new Vector3(cam.transform.position.x + point1X, cam.transform.position.y, -10);
        point2Pos = new Vector3(cam.transform.position.x + point2X, cam.transform.position.y, -10);

        
        player = FindObjectOfType<Player>();

        savePlayerPos = player.ReturnInstance().transform.position;
        this.transform.position = new Vector3(savePlayerPos.x, savePlayerPos.y + 1, -10);
        isActiveOnce = true;


    }

    // Update is called once per frame
    void Update()
    {
        
        savePlayerPos = player.ReturnInstance().transform.position;

        //X
        if (cam.transform.position.x > point1X && cam.transform.position.x < point2X)
        {
            this.transform.position = new Vector3(savePlayerPos.x, savePlayerPos.y + 1, -10);
        }
        else if (player.transform.position.x > point1X && player.transform.position.x < point2X)
        {
            this.transform.position = new Vector3(cam.transform.position.x + -(Mathf.Sign(cam.transform.position.x)), savePlayerPos.y + 1, -10);
        }

        //Y
        if (cam.transform.position.y > point1Y && cam.transform.position.y < point2Y)
        {
            this.transform.position = new Vector3(savePlayerPos.x, savePlayerPos.y + 1, -10);
        }
        else if (player.transform.position.y > point1Y && player.transform.position.y < point2Y)
        {
            this.transform.position = new Vector3(cam.transform.position.x, savePlayerPos.y + -(Mathf.Sign(cam.transform.position.x)), -10);
        }


    }

    private void OnDrawGizmos()
    {
        float offset;
        offset = 8.9f;

        if(isActiveOnce == false)
        {
            Gizmos.DrawWireSphere(new Vector3(this.gameObject.transform.position.x + point1X - offset, this.gameObject.transform.position.y, -10), 0.5f);
            Gizmos.DrawWireSphere(new Vector3(this.gameObject.transform.position.x + point2X + offset, this.gameObject.transform.position.y, -10), 0.5f);

            Gizmos.DrawWireSphere(new Vector3(this.gameObject.transform.position.x , this.gameObject.transform.position.y + point1Y - offset, -10), 0.5f);
            Gizmos.DrawWireSphere(new Vector3(this.gameObject.transform.position.x , this.gameObject.transform.position.y + point2Y + offset, -10), 0.5f);
        }
      
    }

}
