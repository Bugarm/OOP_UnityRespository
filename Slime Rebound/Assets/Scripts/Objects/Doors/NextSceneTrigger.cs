using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneTrigger : MonoBehaviour
{
    public int switchRoomNum;
    public int id;

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
        if (collision.CompareTag("PlayerBody") && GameData.HasEnteredScreneTrig == false)
        {
            
            PlayerState.DisableAllMove = true;
            GameData.SceneTransID = id;

            GameData.HasEnteredDoor = false;
            GameData.HasEnteredScreneTrig = true;

            SaveLoadManager.Instance.SavePlayerTransData();

            SceneSwitchManager.Instance.SwitchRoom(switchRoomNum);
        }
    }
}
