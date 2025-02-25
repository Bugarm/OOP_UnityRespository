using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : Singleton<SceneSwitchManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void SwitchRoom(int nextRoomNum)
    {
        StartCoroutine(DontDestroyManager.Instance.ScreenTrans(true,"",nextRoomNum));        
    }

    public void SwitchToLevel(string Level)
    {
        StartCoroutine(DontDestroyManager.Instance.ScreenTrans(false,Level));
    }

}
