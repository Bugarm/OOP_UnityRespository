using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitDoor : Singleton<LevelExitDoor>
{
    GameObject exitDoor;
    GameObject exitTrigger;
    protected override void Awake()
    {
        base.Awake();

        exitDoor = GameObject.FindGameObjectWithTag("ExitDoor");
        exitTrigger = GameObject.FindGameObjectWithTag("ExitTrigger");
    }

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

        Debug.Log(exitTrigger);

        bool isExitDoor = CheckForDoor();

        if (isExitDoor == true)
        {
            
            if (GameData.ChainsInLevel <= 0)
            {
                exitDoor.SetActive(false);
                exitTrigger.SetActive(true);
            }
            else
            {
                exitDoor.SetActive(true);
                exitTrigger.SetActive(false);
            }
        }
    }

    public bool CheckForDoor()
    {
        GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor");
        GameObject exitTrigger = GameObject.FindGameObjectWithTag("ExitTrigger");

        if (exitDoor == null && exitTrigger == null)
        {
            return false;
        }
        else
        { 
            return true;
        }
    }
}
