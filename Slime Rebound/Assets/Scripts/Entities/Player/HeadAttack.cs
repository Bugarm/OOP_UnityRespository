using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAttack : MonoBehaviour
{
    private GameObject head;
    private void Awake()
    {
        head = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }  
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Level") && PlayerState.IsHeadThrown == true)
        {

            PlayerState.IsHeadThrown = false;
            Destroy(head.transform.parent.gameObject);
            
        }
    }

    private void OnBecameInvisible()
    {
        PlayerState.IsHeadThrown = false;
        Destroy(head.transform.parent.gameObject);
    }
}
