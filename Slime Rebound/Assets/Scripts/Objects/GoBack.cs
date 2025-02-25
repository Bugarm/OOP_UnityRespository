using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : MonoBehaviour
{
    public Vector2 tpTo;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBody"))
        {
            collision.transform.parent.parent.transform.position = tpTo;
        }
    }
}
