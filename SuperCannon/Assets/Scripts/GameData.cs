using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameData : MonoBehaviour
{
    private static float _padding = 0f;
    private static int _score;
 

    public static int Score
    {
        get { return _score; }

        set { _score = value; }
    }

    public static Vector3 MousePos
    {
        get { return GetMousePos(); }

    }

    static Vector3 GetMousePos()
    {
        Camera cam = Camera.main;
        Vector3 _mousePos = cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, 10f);
        return _mousePos;
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

    //    XMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
    //    XMax = camera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
    //    YMin = camera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
    //    YMax = camera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;


}