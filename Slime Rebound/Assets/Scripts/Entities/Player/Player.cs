using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : Singleton<Player>
{
    
    [Header("Settings")]
    public int walkSpeed;
    public int runSpeed;
    public int bounceSpeed;
    public int dashPower;
    public int amountOfJumps;

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

    private GameObject player;
    private Rigidbody2D playerRB;

    private float dirX;
    private float dirY;
    private float jumpPower;
    private int jumpTimes;
    private float attackJumpPower;
    private float speed = 4;

    public int failedBounces = 0;

    // Collision Check
    private PhysicsMaterial2D curMaterial;

    private bool offLedge;
    private bool disableMove;

    private Coroutine ledgeDelayRoutine, jumpRoutine, dashRoutine, poundRoutine, attackRoutine, attackJumpRoutine, headAttackRoutine, bounceRoutine, slideRoutine, stickModeRoutine;

    protected override void Awake()
    {
        base.Awake();

        player = this.gameObject;
        playerRB = GetComponent<Rigidbody2D>();

        throwArrow.SetActive(false);
        
        jumpPower = 9.5f;
        attackJumpPower = 10.4f;
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    private void OnEnable()
    {
        PlayerCollision("Idle");
        AttackTrigger("Reset");
    }

    bool isMoving;
    // Update is called once per frame
    void Update()
    {

        //Debug.Log(GameData.HasSceneTransAnim);
        if (PlayerState.IsDamaged == true)
        {
            dirX = 0; dirY = 0;
        }
        else
        {
            if(PlayerState.DisableAllMove == false)
            { 
                KeyPressed();
                ResetChecks();
                KeyReleased();
            }
            PlayerAcceleration();
            
            if (PlayerState.IsBounceMode == true)
            {
                playerRB.velocity = new Vector3(dirX, playerRB.velocity.y, 0);
            }
            else
            {
                playerRB.velocity = new Vector3(dirX, dirY, 0);
                //Debug.Log(dirX);
            }
            
        }

        if (PlayerState.IsPound == false && PlayerState.IsSlide == false && PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false && PlayerState.IsDamaged == false && PlayerState.IsCrouch == false)
        {
            if (PlayerState.IsTouchingGround == false && playerRB.velocity.y == 0)
            {
                PlayerAnimationManager.Instance.PlayAnimation("jump");
            }
        }

        // Stops velocity when screen is transitioning
        if(GameData.HasSceneTransAnim == true)
        {
            // Left and Right
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            if(isMoving == false)
            {
                dirX = 0;
                speed = walkSpeed;
                PlayerState.IsCrouch = false;
            }

        }
        //Debug.Log(PlayerState.IsStickActive);
    }

    public Vector2 RetrurnPos()
    {
        Vector2 vel = new Vector2 (dirX, dirY);

        return vel;
    }

    public void ResetPlayerVel()
    {
        dirX = 0;
        dirY = 0;
    }

    void ResetChecks()
    {
        // Disable when stick active Touches Ground
        if (PlayerState.IsStickActive == true && PlayerState.IsTouchingGround == true)
        {
            if(stickModeRoutine != null)
            { 
                StopCoroutine(stickModeRoutine);
                stickModeRoutine = null;
            }

            dirX = 0;
            playerRB.sharedMaterial = slip;
            PlayerState.IsStickActive = false;
            

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

            playerRB.sharedMaterial = slip;
            playerRB.gravityScale = 10; // DO smn different asp
            //Reset velocity
            dirX = 0;
        }

        // Allows to stick while bounce
        if(Input.GetKeyDown(KeyCode.L) && (PlayerState.IsBounceMode == true && PlayerState.IsTouchingWall == true))
        {
            PlayerState.IsBounceMode = false;
        }

        //

    }

    void PlayerAcceleration()
    {
        if((PlayerState.IsPound == false && PlayerState.IsAttackJump == false && PlayerState.IsJump == false))
        {

            // Always Apply Gravity
            if (PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false)
            {
                if (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true)
                {
                    // Reset Velocity when it hits ground
                    dirY = 0;
                }
                else 
                {
                    // Player falls down faster every frame
                    dirY -= 0.15f;
                } 
            }
            // Gravity Delay
            else if (PlayerState.IsBounceMode == true)
            {
                if(playerRB.velocity.y < -3.7f)
                {
                    if (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true)
                    {
                        // Reset Velocity when it hits ground
                        playerRB.velocity = new Vector2(playerRB.velocity.x, 0f);
                    }
                    else if ((playerRB.velocity.y < 0f) && PlayerState.IsTouchingWall == false && !(playerRB.velocity.y < -9f))
                    {
                        playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y - 0.15f);
                    }
                }
            }
            
            
            else if (PlayerState.IsStickActive == true)
            {
                
                if ((PlayerState.IsTouchingGround == false || PlayerState.IsTouchingPlatform == false) && PlayerState.IsTouchingWall == true)
                {
                    // Reset Velocity when it hits ground
                    dirY = 0;
                }
                else if ((playerRB.velocity.y < 0f) && PlayerState.IsTouchingWall == false && !(playerRB.velocity.y < -9f))
                {
                    dirY -= 0.15f;
                }
                
            }

        }

    }

    void KeyPressed()
    {
        if (GameData.HasSceneTransAnim == false && Time.timeScale != 0f) // FOR THE PAUSE MENU
        {
            if (PlayerState.IsStickActive == false && PlayerState.IsDash == false && PlayerState.IsPound == false && PlayerState.IsBounceMode == false)
            {
                // Flips Player
                if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
                {
                    player.transform.localScale = new Vector2(player.transform.localScale.x, player.transform.localScale.y);
                }
                else
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
        }

        if (disableMove == false)
        {
            // Run
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && PlayerState.IsPound == false)
        {
            PlayerState.IsRun = true;

            if (PlayerState.IsCrouch == false && PlayerState.IsSlide == false)
            {
                speed = runSpeed;
            }

        }

            // Crouch & Slide & Jump Attack
            if (Input.GetKey(KeyCode.S) && PlayerState.IsHeadAttack == false && PlayerState.IsBounceMode == false)
        {
            if (PlayerState.IsStickActive == false && PlayerState.IsTouchingGround == true)
            {

                // Collider Changes
                PlayerCollision("Crouch");

                if (PlayerState.IsAttackJump == false)
                { 
                    PlayerAnimationManager.Instance.PlayAnimation("crouch");
                }

                wallCollision.offset = new Vector2(0.4836431f, -0.55f);

                // Slide
                if (PlayerState.IsRun == true)
                {
                    PlayerState.IsSlide = true;

                    speed -= Time.deltaTime + 0.025f;

                    // Reset back to crouch when sliding has depleted
                    if(speed <= 0)
                    {
                        PlayerState.IsRun = false;
                        PlayerState.IsSlide = false;
                    }

                }
                
                if (PlayerState.IsSlide == false)
                {
                    PlayerState.IsSlide = false;
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

            //Normal movement
            if (PlayerState.IsStickActive == false && PlayerState.IsDash == false && PlayerState.IsBounceMode == false)
            {
                // Left and Right
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) )
                {
                    dirX = 0;
                    PlayerAnimationManager.Instance.PlayAnimation("idle");

                }
                else if (Input.GetKey(KeyCode.A) && PlayerState.IsTouchingWall == false)
                {
                    PlayerState.IsMove = true;
                    if (PlayerState.IsCrouch == false || PlayerState.IsRun == false)
                    {
                        dirX = -speed;
                    }

                }
                else if (Input.GetKey(KeyCode.D) && PlayerState.IsTouchingWall == false)
                {

                    PlayerState.IsMove = true;
                    if (PlayerState.IsCrouch == false || PlayerState.IsRun == false)
                    {
                        dirX = speed;
                    }

                }

                
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) )
                {
                    if(PlayerState.IsJump == false && PlayerState.IsPound == false && PlayerState.IsCrouch == false 
                        && PlayerState.IsSlide == false && PlayerState.IsHeadAttack == false 
                        && PlayerState.IsAttackJump == false)
                    { 
                        PlayerAnimationManager.Instance.PlayAnimation("walk");
                    }
                }
                


            }

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && attackJumpRoutine == null && PlayerState.IsTouchingTop == false && PlayerState.IsPound == false)
            {
                if (ledgeDelayRoutine == null)
                {
                    ledgeDelayRoutine = StartCoroutine(LedgeDelay());
                }

                if (offLedge == true && PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false)
                {
                    if (jumpRoutine == null && jumpTimes <= amountOfJumps)
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
                else if (PlayerState.IsBounceMode == true && PlayerState.IsStickActive == false)
                {
                    if (jumpRoutine == null)
                    {
                        jumpRoutine = StartCoroutine(JumpFunction());
                    }
                }
            }

        }
        
        // Stick to wall mode
        if (Input.GetKeyDown(KeyCode.L) && PlayerState.IsCrouch == false && PlayerState.IsHeadAttack == false && PlayerState.IsBounceMode == false)
        {
            if(PlayerState.IsTouchingWall == true)
            { 
                if (stickModeRoutine == null)
                {
                    stickModeRoutine = StartCoroutine(StickVelocity());
                }

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

        // Bounce Setup 
        if (Input.GetKey(KeyCode.K) && PlayerState.IsCrouch == false && PlayerState.IsPound == false && PlayerState.IsStickActive == false && PlayerState.IsJump == false)
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
        
        // Ground Pound
        if (Input.GetKeyDown(KeyCode.S) && PlayerState.IsHeadAttack == false)
        {
            if (PlayerState.IsStickActive == false || (PlayerState.IsStickActive == true && PlayerState.IsTouchingWall == false))
            {
                if ((PlayerState.IsTouchingGround == false && PlayerState.IsTouchingPlatform == false))
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
            if(PlayerState.IsTouchingGround == true && PlayerState.IsBounceMode == false && PlayerState.IsCrouch == false && PlayerState.IsPound == false && PlayerState.IsDash == false)
            { 
                if (headAttackRoutine == null)
                {
                    headAttackRoutine = StartCoroutine(HeadThrow());
                }
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.J) && PlayerState.IsHeadAttack == false && PlayerState.IsAttackJump == false)
        {
            // Dash
            if (PlayerState.IsMove == true && PlayerState.IsCrouch == false && playerRB.velocity.x != 0f)
            {
                if (dashRoutine == null)
                {
                    dashRoutine = StartCoroutine(DashFunction());
                }
            }

            //Attack
            if (attackRoutine == null && dashRoutine == null && playerRB.velocity.y == 0f)
            {
                attackRoutine = StartCoroutine(AttackFunction());
            }
        }

    }
    
    void KeyReleased()
    {
        // Run Release
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
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
            //Debug.Log(PlayerState.IsRun);   
        }
        
        // Movement Release
        if (PlayerState.IsStickActive == false && PlayerState.IsBounceMode == false)
        {
            // Movement
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                PlayerState.IsMove = false;
                dirX = 0;
            }

            // Crouch   
            if (!Input.GetKey(KeyCode.S))
            {
                
                if ((PlayerState.IsCrouch == true || PlayerState.IsSlide == true) )
                {
                    if(PlayerState.IsTouchingTop == false)
                    { 
                        PlayerState.IsSlide = false;
                        PlayerState.IsCrouch = false;
                    
                        // Collider Changes
                        PlayerCollision("Idle");

                        speed = walkSpeed;
                    
                        wallCollision.offset = new Vector2(0.4836431f, -0.2430587f);
                    }
                }
                
            }


        }

        // Bounce Release
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

                crouchCollider.gameObject.SetActive(false);
                bounceCollider.gameObject.SetActive(false);

                break;

            case "Crouch":
                crouchCollider.gameObject.SetActive(true);

                idleCollider.gameObject.SetActive(false);
                bounceCollider.gameObject.SetActive(false);

                break;

            case "Bounce":
                bounceCollider.gameObject.SetActive(true);

                idleCollider.gameObject.SetActive(false);
                crouchCollider.gameObject.SetActive(false);

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

                break;

            case "Jump-Attack":
                poundTrigger.gameObject.SetActive(false);
                bounceAttackTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(true);

                break;

            case "Bounce":
                poundTrigger.gameObject.SetActive(false);
                bounceAttackTrigger.gameObject.SetActive(true);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                break;

            case "Pound":
                poundTrigger.gameObject.SetActive(true);
                bounceAttackTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                break;

            case "Reset":
                poundTrigger.gameObject.SetActive(false);
                bounceAttackTrigger.gameObject.SetActive(false);
                attackTrigger.gameObject.SetActive(false);
                jumpAttackTrigger.gameObject.SetActive(false);

                break;

            default:
                Debug.Log("ERROR");
                break;

        }
    }

    // Routines

    // Jumping before falling off the ledge
    IEnumerator LedgeDelay()
    {

        if (PlayerState.IsTouchingGround == false || PlayerState.IsTouchingPlatform == false)
        {
            offLedge = true;
        }
        else
        {
            offLedge = false;
        }

        if(jumpTimes <= 1)
        { 
            yield return new WaitForSeconds(0.5f);
        }

        ledgeDelayRoutine = null;
    }

    IEnumerator StickVelocity()
    {
        float timer = 0;
        jumpRoutine = null;
        PlayerState.IsStickActive = true;

        while (PlayerState.IsStickActive == true && PlayerState.IsPound == false && PlayerState.IsTouchingGround == false)
        {
            if (player.transform.localScale.x == -1)
            {
                dirX = -7.5f;
            }
            else if (player.transform.localScale.x == 1)
            {
                dirX = 7.5f;
            }

            yield return new WaitForSeconds(0.02f);

            // Check incase player gets stuck in a corner while in stick mode
            if (PlayerState.IsTouchingGround == false && PlayerState.IsTouchingWall == false)
            { 
                timer += 0.01f;
                if(timer > 0.5f)
                {
                    PlayerState.IsStickActive = false;
                }
            }
            else
            {
                timer = 0;
            }
        }

        PlayerState.IsStickActive = false;
        dirX = 0;
        playerRB.sharedMaterial = slip;
        stickModeRoutine = null;
    }

    IEnumerator BounceFunction()
    {
        float oldPos = playerSprite.transform.position.x;
        int bounces = 0;
        failedBounces = 0;
        bool hasFailed = false;
        float extraRunSpeed = 0;

        PlayerCollision("Bounce");
        AttackTrigger("Bounce");

        // 
        if (PlayerState.IsBounceMode == true)
        {
            wallCollision.offset = new Vector2(0.72f, -0.2430587f);

            GameManager.Instance.UpdateBounces(bounces, false);

            PlayerCollision("Bounce");
            AttackTrigger("Bounce");

            playerRB.sharedMaterial = bounce;
            playerRB.gravityScale = 1.7f;
            playerRB.velocity = new Vector2(playerRB.velocity.x,2f);

            if (GameManager.Instance.OutbounceUIRoutine != null)
            {
                StopCoroutine(GameManager.Instance.OutbounceUIRoutine);
                GameManager.Instance.OutbounceUIRoutine = null;
            }

            if (GameManager.Instance.InbounceUIRoutine == null)
            {
                GameManager.Instance.InbounceUIRoutine = StartCoroutine(GameManager.Instance.FadeInBounceUI());
            }
        }

        while (PlayerState.IsBounceMode == true && PlayerState.IsPound == false)
        {
            yield return new WaitForSeconds(0.001f);

            PlayerAnimationManager.Instance.PlayAnimation("bounce");

            extraRunSpeed = PlayerState.IsRun == true ? 1.5f : 0;

            // Increases bounce moving speed when holding run
            dirX = player.transform.localScale.x == 1 ? bounceSpeed + extraRunSpeed : -bounceSpeed - extraRunSpeed;

            if (PlayerState.IsTouchingWall == true)
            {
                PlayerAnimationManager.Instance.PlayAnimation("squish");

                player.transform.localScale = new Vector2(-(Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);

                if (PlayerState.IsJump == true)
                {
                    oldPos = playerSprite.transform.position.x;
                    failedBounces = 0;
                    bounces++;
                    GameManager.Instance.UpdateBounces(bounces,false);

                    Vector3 look = player.transform.localScale.x == 1 ? Vector3.right : Vector3.left;

                    StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleSlimeSplash, new Vector2(player.transform.position.x, player.transform.position.y + 0.6f), Quaternion.LookRotation(look)));

                    playerRB.velocity = new Vector3(playerRB.velocity.x, 12.3f, 0);
                }
                else
                {
                    failedBounces++;
                }

                yield return new WaitForSeconds(0.020f);
            }

            // Prevents Unneccery Bouncing
            if(PlayerState.IsTouchingTop == true)
            {
                playerRB.velocity = new Vector3(playerRB.velocity.x, -0.1f, 0);
            }

            // Stops when player does enough failed wall bounces
            if (failedBounces >= 3)
            {
                GameManager.Instance.UpdateBounces(bounces, true);
                PlayerState.IsBounceMode = false;
                hasFailed = true;
            }
        }

        // Cooldown
        if (PlayerState.IsBounceMode == false)
        {
            PlayerAnimationManager.Instance.PlayAnimation("idle");

            StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleSlimeBoom, playerSprite.transform.position, Quaternion.identity));

            wallCollision.offset = new Vector2(0.4836431f, -0.2430587f);

            if (GameManager.Instance.InbounceUIRoutine != null)
            { 
                StopCoroutine(GameManager.Instance.InbounceUIRoutine);
                GameManager.Instance.InbounceUIRoutine = null;
            }

            if (GameManager.Instance.OutbounceUIRoutine == null)
            {
                GameManager.Instance.OutbounceUIRoutine = StartCoroutine(GameManager.Instance.FadeOutBounceUI());
            }

            if(hasFailed == false || GameData.TotalBounces < bounces)
            { 
                GameManager.Instance.BouncesTotalCounted(bounces,false);
            }
            else
            {
                GameManager.Instance.BouncesTotalCounted(bounces, true);
            }

            PlayerCollision("Idle");
            AttackTrigger("Reset");

            playerRB.sharedMaterial = slip;
            playerRB.gravityScale = 10;
            //Reset velocity
            dirX = 0;

            yield return new WaitForSeconds(1.5f);

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

        jumpPower = 7f;

        PlayerAnimationManager.Instance.PlayAnimation("head");

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
        PlayerAnimationManager.Instance.PlayAnimation("attack");
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
        PlayerAnimationManager.Instance.PlayAnimation("pound");


        // Air Time
        dirX = 0;
        dirY = 0;

        speed = walkSpeed / 2;

        disableMove = true;
        yield return new WaitForSeconds(0.5f);
        disableMove = false;

        dirY = -13.5f;
        yield return new WaitUntil(() => PlayerState.IsTouchingGround == true);
        disableMove = true;

        StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleSlimeSplash, new Vector2(player.transform.position.x, player.transform.position.y - 0.6f), Quaternion.LookRotation(Vector3.down)));
        playerRB.gravityScale = 10;
        speed = walkSpeed;
        PlayerState.IsPound = false;

        yield return new WaitForSeconds(0.3f);
        AttackTrigger("Reset");

        disableMove = false;
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
        PlayerAnimationManager.Instance.PlayAnimation("dash");
        yield return new WaitForSeconds(0.4f);
        AttackTrigger("Reset");

        PlayerState.IsDash = false;
        dirX = 0;
        yield return new WaitForSeconds(0.4f);
        dashRoutine = null;

    }

    IEnumerator JumpFunction()
    {
        
        if(PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false)
        { 
            jumpTimes++;
            PlayerAnimationManager.Instance.PlayAnimation("jump");
        }

        dirY = jumpPower;
        PlayerState.IsJump = true;
        yield return new WaitForSeconds(0.20f);
        PlayerState.IsJump = false;
        dirY = 0;

        // For extra jumps in mid air
        
        if (jumpTimes >= amountOfJumps && PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false)
        {
            yield return new WaitUntil(() => PlayerState.IsTouchingGround == true);
            jumpTimes = 0;
            jumpRoutine = null;

            if (PlayerState.IsHeadAttack == false)
            {
                PlayerAnimationManager.Instance.PlayAnimation("idle");
            }
        }
        else
        {
            jumpRoutine = null;
            yield return new WaitUntil(() => PlayerState.IsTouchingGround == true);

            if (PlayerState.IsHeadAttack == false && PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false)
            {
                PlayerAnimationManager.Instance.PlayAnimation("idle");
            }

        }
        
    }

    IEnumerator AttackJumpFunc()
    {
        PlayerAnimationManager.Instance.PlayAnimation("crouchAttack");
        PlayerState.IsAttackJump = true;
        dirY = attackJumpPower;
        
        AttackTrigger("Jump-Attack");
        yield return new WaitForSeconds(0.4f);
        AttackTrigger("Reset");
        dirY = 0;
        PlayerState.IsAttackJump = false;

        yield return new WaitUntil(() => PlayerState.IsTouchingGround == true);

        
        attackJumpRoutine = null;
    }

}
