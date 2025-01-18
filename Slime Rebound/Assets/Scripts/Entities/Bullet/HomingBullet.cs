using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : DefaultBullet
{
    [SerializeField] GameObject playerPos;
    private Rigidbody2D playerRB;

    protected override void Awake()
    {
        base.Awake();

        playerRB = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        base.OnEnable();

    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }
}
