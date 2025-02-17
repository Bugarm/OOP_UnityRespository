using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : Singleton<BackgroundScroll>
{

    public GameObject BackGround_Back;
    public GameObject BackGround_Front;

    private Vector3 point1Pos;
    private Vector3 point2Pos;

    private Player player;
    private Vector3 savePlayerPos;

    private bool doOnce;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<Player>();

        if (player != null)
        {

            savePlayerPos = player.transform.position;
            if(BackGround_Back != null)
            {
                Vector3 backBacknewPos = Vector3.MoveTowards(BackGround_Back.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, 0), 22.5f * Time.deltaTime);
                BackGround_Back.transform.position = Vector3.Slerp(BackGround_Back.transform.position, backBacknewPos, Time.deltaTime * 75);
            }

            if (BackGround_Front != null)
            {
                Vector3 backFrontnewPos = Vector3.MoveTowards(BackGround_Front.transform.position, new Vector3(player.transform.position.x, player.transform.position.y, 0), 24 * Time.deltaTime);

                BackGround_Front.transform.position = Vector3.Slerp(BackGround_Front.transform.position, backFrontnewPos, Time.deltaTime * 84);
            }
        }
    }

    public void ResetBackGroundPos()
    {
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            if (BackGround_Back != null)
            {
                BackGround_Back.transform.position = player.transform.position;
            }

            if (BackGround_Front != null)
            {
                BackGround_Front.transform.position = player.transform.position;
            }
        }
    }
}
