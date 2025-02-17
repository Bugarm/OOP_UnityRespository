using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    public int speed;

    protected GameObject bullet;
    protected Rigidbody2D bulletRB;

    private Coroutine deleteCourtine;

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

    IEnumerator ResetBullet()
    {
        yield return new WaitForSeconds(0.05f);
        this.gameObject.SetActive(false);
        // It keeps the data so it needs to reset
        bulletRB.velocity = new Vector3(0, 0, 0);
        this.gameObject.transform.position = new Vector3(0, 0, 0);
        this.gameObject.transform.rotation = Quaternion.identity;
        deleteCourtine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Level") || collision.CompareTag("Box") || collision.CompareTag("Obsticales") || collision.CompareTag("Platforms") || collision.CompareTag("FloorBreakable"))
        {
            if (deleteCourtine == null)
            {
                deleteCourtine = StartCoroutine(ResetBullet());
            }
        }
        
        if (collision.CompareTag("PlayerBody"))
        {
            if (deleteCourtine == null)
            {
                deleteCourtine = StartCoroutine(ResetBullet());
            }
        }
    }

    private void OnBecameInvisible()
    {
        
        if (this.gameObject.activeInHierarchy == true && deleteCourtine == null)
        {
            deleteCourtine = StartCoroutine(ResetBullet());
        }

    }
}
