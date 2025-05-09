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

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAtDoor == true && GameData.DoorDelay == false && GameData.HasEnteredScreneTrig == false)
        {
            if (Input.GetKeyDown(KeyCode.W) && PlayerState.IsMove == false && PlayerState.IsHeadAttack == false)
            {
                StartCoroutine(AudioManager.Instance.PlaySFXManual(doorOpenSFX,this.gameObject.transform.position));
                PlayerState.DisableAllMove = true;
                GameData.DoorID = id;

                GameManager.Instance.sceneSwitch = true;
                GameData.HasEnteredDoor = true;
                GameData.HasEnteredScreneTrig = false;

                SceneSwitchManager.Instance.SwitchRoom(switchRoomNum);
                
                
            }
        }
    }

    
}
