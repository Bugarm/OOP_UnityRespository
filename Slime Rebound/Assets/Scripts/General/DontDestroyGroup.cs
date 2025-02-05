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

        // Checks if string has a digit
        if(sceneStart.name.Any(char.IsDigit))
        { 
            sceneName = sceneStart.name.Substring(0, sceneStart.name.Length - 1);
        }
        else
        { 
            sceneName = sceneStart.name;
        }

        GameData.LevelState = sceneName;

        DontDestroyManager.Instance.DoorSpawnIn();
        StartCoroutine(DelayLevelDataLoad(SceneManager.GetActiveScene().name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Load Scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name.StartsWith("TutorialRoom") || scene.name.StartsWith("ForestLevel"))
        {
            exitDoor = GameObject.FindGameObjectWithTag("ExitDoor");
            exitTrigger = GameObject.FindGameObjectWithTag("ExitTrigger");

            DontDestroyManager.Instance.DoorSpawnIn();

            StartCoroutine(DelayLevelDataLoad(scene.name));

            LevelExitDoor.Instance.DestroyDoorCheck(exitDoor,exitTrigger);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // So the data can load in time
    

    IEnumerator DelayLevelDataLoad(string sceneName)
    {
        yield return new WaitForSeconds(0.02f);
        
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

}
