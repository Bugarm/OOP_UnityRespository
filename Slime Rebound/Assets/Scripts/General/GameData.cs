using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameData : MonoBehaviour
{
    private static float _padding = 0f;
    private static int _hp = 0;
    private static int _score = 0;
    private static string _levelState = "";

    public static int Hp
    {
        get { return _hp; }

        set { _hp = value; }
    }

    public static int Score
    { 
        get { return _score; } 
    
        set { _score = value; }
    }

    public static string LevelState
    {
        get { return _levelState; }

        set { _levelState = value; }
    }

    public static float XMax
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + _padding;
        }
    }

    public static float XMin
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + _padding;
        }
    }

    public static float YMax
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + _padding;
        }
    }

    public static float YMin
    {
        get
        {
            return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + _padding;
        }
    }
}
