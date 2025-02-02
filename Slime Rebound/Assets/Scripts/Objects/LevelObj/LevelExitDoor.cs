using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitDoor : Singleton<LevelExitDoor>
{
    public GameObject exitTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyDoorCheck()
    {
        if(GameData.ChainsInLevel <= 0)
        { 
            this.gameObject.SetActive(false);
            exitTrigger.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(true);
            exitTrigger.SetActive(false);
        }
    }
}
