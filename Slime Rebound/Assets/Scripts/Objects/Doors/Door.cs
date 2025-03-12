using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    protected bool isAtDoor;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        isAtDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(PlayerState.IsBounceMode == false && PlayerState.IsDamaged == false && PlayerState.IsCrouch == false && PlayerState.IsJump == false && PlayerState.IsPound == false && PlayerState.IsSlide == false && PlayerState.IsHeadAttack == false)
        { 
            if(collision.CompareTag("PlayerBody"))
            {
                isAtDoor = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("PlayerBody"))
        {
            isAtDoor = false;
        }
        
    }
}
