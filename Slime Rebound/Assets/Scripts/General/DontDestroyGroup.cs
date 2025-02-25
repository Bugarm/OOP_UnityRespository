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
        SaveLoadManager.Instance.SaveLevelData(GameData.LevelState);

        GameObject doorStart = GameObject.FindGameObjectWithTag("DoorStart");
        // First first door in the hierarchy (IMP)
        DoorRoomSwitch doorScript = GameObject.FindFirstObjectByType<DoorRoomSwitch>();

        if (doorStart != null) 
        {
            player.transform.position = new Vector3(doorStart.transform.position.x + 0.8f, doorStart.transform.position.y - 0.4f, 0);
            GameData.PlayerPos = new Vector3(doorStart.transform.position.x + 0.8f, doorStart.transform.position.y - 0.4f, 0);
        }
        else
        {
            
            GameObject door = doorScript.gameObject;
            
            player.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
            GameData.PlayerPos = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
        }


        if (doOnce == false)
        {
            HasObjectsSpawnedOnce(SceneManager.GetActiveScene().name);

            doOnce = true;
        }

        exitDoor = GameObject.FindGameObjectWithTag("ExitDoor");
        exitTrigger = GameObject.FindGameObjectWithTag("ExitTrigger");

        PlayerState.DisableAllMove = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Load Scene
    void OnSceneLoadedLevel(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name.StartsWith("TestRoom") || scene.name.StartsWith("Bonus") || scene.name.StartsWith("TutorialRoom") || scene.name.StartsWith("ForestLevel"))
        {
            exitDoor = GameObject.FindGameObjectWithTag("ExitDoor");
            exitTrigger = GameObject.FindGameObjectWithTag("ExitTrigger");


            StartCoroutine(DelayLevelDataLoad(SceneManager.GetActiveScene().name));
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

        ExitTrigger.Instance.gameObject.SetActive(false);

        DoorSpawnIn();

        yield return new WaitForSeconds(0.1f);

        HasObjectsSpawnedOnce(sceneName);

        yield return new WaitForSeconds(0.01f);

        LevelExitDoor.Instance.DestroyDoorCheck(exitDoor, exitTrigger);

        if (hasObjSpawned == true)
        {
            SaveLoadManager.Instance.LoadLevelData(sceneName);
            //Debug.Log(sceneName);
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
        //Debug.Log(sceneName);
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
        GameObject[] levelDoorStart = GameObject.FindGameObjectsWithTag("LevelDoor");
        sceneTrigger = GameObject.FindGameObjectsWithTag("SceneTrigger");

        //Debug.Log(GameData.HasEnteredDoor + " " + GameData.HasEnteredScreneTrig);

        if (GameData.HasEnteredScreneTrig == true)
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

        else
        {
            bool enterCheck = false;

            if (levelDoorStart == null && GameData.HasLevelDoor == true)
            {
                enterCheck = true;
            }

            if (doorStart == null && GameData.HasEnteredDoor == true)
            {
                player.transform.position = new Vector3(0, 0, 0);
                GameData.PlayerPos = player.transform.position;
            }
            
            if(GameData.HasEnteredDoor == true || enterCheck == true)
            { 
                foreach (GameObject door in doorStart)
                {
                    DoorRoomSwitch doorID = door.GetComponent<DoorRoomSwitch>();

                    if (GameData.DoorID == doorID.id)
                    {
                        player.transform.position = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
                        GameData.PlayerPos = new Vector3(door.transform.position.x, door.transform.position.y - 0.4f, 0);
                        GameData.HasEnteredDoor = false;
                        enterCheck = false;


                    }
                }
            }

            if (GameData.HasLevelDoor == true)
            {
                foreach (GameObject levelDoor in levelDoorStart)
                {
                    LevelDoor lvlDoor = levelDoor.GetComponent<LevelDoor>();
                    if (GameData.SceneTransID == lvlDoor.id && lvlDoor.isBonus == true)
                    {
                        player.transform.position = new Vector3(lvlDoor.transform.position.x, lvlDoor.transform.position.y - 0.4f, 0);
                        GameData.PlayerPos = new Vector3(lvlDoor.transform.position.x, lvlDoor.transform.position.y - 0.4f, 0);
                        GameData.HasLevelDoor = false;

                        if (lvlDoor.isDisableOnExit == true)
                        {
                            // Maybe add animation here
                            lvlDoor.gameObject.SetActive(false);
                        }
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
        
    }


}
