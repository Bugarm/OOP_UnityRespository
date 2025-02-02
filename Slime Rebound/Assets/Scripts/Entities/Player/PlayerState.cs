using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    private static bool _isJump, _isDoubleJump, _isDash, _isSlide, _isMove, _isRun, _isCrouch, _isPound, _isAttack, _isAttackJump, _isHeadAttack, _isHeadThrown;
    private static bool _isTouchingWall, _isTouchingGround, _isTouchingPlatform, _isTouchingTop;
    private static bool _isDamaged;
    private static bool _isBounceMode, _isHeadAttackMode, _isStickActive;

    // Modes

    public static bool IsBounceMode
    {
        get { return _isBounceMode; }
        set { _isBounceMode = value; }
    }

    public static bool IsHeadAttackMode
    {
        get { return _isHeadAttackMode; }
        set { _isHeadAttackMode = value; }
    }

    public static bool IsStickActive
    {
        get { return _isStickActive; }
        set { _isStickActive = value; }
    }


    // Damaged
    public static bool IsDamaged
    {
        get { return _isDamaged; }
        set { _isDamaged = value; }
    }

    // Collision
    public static bool IsTouchingWall
    {
        get { return _isTouchingWall; }
        set { _isTouchingWall = value; }
    }

    public static bool IsTouchingGround
    {
        get { return _isTouchingGround; }
        set { _isTouchingGround = value; }
    }

    public static bool IsTouchingPlatform
    {
        get { return _isTouchingPlatform; }
        set { _isTouchingPlatform = value; }
    }

    public static bool IsTouchingTop
    {
        get { return _isTouchingTop; }
        set { _isTouchingTop = value; }
    }

    // Bools
    public static bool IsJump
    {
        get { return _isJump; }
        set { _isJump = value; }
    }

    public static bool IsDoubleJump
    {
        get { return _isDoubleJump; }
        set { _isDoubleJump = value; }
    }

    public static bool IsDash
    {
        get { return _isDash; }
        set { _isDash = value; }
    }

    public static bool IsSlide
    {
        get { return _isSlide; }
        set { _isSlide = value; }
    }

    public static bool IsMove
    {
        get { return _isMove; }
        set { _isMove = value; }
    }

    public static bool IsRun
    {
        get { return _isRun; }
        set { _isRun = value; }
    }

    public static bool IsCrouch
    {
        get { return _isCrouch; }
        set { _isCrouch = value; }
    }

    public static bool IsPound
    {
        get { return _isPound; }
        set { _isPound = value; }
    }

    public static bool IsAttack
    {
        get { return _isAttack; }
        set { _isAttack = value; }
    }

    public static bool IsAttackJump
    {
        get { return _isAttackJump; }
        set { _isAttackJump = value; }
    }

    public static bool IsHeadAttack
    {
        get { return _isHeadAttack; }
        set { _isHeadAttack = value; }
    }

    public static bool IsHeadThrown
    {
        get { return _isHeadThrown; }
        set { _isHeadThrown = value; }
    }

}
