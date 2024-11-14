using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletControl : MonoBehaviour
{
    Rigidbody2D rb;
    FireUp gunFire;
    Horizontal_Projectile myprojectile;

   
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gunFire = GetComponent<FireUp>(); //need to get component
        myprojectile = GetComponent<Horizontal_Projectile>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.gravityScale = 0f;
        
        if(gunFire != null) gunFire.Fire(rb); //checks if gunfire instance exist before using it
        

        try //OPTION2: checking if projectile exist using exception handing try..catch
        {
            myprojectile.FireProjectile(rb);
        }
        catch(NullReferenceException ex) 
        {
            Debug.Log(this.gameObject + "has no projectile firing. Details..." + ex);
        }

    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
