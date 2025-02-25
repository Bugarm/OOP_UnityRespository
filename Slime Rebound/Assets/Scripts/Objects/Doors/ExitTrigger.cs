using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : Singleton<ExitTrigger>
{

    protected override void Awake()
    {
        base.Awake();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBody"))
        {
            CheckStateAndSave(GameData.LevelState);
            SceneSwitchManager.Instance.SwitchToLevel("HUB");
        }
    }

    private void CheckStateAndSave(string scene)
    {
        switch (scene)
        {
            case "TutorialRoom":
                if (GameData.Score > GameData.Tutorial_HighScore)
                {
                    GameData.Tutorial_HighScore = GameData.Score;
                    SaveLoadManager.Instance.SaveImportantData();
                }
                break;

            case "ForestLevel":
                if (GameData.Score > GameData.Level1_HighScore)
                {
                    GameData.Level1_HighScore = GameData.Score;
                    SaveLoadManager.Instance.SaveImportantData();
                }
                break;

        }
    }
}
