using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour
{
    static public float cooldown;
    float cooldownTime = 5f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Stops cooldown when is done
        if (cooldown > -0.1f)
        {
            cooldown -= Time.deltaTime;
        }

        //Debug.Log("Time " + cooldown);

        // Resets cooldown after jump
        if (cooldown < 0f && CirclePhysics.hasJumped == true)
        {
            cooldown = cooldownTime;
            CirclePhysics.hasJumped = false;
        }
    }
}
