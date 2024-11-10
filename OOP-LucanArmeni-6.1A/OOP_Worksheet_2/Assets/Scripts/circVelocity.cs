using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circVelocity : MonoBehaviour
{
    Rigidbody2D rigidbody2;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = this.gameObject.GetComponent<Rigidbody2D>();
        rigidbody2.velocity = new Vector2(10f, 15f);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
