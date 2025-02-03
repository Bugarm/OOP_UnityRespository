using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchManager : Singleton<SceneSwitchManager>
{
    SerializedLevelData myLevelData = new SerializedLevelData();

    public GameObject player;

    private int totalScenes;

    private Coroutine sceneCount;

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

    public void SwitchRoom(int? nextRoomNum)
    {

        if (nextRoomNum <= 0)
        {

            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(GameData.LevelState);
        }
        else
        {
            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(GameData.LevelState + nextRoomNum);
        }

    }

    public void SwitchToLevel(string Level)
    {
        SceneManager.LoadScene(Level);

    }

    

}
