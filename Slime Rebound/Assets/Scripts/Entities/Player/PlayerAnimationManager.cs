using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        PlayAnimation("idle");
    }

    // Update is called once per frame
    void Update()
    {
        // Idle Anim
        if(PlayerState.IsStickActive == false && PlayerState.IsBounceMode == false && PlayerState.IsHeadAttack == false)
        { 
            if ((!Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D)) && PlayerState.IsAttack == false && PlayerState.IsDash == false && PlayerState.IsPound == false && PlayerState.IsCrouch == false && PlayerState.IsTouchingGround == true && PlayerState.IsMove == false && PlayerState.IsJump == false)
            {
                PlayAnimation("idle");
            }
            //
            
            if (PlayerState.IsTouchingWall == true && PlayerState.IsTouchingGround == false)
            {
                PlayAnimation("jump");
            }

            if (PlayerState.IsPound == true && PlayerState.IsTouchingGround == true)
            {
                PlayAnimation("idle");
            }

            if(PlayerState.IsTouchingTop == true && (PlayerState.IsCrouch == true || PlayerState.IsSlide == true))
            {
                PlayAnimation("crouch");
            }

            
        }

        if(PlayerState.IsStickActive == true && PlayerState.IsBounceMode == false && PlayerState.IsHeadAttack == false)
        {
            if(PlayerState.IsTouchingWall == true && PlayerState.IsTouchingGround == false)
            {
                PlayAnimation("stick");
            }
            else if (PlayerState.IsTouchingWall == false && PlayerState.IsTouchingGround == false && PlayerState.IsPound == false)
            {
                PlayAnimation("jump");
            }
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
