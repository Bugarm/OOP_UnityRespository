using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : Door
{
    public string level;
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                SceneSwitchManager.Instance.SwitchToLevel(level);   
            }
        }
    }
}
