using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelData 
{

    // Tutorial
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

    public static List<SaveableObjects> _saveableObj_Tutorial3;

    public static List<SaveableObjects> SaveableObj_Tutorial3
    {
        get { return _saveableObj_Tutorial3; }
        set { _saveableObj_Tutorial3 = value; }
    }

    // Forest Level
    public static List<SaveableObjects> _saveableObj_ForestLevel;

    public static List<SaveableObjects> SaveableObj_ForestLevel
    {
        get { return _saveableObj_ForestLevel; }
        set { _saveableObj_ForestLevel = value; }
    }

    public static List<SaveableObjects> _saveableObj_ForestLevel1;

    public static List<SaveableObjects> SaveableObj_ForestLevel1
    {
        get { return _saveableObj_ForestLevel1; }
        set { _saveableObj_ForestLevel1 = value; }
    }
}
