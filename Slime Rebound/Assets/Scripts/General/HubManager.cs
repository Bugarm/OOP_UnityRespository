using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : Singleton<HubManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        BackgroundScroll.Instance.ResetBackGroundPos();

        GameData.HasSceneTransAnim = false;
        GameData.HasEnteredDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SceneTransFunct(int? nextRoomNum)
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
        this.gameObject.GetComponentInParent<DontDestroyGroup>().enabled = true;
    }

}
