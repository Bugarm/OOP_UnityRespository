using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : DetectionScript
{
    public static FloorCollider instance;

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
