using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    private static bool _isJump, _isDoubleJump, _isDash, _isSlide, _isMove, _isRun, _isCrouch, _isPound, _isAttack, _isAttackJump, _isHeadAttack, _isHeadThrown;

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
