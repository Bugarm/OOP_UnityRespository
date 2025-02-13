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

        if (player != null && (BackGround_Back != null && BackGround_Front != null))
        {

            savePlayerPos = player.transform.position;

            BackGround_Back.transform.position = Vector3.MoveTowards(BackGround_Back.transform.position, new Vector2(player.transform.position.x, player.transform.position.y), 14 * Time.deltaTime);

            BackGround_Front.transform.position = Vector3.MoveTowards(BackGround_Front.transform.position, new Vector2(player.transform.position.x, player.transform.position.y), 6.5f * Time.deltaTime);

        }
    }

    public void ResetBackGroundPos()
    {
        player = FindObjectOfType<Player>();
        if (player != null && (BackGround_Back != null && BackGround_Front != null))
        {
            BackGround_Back.transform.position = player.transform.position;
            BackGround_Front.transform.position = player.transform.position;
        }
    }
}
