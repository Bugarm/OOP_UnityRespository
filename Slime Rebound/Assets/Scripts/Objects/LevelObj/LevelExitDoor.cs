using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitDoor : Singleton<LevelExitDoor>
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

    public void DestroyDoorCheck(GameObject exitDoor, GameObject exitTrigger)
    {


        bool isExitDoor = CheckForDoor(exitDoor,exitTrigger);

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

    public bool CheckForDoor(GameObject exitDoor, GameObject exitTrigger)
    {

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
