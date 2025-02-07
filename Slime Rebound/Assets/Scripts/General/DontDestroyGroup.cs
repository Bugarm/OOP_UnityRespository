using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public float offset;
    float savePlayerVel;
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }

    // IMP
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHP = 10;

        SaveLoadManager.Instance.SaveDataCheckPoint(player.transform.position);
        SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);

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

        DoorSpawnIn();
        StartCoroutine(DelayLevelDataLoad(SceneManager.GetActiveScene().name));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Load Scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        //Debug.Log(savePlayerVel);

        if (scene.name.StartsWith("TutorialRoom") || scene.name.StartsWith("ForestLevel"))
        {
            exitDoor = GameObject.FindGameObjectWithTag("ExitDoor");
            exitTrigger = GameObject.FindGameObjectWithTag("ExitTrigger");
            
            StartCoroutine(DelayLevelDataLoad(scene.name));

            LevelExitDoor.Instance.DestroyDoorCheck(exitDoor, exitTrigger);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // So the data can load in time


    IEnumerator DelayLevelDataLoad(string sceneName)
    {
        yield return new WaitForSeconds(0.01f);
        DoorSpawnIn();

        bool hasObjSpawned = HasObjectsSpawnedOnce(sceneName);

        yield return new WaitForSeconds(0.01f);

        if (hasObjSpawned == false)
        {
            SaveLoadManager.Instance.LoadLevelData(sceneName);
        }

    }


    // Data Related
    public bool HasObjectsSpawnedOnce(string sceneName)
    {
        bool checkedScene = false;
        spawner = GameObject.FindObjectsOfType<PrefabSpawner>();

        if (sceneLoaded.Count > 0)
        {
            foreach (string curScene in sceneLoaded)
            {
                if (curScene == SceneManager.GetActiveScene().name)
                {
                    checkedScene = true;
                }
            }

            if (checkedScene == false)
            {
                LoadObjects(sceneName);
                return true;
            }
            return false;
        }
        else
        {
            LoadObjects(sceneName);
            return true;
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
        GameObject doorStart = GameObject.FindGameObjectWithTag("DoorStart");
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
                player.transform.position = new Vector3(doorStart.transform.position.x, doorStart.transform.position.y - 0.4f, 0);
                GameData.PlayerPos = new Vector3(doorStart.transform.position.x, doorStart.transform.position.y - 0.4f, 0);
            }

            // Reset Player Velocity on Scene Loaded
            Player.Instance.ResetPlayerVel("stop");

            PlayerState.IsRun = false;
            PlayerState.IsBounceMode = false;
            PlayerState.IsStickActive = false;
            PlayerState.IsHeadAttack = false;
        }

        else if (GameData.HasEnteredDoor == false && GameData.HasEnteredScreneTrig == true)
        {
            //Debug.Log(GameData.SceneTransID);

            foreach (GameObject sceneTrig in sceneTrigger)
            {
                NextSceneTrigger trig = sceneTrig.GetComponent<NextSceneTrigger>();

                if (GameData.SceneTransID == trig.id)
                {

                    if (player.transform.localScale.x == 1)
                    {
                        offset = 3;
                    }
                    else
                    {
                        offset = -3;
                    }

                    player.transform.position = new Vector3(sceneTrig.transform.position.x + offset, sceneTrig.transform.position.y - 0.4f, 0);
                    GameData.PlayerPos = new Vector3(sceneTrig.transform.position.x + offset, sceneTrig.transform.position.y - 0.4f, 0);
                }
            }

        }
    }
}
