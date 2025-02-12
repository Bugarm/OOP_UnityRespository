using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyGroup : Singleton<DontDestroyGroup>
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
        SceneManager.sceneLoaded += OnSceneLoadedLevel;
    }

    void OnDisable()
    {
        //Debug.Log("Disable");
        SceneManager.sceneLoaded -= OnSceneLoadedLevel;
    }

    

    // Start is called before the first frame update
    void Start()
    {
        maxHP = 10;

        sceneStart = SceneManager.GetActiveScene();

        // Checks if string has a digit
        if (sceneStart.name.Any(char.IsDigit))
        {
            sceneName = sceneStart.name.Substring(0, sceneStart.name.Length - 1);
        }
        else
        {
            sceneName = sceneStart.name;
        }

        GameData.LevelState = sceneName;

        SaveLoadManager.Instance.SaveLevelData(sceneName);

        GameObject doorStart = GameObject.FindGameObjectWithTag("DoorStart");
        // First first door in the hierarchy (IMP)
        DoorRoomSwitch doorScript = GameObject.FindFirstObjectByType<DoorRoomSwitch>();
        GameObject door = doorScript.gameObject;

        if (doorStart != null) 
        {
            player.transform.position = new Vector3(doorStart.transform.position.x, doorStart.transform.position.y - 0.4f, 0);
            GameData.PlayerPos = new Vector3(doorStart.transform.position.x, doorStart.transform.position.y - 0.4f, 0);
        }
        else
        {
            player.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
            GameData.PlayerPos = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
        }

        if (doOnce == false)
        {
            HasObjectsSpawnedOnce(sceneStart.name);

            doOnce = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Load Scene
    void OnSceneLoadedLevel(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name.StartsWith("TestRoom") || scene.name.StartsWith("TutorialRoom") || scene.name.StartsWith("ForestLevel"))
        {
            exitDoor = GameObject.FindGameObjectWithTag("ExitDoor");
            exitTrigger = GameObject.FindGameObjectWithTag("ExitTrigger");
            
            StartCoroutine(DelayLevelDataLoad(scene.name));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // So the data can load in time

    bool hasObjSpawned = false;
    IEnumerator DelayLevelDataLoad(string sceneName)
    {
        
        yield return new WaitUntil(() => player.activeInHierarchy == true);
        
        DoorSpawnIn();

        yield return new WaitForSeconds(0.1f);

        HasObjectsSpawnedOnce(sceneName);

        yield return new WaitForSeconds(0.01f);

        LevelExitDoor.Instance.DestroyDoorCheck(exitDoor, exitTrigger);

        if (hasObjSpawned == true)
        {
            SaveLoadManager.Instance.LoadLevelData(sceneName);
            hasObjSpawned = false;
            
        }
    }


    // Data Related
    public void HasObjectsSpawnedOnce(string sceneName)
    {
        spawner = GameObject.FindObjectsOfType<PrefabSpawner>();

        if (sceneLoaded.Count > 0)
        {
            foreach (string curScene in sceneLoaded)
            {
                if (curScene == SceneManager.GetActiveScene().name)
                {
                    hasObjSpawned = true;
                }
            }

            if (hasObjSpawned == false)
            {
                LoadObjects(sceneName);
            }
        }
        else
        {
            LoadObjects(sceneName);
        }

        //Debug.Log(sceneLoaded.Count);
    }

    private void LoadObjects(string sceneName)
    {

        foreach (PrefabSpawner spawn in spawner)
        {
            if (spawn != null)
            {
                spawn.SpawnItemOnce();
                
                if (spawn.name.StartsWith("SkeleChainSpawner") == true)
                {
                    GameData.ChainsInLevel++;
                }
            }
        }

        sceneLoaded.Add(sceneName);
        SaveLoadManager.Instance.SaveLevelData(sceneName);
    }

    public void CheckpointLoadData()
    {
        player.transform.position = GameData.PlayerPos;
        GameData.Hp = maxHP;
        GameManager.Instance.DisplayHp();
        GameManager.Instance.DisplayScore();
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
            Player.Instance.ResetPlayerVel("stop");

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
