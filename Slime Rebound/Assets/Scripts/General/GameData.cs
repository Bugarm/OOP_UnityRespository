using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameData : MonoBehaviour
{
    private static float _padding = 0f;
    private static int _hp = 0;
    private static int _score = 0;
    private static string _levelState = "";
    private static int _chainsInLevel;
    private static bool _isPaused;
    private static int _totalBounces = 0;
    private static Vector3 _playerPos;
    private static int _sceneTransID;
    private static int _doorID = 0;
    private static bool _hasSceneTransAnim;

    private static bool _hasEnteredDoor = true;
    private static bool _hasEnteredScreneTrig = false;
    private static bool _hasLevelDoor = false;

    private static bool _doorDelay=false;


    // High Score Varibles

    private static int _tutorial_HighScore;
    private static int _level1_HighScore;

    // 

    public static bool DoorDelay
    {
        get { return _doorDelay; }
        set { _doorDelay = value; }
    }

    public static bool HasEnteredDoor
    {
        get { return _hasEnteredDoor; } 
        set { _hasEnteredDoor = value; }
    }

    public static bool HasLevelDoor
    {
        get { return _hasLevelDoor; }
        set { _hasLevelDoor = value; }
    }

    public static bool HasEnteredScreneTrig
    {
        get { return _hasEnteredScreneTrig; }
        set { _hasEnteredScreneTrig = value; }
    }

    // Door ID

    public static int DoorID
    {
        get { return _doorID; }
        set { _doorID = value; }
    }

    // Scene ID

    public static int SceneTransID
    {
        get { return _sceneTransID; }
        set { _sceneTransID = value; }
    }

    // High Scores
    public static int Tutorial_HighScore
    {
        get { return _tutorial_HighScore; }
        set { _tutorial_HighScore = value; }
    }

    public static int Level1_HighScore
    {
        get { return _level1_HighScore; }
        set { _level1_HighScore = value; }
    }

    //
    public static int TotalBounces
    {
        get { return _totalBounces; }
        set { _totalBounces = value; }
    }

    public static Vector3 PlayerPos
    {
        get { return _playerPos; }
        set { _playerPos = value; }
    }

    public static bool IsPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }

    public static int ChainsInLevel
    {
        get { return _chainsInLevel; }
        set { _chainsInLevel = value; }
    }

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
