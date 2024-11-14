using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMovement
{
    private float xpos, ypos,xspeed,yspeed;
    
    public ShapeMovement(float _xpos, float _ypos,float _xspeed, float _yspeed)
    {
        xpos = _xpos;
        ypos = _ypos;
        xspeed = _xspeed;
        yspeed = _yspeed;
    }


    public void MoveUp(Rigidbody2D rb)
    {
        if(Input.GetKeyDown(KeyCode.W)) 
        {
            rb.velocity = new Vector2(0f, yspeed);
        }

    }

    public void MoveDown(Rigidbody2D rb)
    {
        if(Input.GetKeyDown(KeyCode.S)) 
        {
            rb.velocity = new Vector2(0f, -yspeed);
        }

    }

    public void MoveLeft(Rigidbody2D rb)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.velocity = new Vector2(-xspeed, 0f);
        }

    }

    public void MoveRight(Rigidbody2D rb)
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.velocity = new Vector2(xspeed, 0f);
        }

    }

    public void ResetPosition(Rigidbody2D rb)
    {
        rb.position = new Vector2(xpos,ypos);
    }
}
