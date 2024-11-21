using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefualtBullet : MonoBehaviour
{
    [SerializeField] float speed;
    protected Rigidbody2D rb;

    public void Awake() // so it sets up before start
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    protected virtual void Start()
    {

    }

    // Start is called before the first frame update
    protected virtual void OnEnable()
    {
        rb.velocity = transform.up * speed;


    }

    void OnBecameInvisible()
    {
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

}
