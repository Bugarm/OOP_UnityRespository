using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefualtBullet : MonoBehaviour
{
    [SerializeField] float xSpeed, ySpeed;
    protected Rigidbody2D rb;

    public void Awake() // so it sets up before start
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Vector2 myForce = new Vector2(xSpeed, ySpeed);
        rb.AddForce(myForce);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);    
    }

}
