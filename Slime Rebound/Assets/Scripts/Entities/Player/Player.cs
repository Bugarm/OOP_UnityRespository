using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public int walkSpeed;
    public int runSpeed;
    public int bounceSpeed;
    public int dashPower;

    [Header("Physics Material")]
    public PhysicsMaterial2D slip;
    public PhysicsMaterial2D stick;
    public PhysicsMaterial2D bounce;

    [Header("Player Collider")]
    public CapsuleCollider2D idleCollider;
    public BoxCollider2D crouchCollider;
    public CircleCollider2D bounceCollider;
    
    [Header("Player Attack Trigger")]
    public BoxCollider2D attackTrigger;
    public BoxCollider2D poundTrigger;
    public CircleCollider2D bounceTrigger;

    [Header("Player Sprite")]
    public SpriteRenderer playerSprite;

    [Header("Remove Later")]
    public Sprite idle;
    public Sprite crouch;

    private GameObject player;
    private Rigidbody2D playerRB;

    private float dirX;
    private float dirY;
    private int jumpPower;
    private float speed;

    private bool isTouchingWall, isTouchingGround, isTouchingTop;
    private bool isJump, isDoubleJump, isDash, isMove, isCrouch, isPound, isAttack;

    private bool bounceMode;
    private bool stickActive;

    private Coroutine jumpRoutine, doubleJumpRoutine, dashRoutine, poundRoutine, attackRoutine;

    void Awake()
    {

        player = this.gameObject;
        playerRB = GetComponent<Rigidbody2D>();

        dirX = 0;
        dirY = 0;
        jumpPower = 7;

        speed = walkSpeed;

        
    }

    // Start is called before the first frame update
    void Start()
    {
        poundTrigger.gameObject.SetActive(false);
        bounceTrigger.gameObject.SetActive(false);
        attackTrigger.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingWall = WallDetection.instance.ReturnDetection();
        isTouchingTop = TopDectection.instance.ReturnDetection();
        isTouchingGround = FloorCollider.instance.ReturnDetection();

        ResetChecks();

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

    // Do easy simple long jump Attack here

    // Do Head Attack

    // Should be it hopefully

    void ResetChecks()
    {
        // Disable when stick active Touches Ground
        if (stickActive == true && isTouchingGround == true)
        {
            stickActive = false;
            dirX = 0;
            playerRB.sharedMaterial = slip;
        }

        // Disable Jump when groundpound
        if (isJump == true && isPound == true)
        {
            StopCoroutine(JumpFunction());
            jumpRoutine = null;
        }

        // Allows Ground pound when bounce mode
        if(bounceMode == true && isPound == true)
        {
            bounceMode = false;

            playerRB.sharedMaterial = slip;
            playerRB.gravityScale = 10; // DO smn different asp
            //Reset velocity
            dirX = 0;
        }

        // Allows Double Jump
        if(isDoubleJump == true)
        {
            StopCoroutine(JumpFunction());
            jumpRoutine = null;
        }

        // Allows to stick while bounce
        if(Input.GetKeyDown(KeyCode.L) && (bounceMode == true && isTouchingWall == true))
        {
            bounceMode = false;
        }
    }

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
        if((stickActive == false && bounceMode == false && isPound == false))
        {
            if (isTouchingGround == false)
            {
                // Player falls down faster every frame
                dirY -= 0.04f;
            }
            else if (isJump == false)
            {
                dirY = 0;
            }
        }

        if (stickActive == true && isTouchingWall == false)
        {
            dirY -= 0.06f;
        }


    }

    void AnimationController() // Remove Later
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerSprite.sprite = crouch;
        }

        if (!Input.GetKey(KeyCode.S) && isTouchingTop == false)
        {
            playerSprite.sprite = idle;
        }
    }

    void KeyPressed()
    {
        // Stick to wall switch
        if (Input.GetKeyDown(KeyCode.L) && isCrouch == false)
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
        }

        // Bounce Setup 
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (isTouchingGround == true && isCrouch == false)
            {
                PlayerCollision("Bounce");
                AttackTrigger("Bounce");

                if (stickActive == false)
                {
                    bounceMode = !bounceMode;

                    if (bounceMode == true)
                    {
                        playerRB.sharedMaterial = bounce;
                        playerRB.gravityScale = 1.7f;
                    }
                }
            }
            else if (bounceMode == true)
            {
                PlayerCollision("Idle");
                AttackTrigger("Reset");

                bounceMode = !bounceMode;

                playerRB.sharedMaterial = slip;
                playerRB.gravityScale = 10;
                //Reset velocity
                dirX = 0;
            }
        }

        // Run
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && isCrouch == false)
        {
            speed = runSpeed;
        }

        //Normal movement
        if (stickActive == false && isDash == false && isPound == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                dirX = -speed;

            }
            if (Input.GetKey(KeyCode.D))
            {
                dirX = speed;
            }

            // Do another version of this
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                isMove = true;
                player.transform.localScale = new Vector2((Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);
            }
        }
        // Stick velocity
        else if (isPound == false)
        {
            if (player.transform.localScale.x == -1)
            {
                dirX = -10;
            }
            else if (player.transform.localScale.x == 1)
            {
                dirX = 10;
            }
        }

        // Crouch
        if (Input.GetKey(KeyCode.S))
        {
            if (isTouchingGround == true && stickActive == false && bounceMode == false)
            {
                isCrouch = true;
                speed = walkSpeed - 2;
                // Collider Changes
                PlayerCollision("Crouch");
            }
        // Ground Pound
            else if(isTouchingGround == false)
            {
                if (poundRoutine == null)
                {
                    poundRoutine = StartCoroutine(GroundPoundFunc());
                }
            }
        }

        // Dash and Attack
        if (Input.GetKey(KeyCode.J))
        {
            // Dash
            if (isMove == true && isCrouch == false)
            {
                if (dashRoutine == null)
                {
                    dashRoutine = StartCoroutine(DashFunction());
                }
            }

            //Attack
            if (attackRoutine == null)
            {
                attackRoutine = StartCoroutine(AttackFunction());
            }
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isDoubleJump == false)
        {
            if (stickActive == false && isTouchingGround == true)
            {
                if (jumpRoutine == null)
                {
                    jumpRoutine = StartCoroutine(JumpFunction());
                }
            }
            // Sticky Wall Jump
            else if (stickActive == true && isTouchingWall == true && isCrouch == false)
            {

                if (jumpRoutine == null)
                {
                    // Flip
                    player.transform.localScale = new Vector2(-(Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);

                    jumpRoutine = StartCoroutine(JumpFunction());
                }

            }
        }
        
        // Double Jump
        if (Input.GetKeyDown(KeyCode.Space) && isTouchingGround == false)
        {
            if (doubleJumpRoutine == null)
            {
                doubleJumpRoutine = StartCoroutine(DoubleJumpFunct());
            }
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
                isMove = false;
                dirX = 0;
            }

            // Crouch
            if (isCrouch = true && (isTouchingTop == false && !Input.GetKey(KeyCode.S) || (Input.GetKeyUp(KeyCode.S) && isTouchingTop == false)) )
            {
                isCrouch = false;
                speed = walkSpeed;
                // Collider Changes
                PlayerCollision("Idle");
            }
        }
    }

    // Collision and Triggers Setup
    void PlayerCollision(string mode)
    {
        switch (mode)
        {
            case "Idle":
                idleCollider.gameObject.SetActive(true);
                crouchCollider.gameObject.SetActive(false);
                bounceCollider.gameObject.SetActive(false);

                break;

            case "Crouch":
                idleCollider.gameObject.SetActive(false);
                crouchCollider.gameObject.SetActive(true);
                bounceCollider.gameObject.SetActive(false);

                break;

            case "Bounce":
                idleCollider.gameObject.SetActive(false);
                crouchCollider.gameObject.SetActive(false);
                bounceCollider.gameObject.SetActive(true);

                break;

            default:
                Debug.Log("ERROR");
                break;

        }
    }

    void AttackTrigger(string mode)
    {
        switch (mode)
        {
            case "Normal-Attack":
                poundTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(true);

                break;

            case "Head-Attack":
                // WIP

                break;

            case "Bounce":
                poundTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(true);
                attackTrigger.gameObject.SetActive(false);

                break;

            case "Pound":
                poundTrigger.gameObject.SetActive(true);
                bounceTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);

                break;

            case "Reset":
                poundTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);

                break;

            default:
                Debug.Log("ERROR");
                break;

        }
    }

    // Routines
    IEnumerator AttackFunction()
    {
        isAttack = true;
        AttackTrigger("Normal-Attack");
        yield return new WaitForSeconds(0.5f);
        AttackTrigger("Reset");

        isAttack = false;
        attackRoutine = null;
    }
    
    IEnumerator GroundPoundFunc()
    {
        isPound = true;
        playerRB.gravityScale = 0;
        AttackTrigger("Pound");

        // Air Time
        dirX = 0;
        dirY = 0;
        yield return new WaitForSeconds(0.5f);

        while (isTouchingGround == false)
        {
            dirY = -10f;
            yield return new WaitForSeconds(0.1f);
        }

        playerRB.gravityScale = 10;
        isPound = false;

        yield return new WaitForSeconds(0.3f);
        poundTrigger.gameObject.SetActive(false);
        poundRoutine = null;
    }

    IEnumerator DashFunction()
    {
        if (player.transform.localScale.x == -1)
        {
            dirX = -dashPower;
        }
        else
        {
            dirX = dashPower;
        }
        
        isDash = true;
        AttackTrigger("Normal-Attack");
        yield return new WaitForSeconds(0.4f);
        AttackTrigger("Reset");
        isDash = false;
        dirX = 0;

        dashRoutine = null;

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

    IEnumerator DoubleJumpFunct()
    {
        dirY = jumpPower;
        isDoubleJump = true;
        yield return new WaitForSeconds(0.4f);
        isDoubleJump = false;
        dirY = 0;

        doubleJumpRoutine = null;
    }

}
