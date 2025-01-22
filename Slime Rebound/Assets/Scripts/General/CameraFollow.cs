using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Player player;
    private Vector3 savePlayerPos;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        savePlayerPos = player.ReturnInstance().transform.position;
        this.transform.position = new Vector3(savePlayerPos.x,savePlayerPos.y + 1,-10);

    }
}
