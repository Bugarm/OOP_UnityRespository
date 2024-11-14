using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannoball : DefualtBullet
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); // it runs the start method of the parent //IMP
        rb.gravityScale = 1.0f;
    }

}
