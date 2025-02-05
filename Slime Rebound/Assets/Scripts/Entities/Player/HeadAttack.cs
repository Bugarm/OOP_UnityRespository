using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAttack : Singleton<HeadAttack>
{
    private GameObject head;
    protected override void Awake()
    {
        base.Awake();
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
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if((collision.CompareTag("Level") || collision.CompareTag("Enemies")) && PlayerState.IsHeadThrown == true)
        {

            PlayerState.IsHeadThrown = false;
            Destroy(head.transform.parent.gameObject);
            
        }
        
        if(!(collision.CompareTag("Level") || collision.CompareTag("Enemies")) && PlayerState.IsHeadThrown == true)
        {
            float moveSlight;
            if (head.transform.localScale.x == -1)
            {
                moveSlight = -1.5f;
            }
            else
            {
                moveSlight = 1.5f;
            }

            head.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSlight, -1.5f);
            head.GetComponent<Rigidbody2D>().gravityScale = 14;
        }

    }

    private void OnBecameInvisible()
    {
        PlayerState.IsHeadThrown = false;
        Destroy(head.transform.parent.gameObject);
    }
}
