using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Required
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

public class PlayerAnimationManager : Singleton<PlayerAnimationManager>
{
    private Animator playerAnim;

    protected override void Awake()
    {
        base.Awake();

        playerAnim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        PlayAnimation("idle");
    }

    // Update is called once per frame
    void Update()
    {
        
        if(PlayerState.IsStickActive == false && PlayerState.IsBounceMode == false && PlayerState.IsHeadAttack == false)
        {
            // Idle Anim
            if(PlayerState.IsCrouch == false && PlayerState.IsSlide == false) 
            {                
                if(PlayerState.IsTouchingWall == true || PlayerState.IsMove == false)
                {
                    PlayAnimation("idle");
                }
                else
                {
                    PlayAnimation("walk");
                }
            }

            // Jump
            if (PlayerState.IsTouchingGround == false && PlayerState.IsTouchingPlatform == false && PlayerState.IsTouchingPlatformSide == false)
            {
                PlayAnimation("jump");
            }

            if (PlayerState.IsCrouch == true || PlayerState.IsSlide == true)
            {
                PlayAnimation("crouch");
            }

            if(PlayerState.IsDash == true)
            {
                PlayAnimation("dash");
            }
            
            if (PlayerState.IsAttackJump == true && PlayerState.IsTouchingGround == false)
            {
                PlayAnimation("crouchAttack");
            }

            if(PlayerState.IsPound == true)
            {
                PlayAnimation("pound");
            }

            if(PlayerState.IsAttack == true)
            {
                PlayAnimation("attack");
            }
        }

        // Stick
        if (PlayerState.IsStickActive == true && PlayerState.IsBounceMode == false && PlayerState.IsHeadAttack == false)
        {
            if(PlayerState.IsTouchingWall == true && PlayerState.IsTouchingGround == false && PlayerState.IsTouchingPlatform == false) 
            {
                PlayAnimation("stick");
            }
            else if(PlayerState.IsJump == true)
            {
                PlayAnimation("jump");
            }
        }

        // Bouncing
        if(PlayerState.IsBounceMode == true)
        {
            if(PlayerState.IsTouchingWall == true)
            {
                PlayAnimation("squish");
            }
            else
            {
                PlayAnimation("bounce");
            }
            
        }

        if(PlayerState.IsHeadAttack == true)
        {
            PlayAnimation("head");
        }

        // Ouch
        if (PlayerState.IsDamaged == true)
        {
            PlayAnimation("damaged");
        }

        
    }

    public void PlayAnimation(string mode)
    {
        switch(mode)
        {
            case "idle":
                playerAnim.SetBool("IsIdle", true);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsDamaged", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsStick", false);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", false);
                playerAnim.SetBool("IsSquish", false);

                break;

            case "walk":
                playerAnim.SetBool("IsWalking", true);

                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsDamaged", false);
                playerAnim.SetBool("IsJump", false);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", false);
                playerAnim.SetBool("IsStick", false);
                break;

            case "crouch":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", true);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsWalking", false);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", false);
                break;

            case "crouchAttack":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsCrouchAttack", true);
                playerAnim.SetBool("IsPound", false);
                break;

            case "bounce":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsBounce", true);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsWalking", false);


                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", false);
                playerAnim.SetBool("IsSquish", false);


                break;

            case "jump":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsJump", true);
                playerAnim.SetBool("IsStick", false);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", false);

                break;

            case "stick":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsDamaged", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsStick", true);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", false);
                break;

            case "pound":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsPound", true);
                playerAnim.SetBool("IsStick", false);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", false);
                break;

            case "attack":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsDamaged", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsStick", false);

                playerAnim.SetBool("IsAttack", true);
                playerAnim.SetBool("IsDashAttack", false);
                break;

            case "dash":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsDamaged", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsStick", false);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", true);
                break;

            case "head":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsDamaged", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsStick", false);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", true);
                break;

            case "squish":
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsSquish", true);

                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsDamaged", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsStick", false);

                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);

                playerAnim.SetBool("IsHeadAttack", false);

                break;  

            case "damaged":
                playerAnim.SetBool("IsIdle", false);
                playerAnim.SetBool("IsWalking", false);
                playerAnim.SetBool("IsCrouch", false);
                playerAnim.SetBool("IsCrouchAttack", false);
                playerAnim.SetBool("IsStick", false);
                playerAnim.SetBool("IsBounce", false);
                playerAnim.SetBool("IsJump", false);
                playerAnim.SetBool("IsDamaged", true);
                playerAnim.SetBool("IsPound", false);
                playerAnim.SetBool("IsAttack", false);
                playerAnim.SetBool("IsDashAttack", false);
                playerAnim.SetBool("IsHeadAttack", false);
                playerAnim.SetBool("IsSquish", false);

                break;
        }
    }
}
