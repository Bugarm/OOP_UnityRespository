using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Player Instance;

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
    public BoxCollider2D jumpAttackTrigger;

    [Header("Player Head")]
    public GameObject headAttack;
    public int headGravityPower;
    public GameObject throwArrow;

    [Header("Player Sprite")]
    public SpriteRenderer playerSprite;

    [Header("Remove Later")]
    public Sprite idle;
    public Sprite crouch;

    private GameObject player;
    private Rigidbody2D playerRB;

    private float dirX;
    private float dirY;
    private float jumpPower;
    private int attackJumpPower;
    private float speed;

    // Collision Check
    private bool isTouchingWall, isTouchingGround, isTouchingTop;

    public static bool bounceMode, headAttackMode;
    private bool stickActive;

    private float oldSpeed;

    private Coroutine jumpRoutine, doubleJumpRoutine, dashRoutine, poundRoutine, attackRoutine, attackJumpRoutine, headAttackRoutine, bounceRoutine, slideRoutine;

    void Awake()
    {
        Instance = this;

        player = this.gameObject;
        playerRB = GetComponent<Rigidbody2D>();

        throwArrow.SetActive(false);

        dirX = 0;
        dirY = 0;
        jumpPower = 6.5f;
        attackJumpPower = 8;

        speed = walkSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        AttackTrigger("Reset");

        PlayerCollision("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        // CHANGE THESE
        isTouchingWall = WallDetection.instance.ReturnDetection();
        isTouchingTop = TopDectection.instance.ReturnDetection();
        isTouchingGround = FloorCollider.instance.ReturnDetection();

        ResetChecks();

        KeyPressed();
        AnimationController();
        KeyReleased();
        
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
        if (PlayerState.IsJump == true && PlayerState.IsPound == true)
        {
            StopCoroutine(JumpFunction());
            jumpRoutine = null;
        }

        // Allows Ground pound when bounce mode
        if(bounceMode == true && PlayerState.IsPound == true)
        {
            bounceMode = false;

            playerRB.sharedMaterial = slip;
            playerRB.gravityScale = 10; // DO smn different asp
            //Reset velocity
            dirX = 0;
        }

        // Allows to stick while bounce
        if(Input.GetKeyDown(KeyCode.L) && (bounceMode == true && isTouchingWall == true))
        {
            bounceMode = false;
        }
    }

    void PlayerAcceleration()
    {
        if((stickActive == false && bounceMode == false && PlayerState.IsPound == false && PlayerState.IsAttackJump == false && PlayerState.IsDoubleJump == false && PlayerState.IsJump == false))
        {
            if (isTouchingGround == false)
            {
                // Player falls down faster every frame
                dirY -= 0.15f;
            }
            else
            {
                // Reset Velocity when it hits ground
                dirY = 0;
            }

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
        if (Input.GetKeyDown(KeyCode.L) && PlayerState.IsCrouch == false && PlayerState.IsHeadAttack == false)
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
        if (Input.GetKeyDown(KeyCode.K) && PlayerState.IsHeadAttack == false && PlayerState.IsCrouch == false && stickActive == false && (isTouchingGround == true || bounceMode == true))
        {
            bounceMode = !bounceMode;

            if (bounceRoutine == null)
            {
                bounceRoutine = StartCoroutine(BounceFunction());
            }
        }

        // Run
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && PlayerState.IsCrouch == false)
        {
            PlayerState.IsRun = true;
            speed = runSpeed;
        }

        //Normal movement
        if (stickActive == false && PlayerState.IsDash == false && PlayerState.IsPound == false && PlayerState.IsSlide == false)
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
                PlayerState.IsMove = true;

                player.transform.localScale = new Vector2((Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);
            }
        }

        // Stick velocity
        if (stickActive == true && PlayerState.IsPound == false && PlayerState.IsHeadAttack == false)
        {
            if (player.transform.localScale.x == -1)
            {
                dirX = -7.5f;
            }
            else if (player.transform.localScale.x == 1)
            {
                dirX = 7.5f;
            }
        }

        // Saves old Speed for the crouch
        if (Input.GetKeyDown(KeyCode.S) && PlayerState.IsHeadAttack == false)
        {
            if (oldSpeed <= 0)
            {
                oldSpeed = walkSpeed;
            }
            else if(PlayerState.IsRun == true)
            {
                oldSpeed = runSpeed;
            }
            else
            {
                oldSpeed = walkSpeed;
            }
        }

        // Crouch & Slide & Jump Attack
        if (Input.GetKey(KeyCode.S) && PlayerState.IsHeadAttack == false && bounceMode == false)
        {
            if (stickActive == false && bounceMode == false && isTouchingGround == true)
            {

                // Collider Changes
                PlayerCollision("Crouch");

                // Slide
                if (PlayerState.IsRun == true)
                {
                    if(slideRoutine == null)
                    {
                        PlayerState.IsSlide = true;

                        slideRoutine = StartCoroutine(SlideFunction());
                    }
                }
                // Normal Crouch Speed
                else
                {
                    PlayerState.IsCrouch = true;
                    speed = walkSpeed - 2;
                }

                // Attack Jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (attackJumpRoutine == null)
                    {
                        attackJumpRoutine = StartCoroutine(AttackJumpFunc());
                    }
                }
                
            }
        }

        // Ground Pound
        if (Input.GetKeyDown(KeyCode.S) && PlayerState.IsHeadAttack == false)
        {
            if (stickActive == false || (stickActive == true && isTouchingWall == false))
            {
                if (isTouchingGround == false && PlayerState.IsAttackJump == false)
                {
                    if (poundRoutine == null)
                    {
                        poundRoutine = StartCoroutine(GroundPoundFunc());
                    }
                }
            }
        }

        // Dash & Attack & Head Attack
        if(Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.J))
        {
            if(isTouchingGround == true && PlayerState.IsCrouch == false && PlayerState.IsPound == false && PlayerState.IsDash == false && PlayerState.IsDoubleJump == false)
            { 
                if (headAttackRoutine == null)
                {
                    headAttackRoutine = StartCoroutine(HeadThrow());
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.J) && PlayerState.IsHeadAttack == false)
        {
            // Dash
            if (PlayerState.IsMove == true && PlayerState.IsCrouch == false)
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
        if (Input.GetKeyDown(KeyCode.Space) && PlayerState.IsJump == false && PlayerState.IsAttackJump == false)
        {
            if (stickActive == false && isTouchingGround == true && PlayerState.IsJump == false)
            {
                if (jumpRoutine == null)
                {
                    jumpRoutine = StartCoroutine(JumpFunction());
                }
            }
            // Sticky Wall Jump
            else if (stickActive == true && isTouchingWall == true && PlayerState.IsCrouch == false)
            {
                if (jumpRoutine == null)
                {
                    // Flip
                    if (player.transform.localScale.x == -1)
                    {
                        player.transform.localScale = new Vector2(1, 1);
                    }
                    else
                    {
                        player.transform.localScale = new Vector2(-1, 1);
                    }

                    jumpRoutine = StartCoroutine(JumpFunction());

                }
            }
        }
        
        // Double Jump
        else if (Input.GetKeyDown(KeyCode.Space) && PlayerState.IsAttackJump == false && stickActive == false && PlayerState.IsHeadAttack == false)
        {
            StopCoroutine(JumpFunction());
            jumpRoutine = null;

            if (doubleJumpRoutine == null)
            {
                doubleJumpRoutine = StartCoroutine(DoubleJumpFunc());
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
                PlayerState.IsMove = false;
                dirX = 0;
            }

            // Crouch (do this better)
            if (PlayerState.IsCrouch == true && isTouchingTop == false && (!Input.GetKey(KeyCode.S) || (Input.GetKeyUp(KeyCode.S)) ))
            {

                PlayerState.IsCrouch = false;

                speed = oldSpeed;
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
                jumpAttackTrigger.gameObject.SetActive(false);

                break;

            case "Head-Attack":
                // WIP

                break;

            case "Jump-Attack":
                poundTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(true);

                break;

            case "Bounce":
                poundTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(true);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                break;

            case "Pound":
                poundTrigger.gameObject.SetActive(true);
                bounceTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                break;

            case "Reset":
                poundTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                break;

            default:
                Debug.Log("ERROR");
                break;

        }
    }

    // Routines
    IEnumerator BounceFunction()
    {
        bool doOnce = true;

        // 
        if (bounceMode == true)
        {
            PlayerCollision("Bounce");
            AttackTrigger("Bounce");

            playerRB.sharedMaterial = bounce;
            playerRB.gravityScale = 1.7f;
            playerRB.velocity = new Vector2(playerRB.velocity.x,2f);
        }

        while (bounceMode == true)
        {
            doOnce = true;
            yield return new WaitForSeconds(0.001f);

            if (player.transform.localScale.x == -1)
            {
                dirX = -bounceSpeed;
            }
            else if (player.transform.localScale.x == 1)
            {
                dirX = bounceSpeed;
            }

            if (isTouchingWall == true && doOnce == true)
            {
                player.transform.localScale = new Vector2(-(Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);

                playerRB.velocity = new Vector3(playerRB.velocity.x, 12.3f, 0);
                doOnce = false;
            }
            
        }

        // Cooldown
        if (bounceMode == false)
        {
            PlayerCollision("Idle");
            AttackTrigger("Reset");

            playerRB.sharedMaterial = slip;
            playerRB.gravityScale = 10;
            //Reset velocity
            dirX = 0;
            yield return new WaitForSeconds(1f);
            bounceRoutine = null;
        }

        
    }

    IEnumerator HeadThrow()
    {
        PlayerState.IsHeadAttack = true;
        PlayerState.IsHeadThrown = false;
        GameObject obj;
        GameObject headRender, headPhysics;

        float powerX, powerY;
        float arrowRotate;
        float xSpeed;

        arrowRotate = 0;
        powerX = 0;
        powerY = 0;
        xSpeed = 0;

        throwArrow.SetActive(true);

        // Ignore
        headPhysics = null;
        headRender = null;
        obj = null;

        float oldJumpPower = jumpPower;

        jumpPower = 3.5f;

        while (PlayerState.IsHeadAttack == true)
        {
            yield return new WaitForSeconds(0.001f);

            // Speed Control
            if (PlayerState.IsRun == true)
            {
                xSpeed = 9f;      
            }
            else
            {
                xSpeed = 7f;
            }

            // Direction
            if (player.transform.localScale.x == -1)
            {
                powerX = -xSpeed;
                arrowRotate = -34.68f;
            }
            else
            {
                powerX = xSpeed;
                arrowRotate = 36.08f;
            }

            // Create Head Obj
            if (obj == null && PlayerState.IsHeadThrown == false)
            {
                obj = Instantiate(headAttack, player.transform.position, Quaternion.identity);
                obj.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.5f);

                headPhysics = obj.transform.GetChild(0).gameObject;
                headRender = obj.transform.GetChild(1).gameObject;

                headPhysics.SetActive(false);
                headRender.SetActive(true);
            }

            powerY = 0;
            headPhysics.GetComponent<Rigidbody2D>().gravityScale = 0;
            throwArrow.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Follows Player
            if (PlayerState.IsHeadThrown == false)
            {
                obj.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.5f);
            }  

            // Cancel Attack
            if (PlayerState.IsHeadThrown == false && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.K)))
            {
                PlayerState.IsHeadAttack = false;
                PlayerState.IsHeadThrown = false;
                Destroy(obj);

                jumpPower = oldJumpPower;
                throwArrow.SetActive(false);
                headAttackRoutine = null;
            }

            // Upper Direction
            if (Input.GetKey(KeyCode.W))
            {
                powerY = 9.72f;
                headPhysics.GetComponent<Rigidbody2D>().gravityScale = headGravityPower;
                throwArrow.transform.rotation =  Quaternion.Euler(0, 0, arrowRotate);
            }

            // Do attack
            if (Input.GetKeyDown(KeyCode.J) && PlayerState.IsHeadThrown == false)
            {

                headPhysics.SetActive(true);
                headRender.SetActive(false);

                headPhysics.GetComponent<Rigidbody2D>().velocity = new Vector2(powerX, powerY);

                PlayerState.IsHeadThrown = true;

                obj = null;

                // Cooldown
                yield return new WaitForSeconds(1.8f);
            }
        }
            
    }

    IEnumerator AttackFunction()
    {
        PlayerState.IsAttack = true;
        AttackTrigger("Normal-Attack");
        yield return new WaitForSeconds(0.5f);
        AttackTrigger("Reset");

        PlayerState.IsAttack = false;
        attackRoutine = null;
    }
    
    IEnumerator GroundPoundFunc()
    {
        PlayerState.IsPound = true;
        playerRB.gravityScale = 0;
        AttackTrigger("Pound");
        PlayerCollision("Idle");

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
        PlayerState.IsPound = false;

        yield return new WaitForSeconds(0.3f);
        AttackTrigger("Reset");
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

        PlayerState.IsDash = true;
        AttackTrigger("Normal-Attack");
        yield return new WaitForSeconds(0.4f);
        AttackTrigger("Reset");
        PlayerState.IsDash = false;
        dirX = 0;

        dashRoutine = null;

    }

    IEnumerator JumpFunction()
    {
        dirY = jumpPower;
        PlayerState.IsJump = true;
        yield return new WaitForSeconds(0.4f);
        PlayerState.IsJump = false;

        if (PlayerState.IsDoubleJump == false)
        { 
            dirY = 0;
        }

        jumpRoutine = null;

    }

    IEnumerator DoubleJumpFunc()
    {
        dirY = 0;
        yield return new WaitForSeconds(0.1f);
        dirY = jumpPower;
        PlayerState.IsDoubleJump = true;
        yield return new WaitForSeconds(0.4f);
        PlayerState.IsDoubleJump = false;
        dirY = 0;

        doubleJumpRoutine = null;
    }

    IEnumerator AttackJumpFunc()
    {
        PlayerState.IsAttackJump = true;
        dirY = attackJumpPower;
        AttackTrigger("Jump-Attack");
        yield return new WaitForSeconds(0.4f);
        AttackTrigger("Reset");
        dirY = 0;
        PlayerState.IsAttackJump = false;

        attackJumpRoutine = null;
    }

    IEnumerator SlideFunction()
    {
        // THERE'S A BUG
        while (PlayerState.IsSlide == true || PlayerState.IsCrouch == true)
        {
            
            speed -= 0.045f;
            if (speed < 0)
            {
                speed = walkSpeed - 2;
                PlayerState.IsSlide = false;

            }
            yield return new WaitForSeconds(0.001f);
        }

        PlayerState.IsSlide = false;
        speed = oldSpeed;
        // Collider Changes
        PlayerCollision("Idle");

        slideRoutine = null;
    }
}
