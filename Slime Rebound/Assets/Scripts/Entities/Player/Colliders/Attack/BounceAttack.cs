using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAttack : DetectionScript
{
    public static BounceAttack instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
