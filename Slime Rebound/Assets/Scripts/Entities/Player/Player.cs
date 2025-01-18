using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public int walkSpeed;
    public int runSpeed;
    public int bounceSpeed;
    //public int jumpPower;

    [Header("Physics Material")]
    public PhysicsMaterial2D slip;
    public PhysicsMaterial2D stick;
    public PhysicsMaterial2D bounce;

    private BoxCollider2D floorCollision;

    private GameObject player;
    private Rigidbody2D playerRB;
    private SpriteRenderer playerSprite;

    private float dirX;
    private float dirY;
    private int jumpPower;
    private float speed;

    private bool isTouchingGround;
    private bool isTouchingWall;
    private bool isJump;
    private bool bounceMode;

    private bool stickActive;

    private Coroutine jumpRoutine;

    private void Awake()
    {

        player = this.gameObject;
        playerRB = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();

        floorCollision = this.gameObject.GetComponent<BoxCollider2D>();

        dirX = 0;
        dirY = 0;
        jumpPower = 7;

        speed = walkSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingWall = WallDetection.instance.ReturnWallDetection();

        KeyPressed();
        AnimationController();
        KeyReleased();

        BounceMode();
        PlayerAcceleration();

        if(bounceMode == true)
        {
            playerRB.velocity = new Vector3(dirX, playerRB.velocity.y, 0);
        }
        else
        {
            playerRB.velocity = new Vector3(dirX, dirY, 0);
        }
    }

    // Do double jump or easy simple jump here
    // Yea do research on this one

    // Do dash
    // like jump but using y intead

    // Do a simple attack (Right attack)
    // Do a child game object to be active and check it's trigger if on enemy

    // Do slam
    // Idea: Velocity X keeps doing down until it hits level by using ground detection

    void BounceMode()
    {
        if (bounceMode == true)
        {
            if (player.transform.localScale.x == -1)
            {
                dirX = -bounceSpeed;
            }
            else if (player.transform.localScale.x == 1)
            {
                dirX = bounceSpeed;
            }

            if (isTouchingWall == true)
            {
                player.transform.localScale = new Vector2(-(Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);

                playerRB.velocity = new Vector3(playerRB.velocity.x, 12.3f, 0);

            }

        }
    }

    void PlayerAcceleration()
    {
        if((stickActive == false && bounceMode == false) || isTouchingGround == false)
        {
            if (isTouchingGround == false)
            {
                // Player falls down faster every frame
                dirY -= 0.02f;
            }
            else if (isJump == false)
            {
                dirY = 0;
            }
        }
        
    }

    void AnimationController()
    {
        

    }

    void KeyPressed()
    {
        // Stick to wall switch
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (bounceMode == false)
            {
                stickActive = !stickActive;
                if (stickActive == true)
                {
                    playerRB.sharedMaterial = stick;
                }
                else
                {
                    playerRB.sharedMaterial = slip;
                }

            }
            else
            {
                playerRB.sharedMaterial = slip;
            }
        }

        // Bounce Setup 
        if(Input.GetKeyDown(KeyCode.K) && isTouchingGround == true)
        {
            if(stickActive == false)
            { 
                bounceMode = !bounceMode;

                if(bounceMode == true)
                {
                    playerRB.sharedMaterial = bounce;
                    playerRB.gravityScale = 1.7f;
                }
            }
        }
        else if(bounceMode == true && Input.GetKeyDown(KeyCode.K))
        {
            bounceMode = !bounceMode;

            playerRB.sharedMaterial = slip;
            playerRB.gravityScale = 10; // DO smn different asp
            //Reset velocity
            dirX = 0;
        }

        // Run
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            speed = runSpeed;
        }

        //Normal movement
        if (stickActive == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                dirX = -speed;
                
            }
            if (Input.GetKey(KeyCode.D))
            {
                dirX = speed;
            }

            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                player.transform.localScale = new Vector2((Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);
            }
        }
        // Stick velocity
        else
        {
            if (player.transform.localScale.x == -1)
            {
                dirX = -8;
            }
            else if (player.transform.localScale.x == 1)
            {
                dirX = 8;
            }
        }

        // Crouch
        if (Input.GetKey(KeyCode.S))
        {

        }

        // Jump
        if (stickActive == false && Input.GetKeyDown(KeyCode.Space) && isTouchingGround == true)
        {
            if (jumpRoutine == null)
            {
                jumpRoutine = StartCoroutine(JumpFunction());
            }
        }     

        // Sticky Wall Jump
        else if (stickActive == true && Input.GetKeyDown(KeyCode.Space) && isTouchingWall == true)
        {
            
            if (jumpRoutine == null)
            {
                // Flip
                player.transform.localScale = new Vector2(-(Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);

                jumpRoutine = StartCoroutine(JumpFunction());  
            }
            
        }

        // Disable when stick active Touches Ground
        if(stickActive == true && isTouchingGround == true)
        {
            stickActive = false;
            dirX = 0;
            playerRB.sharedMaterial = slip;
        }

    }

    void KeyReleased()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            speed = walkSpeed;
        }

        if (stickActive == false && bounceMode == false)
        { 
            // Movement
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                dirX = 0;
            }

            // Crouch
            if (Input.GetKeyUp(KeyCode.S))
            {

            }
        }
    }

    IEnumerator JumpFunction()
    {
        dirY = jumpPower;
        isJump = true;
        yield return new WaitForSeconds(0.4f);
        isJump = false;
        dirY = 0;

        jumpRoutine = null;

    }


    // Floor Collision
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Level"))
        {
            if (floorCollision.IsTouching(collision))
            {
                isTouchingGround = true;
            }
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Level"))
        {
            isTouchingGround = false;
        }
    }

    private void OnBecameInvisible()
    {
        player.transform.position = new Vector2(-12,-2.50f);
    }
}
