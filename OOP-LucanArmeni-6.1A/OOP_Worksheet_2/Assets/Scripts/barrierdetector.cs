using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrierdetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = Vector2.up;

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.up, 0.5f);

        // If it hits something...
        if (hit.collider == null)
        {

        }
    }
}
