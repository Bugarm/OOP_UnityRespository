using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : Singleton<SceneSwitchManager>
{

    public GameObject player;

    private int totalScenes;

    protected override void Awake()
    {
        base.Awake();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
