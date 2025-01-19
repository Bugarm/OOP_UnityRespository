using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDectection : DetectionScript
{
    public static TopDectection instance;


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
