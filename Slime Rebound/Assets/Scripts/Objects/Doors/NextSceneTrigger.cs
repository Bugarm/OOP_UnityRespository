using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneTrigger : MonoBehaviour
{
    public int switchRoomNum;
    public int id;

    private bool delay;

    Coroutine delayDoorRoutine;

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
        if (collision.CompareTag("PlayerBody") && delay == false && GameData.HasEnteredDoor == false)
        {
            GameData.HasSceneTransAnim = true;
            GameData.SceneTransID = id;

            GameData.HasEnteredDoor = false;
            GameData.HasEnteredScreneTrig = true;

            SaveLoadManager.Instance.SavePlayerTransData();

            SceneSwitchManager.Instance.SwitchRoom(switchRoomNum);

            if (delayDoorRoutine == null)
            {
                delayDoorRoutine = StartCoroutine(DelayDoorEnter());
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
