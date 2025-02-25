using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBounce : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBody"))
        {
            if(PlayerState.IsBounceMode == true)
            { 
                PlayerState.IsBounceMode = false;
            }
        }
    }
}
