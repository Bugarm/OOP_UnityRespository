using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyHUB : Singleton<DontDestroyHUB>
{
    private Scene sceneStart;

    public GameObject player;
    private int maxHP;
    string sceneName;

    private List<string> sceneLoaded = new List<string>();
    private PrefabSpawner[] spawner;

    public GameObject exitDoor;
    public GameObject exitTrigger;

    private GameObject[] sceneTrigger;

    private float offset;

    private bool doOnce;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }

    // Scene Loading Managers
    private void OnEnable()
    {
        //Debug.Log("Enable");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        //Debug.Log("Disable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.HasEnteredDoor = false;
        GameData.HasLevelDoor = false;

        GameObject doorStart = GameObject.FindGameObjectWithTag("DoorStart");
        GameObject doorlevel = GameObject.FindGameObjectWithTag("LevelDoor");
        // First first door in the hierarchy (IMP)
        DoorRoomSwitch doorScript = GameObject.FindFirstObjectByType<DoorRoomSwitch>();

        if (doorStart != null)
        {
            player.transform.position = new Vector3(doorStart.transform.position.x + 0.8f, doorStart.transform.position.y - 0.4f, 0);
            GameData.PlayerPos = new Vector3(doorStart.transform.position.x + 0.8f, doorStart.transform.position.y - 0.4f, 0);
        }
        else if(doorScript != null)
        {
            GameObject door = doorScript.gameObject;

            player.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
            GameData.PlayerPos = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
        }
        else if(doorlevel != null)
        {
            player.transform.position = new Vector3(doorlevel.transform.position.x, doorlevel.transform.position.y - 0.4f, 0);
            GameData.PlayerPos = new Vector3(doorlevel.transform.position.x, doorlevel.transform.position.y - 0.4f, 0);
        }
        PlayerState.DisableAllMove = false;

        SaveLoadManager.Instance.LoadImportantData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveLoadManager.Instance.LoadImportantData();

        if (scene.name.StartsWith("HUB"))
        {
            StartCoroutine(DelayLevelDataLoad());
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator DelayLevelDataLoad()
    {
        yield return new WaitUntil(() => player.activeInHierarchy == true);
        DoorSpawnIn();
    }
    public void DoorSpawnIn()
    {
        player = FindAnyObjectByType<Player>().gameObject;
        // Spawns in the starting Door
        GameObject[] doorStart = GameObject.FindGameObjectsWithTag("Doors");
        sceneTrigger = GameObject.FindGameObjectsWithTag("SceneTrigger");

        //Debug.Log(GameData.HasEnteredDoor + " " + GameData.HasEnteredScreneTrig);

        if (GameData.HasEnteredDoor == true && GameData.HasEnteredScreneTrig == false)
        {

            if (doorStart == null)
            {
                player.transform.position = new Vector3(0, 0, 0);
                GameData.PlayerPos = new Vector3(0, 0, 0);
            }
            else
            {
                foreach (GameObject door in doorStart)
                {
                    DoorRoomSwitch doorID = door.GetComponent<DoorRoomSwitch>();

                    if (GameData.DoorID == doorID.id)
                    {
                        player.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
                        GameData.PlayerPos = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
                        GameData.HasEnteredDoor = false;

                    }
                }
            }

            // Reset Player Velocity on Scene Loaded
            Player.Instance.ResetPlayerVel();

            PlayerState.IsRun = false;
            PlayerState.IsBounceMode = false;
            PlayerState.IsStickActive = false;
            PlayerState.IsHeadAttack = false;
            PlayerState.IsHeadThrown = false;
        }

        else if (GameData.HasEnteredDoor == false && GameData.HasEnteredScreneTrig == true)
        {
            //Debug.Log(GameData.SceneTransID);

            foreach (GameObject sceneTrig in sceneTrigger)
            {
                NextSceneTrigger trig = sceneTrig.GetComponent<NextSceneTrigger>();
                if (GameData.SceneTransID == trig.id)
                {
                    if (trig.transform.localScale.x == -1)
                    {
                        offset = 3;
                    }
                    else
                    {
                        offset = -3;
                    }

                    player.transform.position = new Vector3(sceneTrig.transform.position.x + offset, sceneTrig.transform.position.y - 0.4f, 0);
                    GameData.PlayerPos = new Vector3(sceneTrig.transform.position.x + offset, sceneTrig.transform.position.y - 0.4f, 0);
                    GameData.HasEnteredScreneTrig = false;
                }
            }

        }
    }


}
