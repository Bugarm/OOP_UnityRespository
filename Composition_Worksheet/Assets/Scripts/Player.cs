using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : Character
{
    public float speed = 5f;
    protected override void Start()
    {
        base.Start();
        // Additional setup if needed
    }

    void Update()
    {
        Move();

    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontal, 0, 0);
        transform.Translate(movement * speed * Time.deltaTime);
    }

}

