using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorRoomSwitch : Door
{
    public int switchRoomNum;
    public int id;

    public bool hasEntered;
    private bool delay;

    Coroutine delayDoorRoutine;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAtDoor == true && delay == false && GameData.HasEnteredScreneTrig == false)
        {
            if (Input.GetKeyDown(KeyCode.W) && Player.Instance.RetrurnPos().x == 0 && PlayerState.IsHeadAttack == false)
            { 
                GameData.DoorID = id;

                GameManager.Instance.sceneSwitch = true;
                GameData.HasEnteredDoor = true;
                GameData.HasEnteredScreneTrig = false;

                SceneSwitchManager.Instance.SwitchRoom(switchRoomNum);
                
                if(delayDoorRoutine == null)
                { 
                    delayDoorRoutine = StartCoroutine(DelayDoorEnter());
                }
            }
        }
    }

    IEnumerator DelayDoorEnter()
    {
        delay = true;
        yield return new WaitForSeconds(1);
        delay = false;
        delayDoorRoutine = null;
    }
}
