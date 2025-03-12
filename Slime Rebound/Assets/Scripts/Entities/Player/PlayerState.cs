using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    private static bool _isJump, _isDash, _isSlide, _isMove, _isRun, _isCrouch, _isPound, _isAttack, _isAttackJump, _isHeadThrown;
    private static bool _isTouchingWall, _isTouchingGround, _isTouchingPlatform, _isTouchingPlatformSide, _isTouchingTop;
    private static bool _isDamaged, _isFakeWallAllowed, _inFakeWall;
    private static bool _isBounceMode, _isHeadAttack, _isStickActive;
    private static bool _disableAllMove;
    private static bool _isDestroyObj;


    public static bool IsDestroyedObj
    {
        get { return _isDestroyObj; }
        set { _isDestroyObj = value; }
    }

    //
    public static bool DisableAllMove
    {
        get { return _disableAllMove; }
        set { _disableAllMove = value; }
    }


    // Modes

    public static bool IsBounceMode
    {
        get { return _isBounceMode; }
        set { _isBounceMode = value; }
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

    // Fake Wall
    public static bool IsFakeWallAllowed
    {
        get { return _isFakeWallAllowed; }
        set { _isFakeWallAllowed = value; }
    }

    public static bool InFakeWall
    {
        get { return _inFakeWall; }
        set { _inFakeWall = value; }
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

    public static bool IsTouchingPlatformSide
    {
        get { return _isTouchingPlatformSide; }
        set { _isTouchingPlatformSide = value; }
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
