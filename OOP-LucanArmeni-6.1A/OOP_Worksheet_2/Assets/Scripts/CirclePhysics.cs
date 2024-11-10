using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CirclePhysics : MonoBehaviour
{
    public GameObject circlePrefab;
    static public bool hasJumped;

    // Start is called before the first frame update
    void Start()
    {
        hasJumped = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Space) && Cooldown.cooldown < 0f)
        {
            hasJumped = true;
            CallJump();
        }
    }

    void CallJump()
    {
      circlePrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 20f * 200f) * Time.deltaTime;
    }
}
