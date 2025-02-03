using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyGroup : Singleton<DontDestroyGroup>
{
    private Scene sceneStart;

    public GameObject player;
    private int maxHP;

    private List<string> sceneLoaded = new List<string>();
    private PrefabSpawner[] spawner;


    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHP = 10;

        SceneManager.sceneLoaded += OnSceneLoaded;

        SaveLoadManager.Instance.SaveDataCheckPoint(player.transform.position);
        SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);

        sceneStart = SceneManager.GetActiveScene();

        GameData.LevelState = sceneStart.name;

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

        if (scene.name != "HUB" && scene.name != "MainMenu" && scene.name != "OptionScreen")
        {
            DoorSpawnIn();

            StartCoroutine(DelayLevelDataLoad(scene.name));
        }
    }

    // So the data can load in time
    void DoorSpawnIn()
    {
        
        // Spawns in the starting Door
        GameObject doorStart = GameObject.FindGameObjectWithTag("DoorStart");

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
        Player.Instance.ResetPlayerVel();
    }

    IEnumerator DelayLevelDataLoad(string sceneName)
    {
        yield return new WaitForSeconds(0.02f);
        
        bool hasObjSpawned = HasObjectsSpawnedOnce(sceneName);

        yield return new WaitForSeconds(0.01f);

        if (hasObjSpawned == false)
        {
            SaveLoadManager.Instance.LoadLevelData(sceneName);
        }

        yield return new WaitForSeconds(0.01f);
        LevelExitDoor.Instance.DestroyDoorCheck();
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

}
