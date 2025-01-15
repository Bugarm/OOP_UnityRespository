using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : DefaultBullet
{
    [SerializeField] Transform playerPos;

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();

        bulletRB.velocity = new Vector2();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
