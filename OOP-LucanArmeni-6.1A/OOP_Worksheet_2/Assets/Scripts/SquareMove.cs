using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquareMove : MonoBehaviour
{
    Vector2 direction = Vector2.up;

    float cooldown;
    float cooldownTime = 0.3f;

    float YPos = 0f;
    float XPos = 0f;

    bool hasMoved;

    // Start is called before the first frame update
    void Start()
    {
        hasMoved = false;

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
        if (cooldown < 0f)
        {
            if(hasMoved == false)
            {
                SimpleMovement();
            }
            //Reset
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 1f);
            //Debug.Log(hit.collider);       
            if (hasMoved == true)  
            {
                if (hit.collider == null)
                {
                    this.gameObject.transform.position += transform.TransformDirection(new Vector3(XPos, YPos, 0f));
                }
                // Resets value
                cooldown = cooldownTime;
                hasMoved = false;
                XPos = 0f;
                YPos = 0f;
                direction = Vector2.zero;
                
            }
        }

    }

    void SimpleMovement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            YPos = 1f;
            hasMoved = true;
            direction = Vector2.up;
        }

        else if (Input.GetKey(KeyCode.A))
        {
            XPos = -1f;
            hasMoved = true;
            direction = Vector2.left;

        }

        else if (Input.GetKey(KeyCode.S))
        {
            YPos = -1f;
            hasMoved = true;
            direction = Vector2.down;

        }

        else if (Input.GetKey(KeyCode.D))
        {
            XPos = 1f;
            hasMoved = true;
            direction = Vector2.right;

        }
    }
}
