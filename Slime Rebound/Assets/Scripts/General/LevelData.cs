using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    //
    public static List<SaveableObjects> _saveableObj_Tutorial;

    public static List<SaveableObjects> SaveableObj_Tutorial
    {
        get { return _saveableObj_Tutorial; }
        set { _saveableObj_Tutorial = value; }
    }
    //
    public static List<SaveableObjects> _saveableObj_Tutorial1;

    public static List<SaveableObjects> SaveableObj_Tutorial1
    {
        get { return _saveableObj_Tutorial1; }
        set { _saveableObj_Tutorial1 = value; }
    }
    //
    public static List<SaveableObjects> _saveableObj_Tutorial2;

    public static List<SaveableObjects> SaveableObj_Tutorial2
    {
        get { return _saveableObj_Tutorial2; }
        set { _saveableObj_Tutorial2 = value; }
    }
}
