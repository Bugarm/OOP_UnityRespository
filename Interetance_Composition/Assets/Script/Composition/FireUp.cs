using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireUp : MonoBehaviour
{
    [SerializeField] float ySpeed;
    
    public void Fire(Rigidbody2D rb)
    {
        Vector2 myforce = new Vector2(0f, ySpeed);
        rb.AddForce(myforce);
    }
}
