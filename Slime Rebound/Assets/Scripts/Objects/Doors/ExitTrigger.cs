using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : Singleton<ExitTrigger>
{

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
                GameData.Tutorial_HighScore = GameData.Score;
                break;

            case "ForestLevel":
                GameData.Tutorial_HighScore = GameData.Score;
                break;

        }
        SaveLoadManager.Instance.SaveImportantData();
    }
}
