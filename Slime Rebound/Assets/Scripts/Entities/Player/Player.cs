using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : Singleton<Player>
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
    public PhysicsMaterial2D platform;

    [Header("Player Collider")]
    public CapsuleCollider2D idleCollider;
    public BoxCollider2D crouchCollider;
    public CircleCollider2D bounceCollider;

    [Header("Player Trigger")]
    public CapsuleCollider2D idleTrigger;
    public BoxCollider2D crouchTrigger;
    public CircleCollider2D bounceTrigger;

    [Header("Player Attack Trigger")]
    public BoxCollider2D attackTrigger;
    public BoxCollider2D poundTrigger;
    public CircleCollider2D bounceAttackTrigger;
    public BoxCollider2D jumpAttackTrigger;

    [Header("Player Head")]
    public GameObject headAttack;
    public int headGravityPower;
    public GameObject throwArrow;
    public GameObject playerHeadStorage;

    [Header("Wall Collision")]
    public BoxCollider2D wallCollision;

    [Header("Player Sprite")]
    public SpriteRenderer playerSprite;

    [Header("Remove Later")]
    public Sprite idle;
    public Sprite crouch;

    private GameObject player;
    private Rigidbody2D playerRB;

    private float dirX;
    private float dirY;
    private float newxPos;
    private float jumpPower;
    private float attackJumpPower;
    private float speed;

    // Collision Check
    private PhysicsMaterial2D curMaterial;

    private float oldSpeed;

    private Coroutine jumpRoutine, doubleJumpRoutine, dashRoutine, poundRoutine, attackRoutine, attackJumpRoutine, headAttackRoutine, bounceRoutine, slideRoutine;

    protected override void Awake()
    {
        base.Awake();

        player = this.gameObject;
        playerRB = GetComponent<Rigidbody2D>();

        throwArrow.SetActive(false);

        dirX = 0;
        dirY = 0;
        jumpPower = 6.45f;
        attackJumpPower = 10.4f;

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
        // Check on this later
        curMaterial = PlayerState.IsTouchingPlatform == true ? platform : slip;
        
        if(PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false)
        { 
            if(PlayerState.IsTouchingPlatform == true)
            {
                playerRB.sharedMaterial = platform;
            }
            else
            {
                playerRB.sharedMaterial = slip;
            }
        }

        if (PlayerState.IsDamaged == false)
        {
            KeyPressed();
            KeyReleased();
            ResetChecks();
        
            AnimationController();
            PlayerAcceleration();

            if(PlayerState.IsBounceMode == true)
            {
                playerRB.velocity = new Vector3(dirX, playerRB.velocity.y, 0);
            }
            else
            {
                playerRB.velocity = new Vector3(dirX, dirY, 0);
            }
        }
        else
        {
            dirX = 0; dirY = 0;
        }
    }

    public void ResetPlayerVel()
    {
        dirX = 0;
        dirY = 0;
        

        PlayerState.IsRun = false;
        PlayerState.IsBounceMode = false;
        PlayerState.IsStickActive = false;
        PlayerState.IsHeadAttack = false;
    }

    void ResetChecks()
    {
        // Disable when stick active Touches Ground
        if (PlayerState.IsStickActive == true && PlayerState.IsTouchingGround == true)
        {
            PlayerState.IsStickActive = false;
            dirX = 0;
            playerRB.sharedMaterial = curMaterial;
        }

        // Disable Jump when groundpound
        if (PlayerState.IsJump == true && PlayerState.IsPound == true)
        {
            StopCoroutine(JumpFunction());
            jumpRoutine = null;
        }

        // Allows Ground pound when bounce mode
        if(PlayerState.IsBounceMode == true && PlayerState.IsPound == true)
        {
            PlayerState.IsBounceMode = false;

            playerRB.sharedMaterial = curMaterial;
            playerRB.gravityScale = 10; // DO smn different asp
            //Reset velocity
            dirX = 0;
        }

        // Allows to stick while bounce
        if(Input.GetKeyDown(KeyCode.L) && (PlayerState.IsBounceMode == true && PlayerState.IsTouchingWall == true))
        {
            PlayerState.IsBounceMode = false;
        }

    }

    void PlayerAcceleration()
    {
        if((PlayerState.IsPound == false && PlayerState.IsAttackJump == false && PlayerState.IsDoubleJump == false && PlayerState.IsJump == false))
        {
            // Always Apply Gravity
            if(PlayerState.IsBounceMode == false || PlayerState.IsStickActive == true)
            {
                if ((playerRB.velocity.y < 0f) && PlayerState.IsTouchingWall == false)
                {
                    // Player falls down faster every frame
                    dirY -= 0.15f;
                }
                
                if(PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true)
                {
                    // Reset Velocity when it hits ground
                    dirY = 0;
                }
            }
            // Gravity Delay
            else if (PlayerState.IsBounceMode == true && PlayerState.IsTouchingWall == false)
            {
                if(playerRB.velocity.y < -3.7f)
                {  
                    if((PlayerState.IsTouchingGround == true))
                    {
                        playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
                    }
                    else
                    {
                        playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y - 0.15f);
                    }
                }
            }
        }

    }

    void AnimationController() // Remove Later
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerSprite.sprite = crouch;
        }

        if (!Input.GetKey(KeyCode.S) && PlayerState.IsTouchingTop == false)
        {
            playerSprite.sprite = idle;
        }
    }

    void KeyPressed()
    {
        // Stick to wall switch
        if (Input.GetKeyDown(KeyCode.L) && PlayerState.IsCrouch == false && PlayerState.IsHeadAttack == false)
        {
            if (PlayerState.IsBounceMode == false)
            {
                PlayerState.IsStickActive = !PlayerState.IsStickActive;
                if (PlayerState.IsStickActive == true)
                {
                    playerRB.sharedMaterial = stick;
                }
                else
                {
                    playerRB.sharedMaterial = curMaterial;
                }

            }
        }

        // Bounce Setup // Do a check where it only goes faster when it checks Y velocity when bounce mode
        if (Input.GetKey(KeyCode.K) && PlayerState.IsCrouch == false && PlayerState.IsStickActive == false && PlayerState.IsJump == false)
        {
            if (Input.GetKeyDown(KeyCode.K))
            { 
              PlayerState.IsBounceMode = true;
            }

            if (bounceRoutine == null)
            {
                bounceRoutine = StartCoroutine(BounceFunction());
            }
        }

        // Run
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && PlayerState.IsCrouch == false && PlayerState.IsSlide == false)
        {
            PlayerState.IsRun = true;
            speed = runSpeed;
        }

        //Normal movement
        if (PlayerState.IsStickActive == false && PlayerState.IsDash == false && PlayerState.IsPound == false && PlayerState.IsBounceMode == false)
        {
            if (Input.GetKey(KeyCode.A) && PlayerState.IsTouchingWall == false)
            {
                PlayerState.IsMove = true;
                dirX = -speed;
                

            }
            if (Input.GetKey(KeyCode.D) && PlayerState.IsTouchingWall == false)
            {
                PlayerState.IsMove = true;
                dirX = speed;

            }

            if (Time.timeScale != 0f)
            { 
                if (Input.GetKey(KeyCode.A))
                {
                    player.transform.localScale = new Vector2(-1, player.transform.localScale.y);
                }

                if (Input.GetKey(KeyCode.D))
                {
                    player.transform.localScale = new Vector2(1, player.transform.localScale.y);
                }
            }

            

            

        }

        // Stick velocity
        if (PlayerState.IsStickActive == true && PlayerState.IsPound == false && PlayerState.IsHeadAttack == false)
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
        if (Input.GetKey(KeyCode.S) && (PlayerState.IsCrouch == true || PlayerState.IsSlide == true))
        {
            if (speed<= 0)
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
        if (Input.GetKey(KeyCode.S) && PlayerState.IsHeadAttack == false && PlayerState.IsBounceMode == false)
        {
            if (PlayerState.IsStickActive == false && PlayerState.IsTouchingGround == true)
            {

                // Collider Changes
                PlayerCollision("Crouch");

                wallCollision.offset = new Vector2(0.4836431f, -0.55f);

                // Slide
                if (PlayerState.IsRun == true)
                {
                    PlayerState.IsSlide = true;

                    speed -= 0.025f;
                    if (speed < 0)
                    {
                        speed = walkSpeed - 2;

                    }
                }
                // Normal Crouch Speed
                else if(PlayerState.IsSlide == false)
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
            if (PlayerState.IsStickActive == false || (PlayerState.IsStickActive == true && PlayerState.IsTouchingWall == false))
            {
                if ((PlayerState.IsTouchingGround == false && PlayerState.IsTouchingPlatform == false) && PlayerState.IsAttackJump == false)
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
            if(PlayerState.IsTouchingGround == true && PlayerState.IsBounceMode == false && PlayerState.IsCrouch == false && PlayerState.IsPound == false && PlayerState.IsDash == false && PlayerState.IsDoubleJump == false)
            { 
                if (headAttackRoutine == null)
                {
                    headAttackRoutine = StartCoroutine(HeadThrow());
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.J) && PlayerState.IsHeadAttack == false && PlayerState.IsAttackJump == false && playerRB.velocity.y == 0f)
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
        if (Input.GetKeyDown(KeyCode.Space) && PlayerState.IsJump == false && PlayerState.IsAttackJump == false && PlayerState.IsTouchingTop == false)
        {
            if (PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false && (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true) && PlayerState.IsJump == false)
            {
                if (jumpRoutine == null)
                {
                    jumpRoutine = StartCoroutine(JumpFunction());
                }
            }
            // Sticky Wall Jump
            else if (PlayerState.IsBounceMode == false && PlayerState.IsStickActive == true && PlayerState.IsTouchingWall == true && PlayerState.IsCrouch == false)
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
            // Bounce Jump
            else if(PlayerState.IsBounceMode == true) 
            {
                if (jumpRoutine == null)
                {
                    jumpRoutine = StartCoroutine(JumpFunction());
                }
            }
        }
        
        // Double Jump
        else if (Input.GetKeyDown(KeyCode.Space) && PlayerState.IsAttackJump == false && PlayerState.IsStickActive == false && PlayerState.IsHeadAttack == false && PlayerState.IsTouchingTop == false)
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
        if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) )
        {
            PlayerState.IsRun = false;
            if (PlayerState.IsCrouch == false && PlayerState.IsSlide == false)
            {
                speed = walkSpeed;
            }
            else
            {
                speed = walkSpeed - 2;
            }
            
            
        }

        if (PlayerState.IsStickActive == false && PlayerState.IsBounceMode == false)
        { 
            // Movement
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                PlayerState.IsMove = false;
                dirX = 0;
            }

            // Crouch (do this better)
            if ((PlayerState.IsCrouch == true || PlayerState.IsSlide == true) && PlayerState.IsTouchingTop == false && (!Input.GetKey(KeyCode.S) || (Input.GetKeyUp(KeyCode.S)) ))
            {
                PlayerState.IsSlide = false;
                PlayerState.IsCrouch = false;

                speed = oldSpeed;
                // Collider Changes
                PlayerCollision("Idle");

                wallCollision.offset = new Vector2(0.4836431f, -0.2430587f);
            }
        }

        if (Input.GetKeyUp(KeyCode.K) && PlayerState.IsHeadAttack == false && PlayerState.IsCrouch == false && PlayerState.IsStickActive == false)
        {
            PlayerState.IsBounceMode = false;
        }
    }

    // Collision and Triggers Setup
    void PlayerCollision(string mode)
    {
        switch (mode)
        {
            case "Idle":
                idleCollider.gameObject.SetActive(true);
                idleTrigger.gameObject.SetActive(true);

                crouchCollider.gameObject.SetActive(false);
                bounceCollider.gameObject.SetActive(false);

                // Trigger
                crouchTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);

                break;

            case "Crouch":
                crouchCollider.gameObject.SetActive(true);
                crouchTrigger.gameObject.SetActive(true);

                idleCollider.gameObject.SetActive(false);
                bounceCollider.gameObject.SetActive(false);

                // Trigger
                idleTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);

                break;

            case "Bounce":
                bounceCollider.gameObject.SetActive(true);
                bounceTrigger.gameObject.SetActive(true);

                idleCollider.gameObject.SetActive(false);
                crouchCollider.gameObject.SetActive(false);

                // Trigger
                idleTrigger.gameObject.SetActive(false);
                crouchTrigger.gameObject.SetActive(false);

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
                bounceAttackTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(true);
                jumpAttackTrigger.gameObject.SetActive(false);

                // Triggers
                idleTrigger.gameObject.SetActive(false);
                crouchTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);
                break;

            case "Head-Attack":
                // WIP

                break;

            case "Jump-Attack":
                poundTrigger.gameObject.SetActive(false);
                bounceAttackTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(true);

                // Triggers
                idleTrigger.gameObject.SetActive(false);
                crouchTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);
                break;

            case "Bounce":
                poundTrigger.gameObject.SetActive(false);
                bounceAttackTrigger.gameObject.SetActive(true);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                // Triggers
                idleTrigger.gameObject.SetActive(false);
                crouchTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);
                break;

            case "Pound":
                poundTrigger.gameObject.SetActive(true);
                bounceAttackTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                // Triggers
                idleTrigger.gameObject.SetActive(false);
                crouchTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);
                break;

            case "Reset":
                poundTrigger.gameObject.SetActive(false);
                bounceAttackTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                // Triggers
                idleTrigger.gameObject.SetActive(true);
                crouchTrigger.gameObject.SetActive(false);
                bounceTrigger.gameObject.SetActive(false);

                break;

            default:
                Debug.Log("ERROR");
                break;

        }
    }

    // Routines
    IEnumerator BounceFunction()
    {
        int bounces = 0;
        float extraRunSpeed = 0;

        PlayerCollision("Bounce");
        AttackTrigger("Bounce");

        // 
        if (PlayerState.IsBounceMode == true)
        {
            PlayerCollision("Bounce");
            AttackTrigger("Bounce");

            playerRB.sharedMaterial = bounce;
            playerRB.gravityScale = 1.7f;
            playerRB.velocity = new Vector2(playerRB.velocity.x,2f);
        }

        while (PlayerState.IsBounceMode == true)
        {
            yield return new WaitForSeconds(0.001f);


            if (PlayerState.IsRun == true)
            {
                extraRunSpeed = 1.5f;
            }
            else
            {
                extraRunSpeed = 0;
            }

            if (player.transform.localScale.x == -1)
            {
                dirX = -bounceSpeed - extraRunSpeed;
            }
            else if (player.transform.localScale.x == 1)
            {
                dirX = bounceSpeed + extraRunSpeed;
            }


            if (PlayerState.IsTouchingWall == true)
            {
                bounces++;
                GameManager.Instance.DisplayBounces(bounces);

                player.transform.localScale = new Vector2(-(Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);

                if (PlayerState.IsJump == true)
                { 
                    playerRB.velocity = new Vector3(playerRB.velocity.x, 12.3f, 0);
                }
                yield return new WaitForSeconds(0.020f);

            }

            // Prevents Unneccery Bouncing
            if(PlayerState.IsTouchingTop == true)
            {
                playerRB.velocity = new Vector3(playerRB.velocity.x, 0f, 0);
            }

        }

        // Cooldown
        if (PlayerState.IsBounceMode == false)
        {
            GameManager.Instance.BouncesTotalCounted(bounces);

            PlayerCollision("Idle");
            AttackTrigger("Reset");

            playerRB.sharedMaterial = curMaterial;
            playerRB.gravityScale = 10;
            //Reset velocity
            dirX = 0;
            bounces = 0;
            GameManager.Instance.DisplayBounces(bounces);
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
            if(PlayerState.IsHeadThrown == false)
            { 
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
            }
            // Create Head Obj
            if (obj == null && PlayerState.IsHeadThrown == false)
            {
                obj = Instantiate(headAttack, player.transform.position, Quaternion.identity);
                obj.transform.SetParent(playerHeadStorage.transform);
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
                throwArrow.SetActive(false);

                headPhysics.SetActive(true);
                headRender.SetActive(false);

                headPhysics.GetComponent<Rigidbody2D>().velocity = new Vector2(powerX, powerY);

                PlayerState.IsHeadThrown = true;

                obj = null;

                // Cooldown
                yield return new WaitForSeconds(1.8f);

                throwArrow.SetActive(true);
            }

            // Cancel Attack
            if (PlayerState.IsHeadThrown == false && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.L)))
            {
                PlayerState.IsHeadAttack = false;
                PlayerState.IsHeadThrown = false;
                Destroy(obj);

                jumpPower = oldJumpPower;
                throwArrow.SetActive(false);
                headAttackRoutine = null;
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

        while (PlayerState.IsTouchingGround == false)
        {
            dirY = -13.5f;
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
        if (PlayerState.IsBounceMode == false)
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
        else
        {
            // Doesn't take double jump and jump power
            PlayerState.IsJump = true;
            yield return new WaitForSeconds(0.3f);
            PlayerState.IsJump = false;

            jumpRoutine = null;
        }
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

}
