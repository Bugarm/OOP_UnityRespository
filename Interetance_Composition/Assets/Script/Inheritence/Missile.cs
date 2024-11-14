using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : DefualtBullet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); //IMP
        rb.gravityScale = 0.5f;
    }

}
