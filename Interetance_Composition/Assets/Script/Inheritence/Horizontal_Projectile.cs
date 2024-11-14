using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horizontal_Projectile : MonoBehaviour
{
    [SerializeField] float xSpeed;

    public void FireProjectile(Rigidbody2D rb)
    {
        rb.gravityScale = 1.0f;
        Vector2 myforce = new Vector2(xSpeed, 0.0f);
        rb.AddForce(myforce);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
