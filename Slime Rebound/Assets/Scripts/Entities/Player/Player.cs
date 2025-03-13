using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
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
    private float speed = 4;

    public int failedBounces = 0;

    private bool offLedge;
    private bool startFallAttack;

    private Coroutine ledgeDelayRoutine, slideRoutine, jumpRoutine, dashRoutine, poundRoutine, attackRoutine, attackJumpRoutine, headAttackRoutine, bounceRoutine, stickModeRoutine;

    protected override void Awake()
    {
        base.Awake();
        player = this.gameObject;
        playerRB = GetComponent<Rigidbody2D>();

        throwArrow.SetActive(false);
        
        jumpPower = 9.4f;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerState.IsMove = false;
        PlayerState.IsBounceMode = false;
        PlayerState.IsHeadAttack = false;
        PlayerState.IsStickActive = false;
        PlayerState.IsDamaged = false;
        PlayerState.DisableAllMove = false;
        PlayerState.IsTouchingPlatform = false;
        PlayerState.IsPound = false;
    }

    private void OnEnable()
    {
        PlayerCollision("Idle");
        AttackTrigger("Reset");
    }

    // to make it Physics smoother
    private void FixedUpdate()
    {
        // Platform Physics
        if (PlayerState.IsJump == false && PlayerState.IsDamaged == false && PlayerState.IsMove == false)
        {
            if(PlayerState.IsTouchingPlatform == true && PlayerState.IsStickActive == false)
            { 
                float platofrmPosX = DetectionScript.Instance.GetPlatform().transform.position.x;

                player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector2( platofrmPosX, player.transform.position.y), 5.5f * Time.deltaTime);
            }

            if (PlayerState.IsTouchingPlatformSide == true && PlayerState.IsStickActive == true)
            {
                float platofrmPosY = DetectionScript.Instance.GetPlatform().transform.position.y;

                player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector2(player.transform.position.x, platofrmPosY), 4.5f * Time.deltaTime);
            }

        }

        if (PlayerState.IsTouchingPlatform == false)
        {
            PlayerAcceleration();
        }

        // Player Physics
        if (PlayerState.IsDamaged == false)
        {
            // Player Movement Velocity Changes //
            playerRB.velocity = new Vector3(dirX * (Time.deltaTime * 54), dirY * (Time.deltaTime * 54), 0);
        }
        else // Damaged No Velocity
        {
            dirX = 0; 
            dirY = 0;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        if (PlayerState.DisableAllMove == false)
        { 
            KeyPressed();
            ResetChecks();
            KeyReleased();
        }

        //Debug.Log(PlayerState.IsTouchingTop);
        //Debug.Log(GameData.HasSceneTransAnim);
        //Debug.Log(PlayerState.IsTouchingGround);
    }

    public void ResetPlayerVel()
    {
        dirX = 0;
        dirY = 0;
    }

    void ResetChecks()
    {
        // Disable when stick active Touches Ground
        if (PlayerState.IsStickActive == true && (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true))
        {
            if (stickModeRoutine != null)
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

        // Bounce to dash
        if(PlayerState.IsBounceMode == true && PlayerState.IsDash == true)
        {
            PlayerState.IsBounceMode = false;

            if(bounceRoutine != null)
            {
                StopCoroutine(bounceRoutine);
                bounceRoutine = null;
            }
        }
    
        // Disable Dash if Pound
        if(PlayerState.IsPound == true && PlayerState.IsDash == true)
        {
            PlayerState.IsDash = false;
        }
    
        if(PlayerState.IsHeadAttack == true && PlayerState.IsStickActive == true)
        {
            PlayerState.IsHeadAttack = false;
        }
    }

    void PlayerAcceleration()
    {
        float fallSpeed = 0.60f;

        if(PlayerState.IsPound == false && PlayerState.IsJump == false)
        {
            // Always Apply Gravity
            if (PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false && PlayerState.IsAttackJump == false)
            {
                if (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true)
                {
                    // Reset Velocity when it hits ground
                    dirY = 0;
                }
                else if (playerRB.velocity.y > -13f)
                {
                    // Player falls down faster every frame
                    dirY -= fallSpeed;
                }
            }

            // JumpAttack
            else if(startFallAttack == true && PlayerState.IsBounceMode == false && PlayerState.IsStickActive == false)
            {
                if (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true)
                {
                    // Reset Velocity when it hits ground
                    dirY = 0;
                }
                else if (playerRB.velocity.y > -13f)
                {
                    // Player falls down faster every frame
                    dirY -= fallSpeed;
                }
            }

            // Gravity Delay
            else if (PlayerState.IsBounceMode == true)
            {
                
                if (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true)
                {
                    // Reset Velocity when it hits ground
                    dirY = 0;
                }
                
                else if (PlayerState.IsTouchingWall == false && !(playerRB.velocity.y < -9f))
                {
                    dirY -= fallSpeed;
                }
                
            }
            
            else if (PlayerState.IsStickActive == true && PlayerState.IsBounceMode == false)
            {
                if ((PlayerState.IsTouchingGround == false && PlayerState.IsTouchingPlatform == false) && PlayerState.IsTouchingWall == true)
                {
                    // Reset Velocity when it hits ground
                    dirY = 0;
                }
                
                else if ((playerRB.velocity.y < 0f) && PlayerState.IsTouchingWall == false && !(playerRB.velocity.y < -9f))
                {
                    dirY -= fallSpeed;

                }       
            }
        }
    }

    void KeyPressed()
    {
        if (Time.timeScale != 0f && PlayerState.IsDamaged == false) // FOR THE PAUSE MENU
        {
            // Player Flipping
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
        
            // Run
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if (PlayerState.IsPound == false && PlayerState.IsCrouch == false && PlayerState.IsSlide == false)
                { 
                    PlayerState.IsRun = true;
                    speed = runSpeed;
                }
            }

            // Crouch & Slide & Jump Attack
            if (Input.GetKey(KeyCode.S))
            {
                if (PlayerState.IsStickActive == false && PlayerState.IsHeadAttack == false && PlayerState.IsBounceMode == false && PlayerState.IsDash == false && (PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true))
                {
                    // Collider Changes
                    PlayerCollision("Crouch");

                    wallCollision.offset = new Vector2(0.4836431f, -0.55f);

                    if (Input.GetKeyDown(KeyCode.S) && PlayerState.IsRun == true && slideRoutine == null && PlayerState.IsCrouch == false)
                    {
                        slideRoutine = StartCoroutine(SlideFunc());
                    }

                    if (PlayerState.IsSlide == false)
                    {
                        PlayerState.IsCrouch = true;
                        speed = walkSpeed / 2;
                    }

                    // Attack Jump
                    if (Input.GetKeyDown(KeyCode.Space) && PlayerState.IsTouchingTop == false)
                    {
                        if (attackJumpRoutine == null)
                        {
                            attackJumpRoutine = StartCoroutine(AttackJumpFunc());
                        }
                    }

                }
            }

            //Normal movement
            if (PlayerState.IsStickActive == false && PlayerState.IsBounceMode == false && PlayerState.IsDash == false)
            {
                // Left and Right
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) )
                {
                    dirX = 0;
                }

                else if (Input.GetKey(KeyCode.A))
                {
                    if(PlayerState.IsTouchingWall == false)
                    { 
                        PlayerState.IsMove = true;
                        if (PlayerState.IsBounceMode == false)
                        {
                            dirX = -speed;
                        }
                    }
                }

                else if (Input.GetKey(KeyCode.D))
                {
                    if (PlayerState.IsTouchingWall == false)
                    {
                        PlayerState.IsMove = true;
                        if (PlayerState.IsBounceMode == false)
                        {
                            dirX = speed;
                        }
                    }
                }
  
            }

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(attackJumpRoutine == null && PlayerState.IsDash == false && PlayerState.IsStickActive == false && PlayerState.IsTouchingTop == false && PlayerState.IsPound == false)
                { 
                    // Ledge delay
                    if (ledgeDelayRoutine == null && jumpTimes <= 1 && PlayerState.IsBounceMode == false)
                    {
                        ledgeDelayRoutine = StartCoroutine(LedgeDelay());
                    }

                    // Jump Function
                    if (offLedge == true && PlayerState.IsBounceMode == false)
                    {
                        if (jumpRoutine == null && jumpTimes <= amountOfJumps)
                        {
                            jumpRoutine = StartCoroutine(JumpFunction());
                        }
                    }

                    // Bounce Jump
                    else if (PlayerState.IsBounceMode == true )
                    {
                        if (jumpRoutine == null)
                        {
                            jumpRoutine = StartCoroutine(JumpFunction());
                        }
                    }
                }
            }

            // Stick to wall mode
            if (Input.GetKeyDown(KeyCode.L))
            {
                
                if (PlayerState.IsTouchingWall == true && PlayerState.IsPound == false && PlayerState.IsCrouch == false 
                    && PlayerState.IsHeadAttack == false && PlayerState.IsBounceMode == false 
                    && PlayerState.IsAttackJump == false)
                {
                    PlayerState.IsStickActive = true;

                    playerRB.sharedMaterial = stick;

                    if (stickModeRoutine == null)
                    {
                        stickModeRoutine = StartCoroutine(StickVelocity());
                    }
                }
                

            }

            // Bounce Setup 
            if (Input.GetKey(KeyCode.K))
            {
                if(PlayerState.IsAttackJump == false && PlayerState.IsAttack == false && PlayerState.IsDash == false && PlayerState.IsCrouch == false && PlayerState.IsPound == false && PlayerState.IsStickActive == false && PlayerState.IsJump == false)
                { 
                    PlayerState.IsBounceMode = true;

                    if (bounceRoutine == null)
                    {
                        bounceRoutine = StartCoroutine(BounceFunction());
                    }
                }
            }
        
            // Ground Pound
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (PlayerState.IsHeadAttack == false)
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

            //   Attacks   //

            // Head Attack
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

            // Dash & Attack
            else if (Input.GetKeyDown(KeyCode.J))
            {
                if(PlayerState.IsHeadAttack == false && PlayerState.IsAttackJump == false)
                { 
                    // Dash
                    if (PlayerState.IsMove == true && PlayerState.IsTouchingWall == false && PlayerState.IsCrouch == false && playerRB.velocity.x != 0f)
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
        if (PlayerState.IsStickActive == false && PlayerState.IsBounceMode == false && PlayerState.IsDash == false)
        {
            // Movement
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                PlayerState.IsMove = false;
                speed = walkSpeed;
                PlayerState.IsCrouch = false;
                dirX = 0;
            }

            // Crouch   
            if (!Input.GetKey(KeyCode.S))
            {
                if (PlayerState.IsTouchingTop == false)
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

        // Bounce Release
        if (Input.GetKeyUp(KeyCode.K) && PlayerState.IsHeadAttack == false && PlayerState.IsCrouch == false && PlayerState.IsStickActive == false)
        {
            PlayerState.IsBounceMode = false;
        }
        
        // Left and Right
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            PlayerState.IsMove = true;
        }
        else
        {
            PlayerState.IsMove = false;
        }
        
    }
    
    // Collision and Triggers Setup //
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

    // Routines //

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

    IEnumerator SlideFunc()
    {
        PlayerState.IsSlide = true;

        while (PlayerState.IsSlide == true )
        {
            yield return new WaitForSeconds(0.01f);
            // Slide
            speed -= Time.deltaTime + 0.085f;

            // Reset back to crouch when sliding has depleted
            if (speed <= 0)
            {
                PlayerState.IsSlide = false;
                PlayerState.IsCrouch = true;
                PlayerState.IsRun = false;
            }

            yield return new WaitForFixedUpdate();

        }

        PlayerState.IsSlide = false;
        PlayerState.IsCrouch = true;
        PlayerState.IsRun = false;
        
        speed = walkSpeed / 2;

        slideRoutine = null;
    }

    IEnumerator StickVelocity()
    {
        float timer = 0;
        jumpRoutine = null;

        while (PlayerState.IsStickActive == true && PlayerState.IsPound == false && (PlayerState.IsTouchingGround == false || PlayerState.IsTouchingPlatform == false))
        {
            dirX = player.transform.localScale.x == 1 ? 7.5f : -7.5f;
            
            yield return new WaitForSeconds(0.001f);

            // Check incase player gets stuck in a corner while in stick mode
            if ((PlayerState.IsTouchingGround == false && PlayerState.IsTouchingPlatform == true) && PlayerState.IsTouchingWall == false)
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

            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && PlayerState.IsTouchingWall == true)
            {
                if (jumpRoutine == null)
                {
                    // Flip
                    player.transform.localScale = player.transform.localScale.x == -1 ? new Vector2(1, 1) : new Vector2(-1, 1);

                    jumpRoutine = StartCoroutine(JumpFunction());
                }
            }

            //Disable
            if (Input.GetKeyDown(KeyCode.L))
            {
                //dirX = 0;
                playerRB.sharedMaterial = slip;
                yield return new WaitUntil(() => PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true);
                PlayerState.IsStickActive = false;
            }

        }



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

        // Set up
        if (PlayerState.IsBounceMode == true)
        {
            wallCollision.offset = new Vector2(0.72f, -0.2430587f);

            if (GameObject.FindAnyObjectByType<GameManager>() != null)
            {
                GameManager.Instance.UpdateBounces(bounces, false);
            }

            PlayerCollision("Bounce");
            AttackTrigger("Bounce");

            playerRB.sharedMaterial = bounce;

            // Text Fade Effect
            if (GameObject.FindAnyObjectByType<GameManager>() != null)
            {
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
        }

        // Bounce Loop
        while (PlayerState.IsBounceMode == true)
        {
            yield return new WaitForSeconds(0.001f);

            extraRunSpeed = PlayerState.IsRun == true ? 1.5f : 0;

            // Increases bounce moving speed when holding run
            dirX = player.transform.localScale.x == 1 ? bounceSpeed + extraRunSpeed : -bounceSpeed - extraRunSpeed;

            // This is where it would bounces back when collding with a wall
            if ((PlayerState.IsTouchingWall == true || PlayerState.IsTouchingPlatformSide == true) && PlayerState.IsTouchingTop == false && GameData.HasEnteredScreneTrig == false)
            {
                PlayerAnimationManager.Instance.PlayAnimation("squish");

                player.transform.localScale = new Vector2(-(Mathf.Sign(playerRB.velocity.x)), player.transform.localScale.y);

                if (PlayerState.IsJump == true)
                {
                    oldPos = playerSprite.transform.position.x;
                    failedBounces = 0;
                    bounces++;

                    if (GameObject.FindAnyObjectByType<GameManager>() != null)
                    {
                        GameManager.Instance.UpdateBounces(bounces, false);
                    }

                    Vector3 look = player.transform.localScale.x == 1 ? Vector3.right : Vector3.left;

                    StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleSlimeSplash, new Vector2(player.transform.position.x, player.transform.position.y + 0.6f), Quaternion.LookRotation(look)));
                    dirY = jumpPower;
                }
                else
                {
                    failedBounces++;
                }

                yield return new WaitForSeconds(0.002f);


            }

            // Prevents Unneccery Bouncing
            if (PlayerState.IsTouchingTop == true)
            {
                dirY = 0;
            }

            // Stops when player does enough failed wall bounces
            if (failedBounces > 3)
            {
                if (GameObject.FindAnyObjectByType<GameManager>() != null)
                {
                    GameManager.Instance.UpdateBounces(bounces, true);
                }
                PlayerState.IsBounceMode = false;
                hasFailed = true;
            }

            yield return new WaitForFixedUpdate();

        }

        // Cooldown
        if (PlayerState.IsBounceMode == false)
        {
            StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleSlimeBoom, playerSprite.transform.position, Quaternion.identity));

            wallCollision.offset = new Vector2(0.4836431f, -0.2430587f);

            if (GameObject.FindAnyObjectByType<GameManager>() != null)
            {
                if (GameManager.Instance.InbounceUIRoutine != null)
                {
                    StopCoroutine(GameManager.Instance.InbounceUIRoutine);
                    GameManager.Instance.InbounceUIRoutine = null;
                }

                if (GameManager.Instance.OutbounceUIRoutine == null)
                {
                    GameManager.Instance.OutbounceUIRoutine = StartCoroutine(GameManager.Instance.FadeOutBounceUI());
                }

                if (hasFailed == false || GameData.TotalBounces < bounces)
                {
                    GameManager.Instance.BouncesTotalCounted(bounces, false);
                }
                else
                {
                    GameManager.Instance.BouncesTotalCounted(bounces, true);
                }
            }
            PlayerCollision("Idle");
            AttackTrigger("Reset");

            playerRB.sharedMaterial = slip;
            playerRB.gravityScale = 10;
            //Reset velocity
            dirX = 0;

            yield return new WaitForSeconds(1.5f);

            PlayerState.IsMove = false;
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

        // Air Time
        dirX = 0;
        dirY = 0;

        speed = walkSpeed / 2;

        PlayerState.DisableAllMove = true;
         yield return new WaitForSeconds(0.5f);
        PlayerState.DisableAllMove = false;

        dirY = -15.5f;
        yield return new WaitUntil(() => PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true || PlayerState.IsDamaged == true);
        PlayerState.DisableAllMove = true;
        
        StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleSlimeSplash, new Vector2(player.transform.position.x, player.transform.position.y - 0.6f), Quaternion.LookRotation(Vector3.down)));
        playerRB.gravityScale = 10;
        speed = walkSpeed;
        PlayerState.IsPound = false;

        dirX = 0;
        PlayerState.IsMove = false;

        yield return new WaitForSeconds(0.3f);
        AttackTrigger("Reset");

        PlayerState.DisableAllMove = false;

        poundRoutine = null;
    }

    IEnumerator DashFunction()
    {
        dirX = player.transform.localScale.x == 1 ? dashPower : -dashPower;

        PlayerState.IsDash = true;

        AttackTrigger("Normal-Attack");
        yield return new WaitForSeconds(0.4f);
        yield return new WaitUntil(() => PlayerState.IsDestroyedObj == false);
        AttackTrigger("Reset");

        PlayerState.IsDash = false;
        dirX = 0;
        yield return new WaitForSeconds(0.4f);

        dashRoutine = null;
    }

    IEnumerator JumpFunction()
    {
        float timeDelay = 0f;

        if (PlayerState.IsBounceMode == false )
        {
            if (PlayerState.IsStickActive == false)
            {

                jumpTimes++;
                //Debug.Log(jumpTimes);
                //StartCoroutine(AudioManager.Instance.PlayAudio("Pep", player.transform.position, true));
            }
   
            dirY = jumpPower;

            timeDelay = 0.20f;            
        }
        else // Bounce Settings
        {
            timeDelay = 0.30f;
        }
        

        PlayerState.IsJump = true;
        yield return new WaitForSeconds(timeDelay);
        PlayerState.IsJump = false;

        if (PlayerState.IsBounceMode == false)
        {

            dirY = 0;

            // For extra jumps in mid air
            if (jumpTimes >= amountOfJumps && PlayerState.IsStickActive == false)
            {
                yield return new WaitUntil(() => PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true);
                jumpTimes = 0;
                jumpRoutine = null;
            }
            else
            {
                jumpRoutine = null;
                yield return new WaitUntil(() => PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true);
                PlayerState.IsMove = false; // Animation Reset
                jumpTimes = 0;
            }
        }
        else
        {
            jumpRoutine = null;
        }
    }

    IEnumerator AttackJumpFunc()
    {
        PlayerState.IsAttackJump = true;
        dirY = jumpPower;
        
        AttackTrigger("Jump-Attack");
        yield return new WaitForSeconds(0.4f);
        yield return new WaitUntil(() => PlayerState.IsDestroyedObj == false);
        AttackTrigger("Reset");
        dirY = 0;
        startFallAttack = true;

        yield return new WaitUntil(() => PlayerState.IsTouchingGround == true || PlayerState.IsTouchingPlatform == true);
        StartCoroutine(ParticleSpawnerManager.Instance.PlayParticle(ParticleSpawnerManager.Instance.particleSlimeSplash, new Vector2(player.transform.position.x, player.transform.position.y - 0.6f), Quaternion.LookRotation(Vector3.down)));
        PlayerState.IsAttackJump = false;
        startFallAttack = false;

        attackJumpRoutine = null;
    }

}
