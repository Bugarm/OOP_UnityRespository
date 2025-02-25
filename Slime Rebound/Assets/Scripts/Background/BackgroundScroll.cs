using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : Singleton<BackgroundScroll>
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceX = cam.transform.position.x * parallaxEffect;

        float movementX = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distanceX, cam.transform.position.y, transform.position.z);
        // X
        if (movementX > startPos + length)
        {
            startPos += length;
        }
        else if (movementX < startPos - length)
        {
            startPos -= length;
        }

    }
}
