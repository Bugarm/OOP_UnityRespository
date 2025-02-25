using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : Door
{

    public string level;
    public int id;
    public bool isBonus;
    public bool isDisableOnExit;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAtDoor == true)
        {
            if (Input.GetKeyDown(KeyCode.W) && GameData.HasLevelDoor == false)
            {
                PlayerState.DisableAllMove = true;

                GameManager.Instance.sceneSwitch = true;
                GameData.HasEnteredDoor = false;
                GameData.HasEnteredScreneTrig = false;

                GameData.SceneTransID = id;
                GameData.HasLevelDoor = true;
                SceneSwitchManager.Instance.SwitchToLevel(level);

            }
        }
    }
}
