using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    public int speed;

    protected GameObject bullet;
    protected Rigidbody2D bulletRB;

    protected virtual void Awake()
    {
        bullet = this.gameObject;
        bulletRB = bullet.GetComponent<Rigidbody2D>();

        bulletRB.gravityScale = 0;
    }

    protected virtual void OnEnable()
    {
        bulletRB.velocity = transform.up * speed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    void ResetBullet()
    {
        this.gameObject.SetActive(false);
        // It keeps the data so it needs to reset
        bulletRB.velocity = new Vector3(0, 0, 0);
        this.gameObject.transform.position = new Vector3(0, 0, 0);
        this.gameObject.transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Level"))
        {
            ResetBullet();
        }
        else if (collision.CompareTag("Player"))
        {
            //Damage Player Here
            ResetBullet();
        }
    }

    private void OnBecameInvisible()
    {
        ResetBullet();

    }
}
