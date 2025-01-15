using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    protected GameObject bullet;
    protected Rigidbody2D bulletRB;

    protected virtual void OnEnable()
    {
        bullet = this.gameObject;
        bulletRB = bullet.GetComponent<Rigidbody2D>();
        
        bulletRB.gravityScale = 1;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Level"))
        {
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Player"))
        {
            //Damage Player Here

            gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
