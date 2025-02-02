using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private Scene sceneStart;

    public TMP_Text hpUI;
    public TMP_Text scoreUI;
    public TMP_Text bounceUI;

    public GameObject player;
    private Rigidbody2D playerRB;

    private float powerX;
    private float powerXval;

    private int maxHP;

    private bool ignoreFirstSceneLoad = true;

    public List<SaveableObjects> saveableObjects;
    SerializedLevelData myLevelData = new SerializedLevelData();

    public List<GameObject> ObjectsKeep;

    private SaveLoadManager saveManager;

    private PrefabSpawner[] spawner;

    public Coroutine damageRoutine, flashRoutine, dataSwitchRoutine;

    private List<string> sceneLoaded = new List<string>();

    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += OnSceneLoaded;

        sceneStart = SceneManager.GetActiveScene();

        maxHP = 10;
        saveManager = SaveLoadManager.Instance;

        // Don't Destroy
        DontDestroyOnLoad(this.gameObject);

        foreach (GameObject obj in ObjectsKeep)
        {
            DontDestroyOnLoad(obj);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponentInParent<Rigidbody2D>();

        GameData.LevelState = sceneStart.name;

        GameData.Hp = 3;
        GameData.Score = 0;

        DisplayScore();
        DisplayHp();

        LevelExitDoor.Instance.DestroyDoorCheck();
        SaveLoadManager.Instance.SaveDataCheckPoint(player.transform.position);

        
        ignoreFirstSceneLoad = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameData.Hp <= 0)
        {
            if(SaveLoadManager.Instance.hasSavedOnce == false)
            {
                SceneManager.LoadScene(GameData.LevelState);
                SaveLoadManager.Instance.LoadCheckpointData();
                SaveLoadManager.Instance.LoadLevelData(SceneManager.GetActiveScene().name);
                CheckpointLoadData();
            }
            else
            {
                SaveLoadManager.Instance.LoadCheckpointData();
                SaveLoadManager.Instance.LoadLevelData(SceneManager.GetActiveScene().name);
                CheckpointLoadData();

            }
        }
    }

    void FindAllChainsInLevel()
    {
        GameObject.Find("Assets/Scenes/");
    }

    // Load Scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "HUB" && scene.name != "MainMenu" && scene.name != "OptionScreen")
        {
            
            // Spawns in the starting Door
            GameObject doorStart = GameObject.FindGameObjectWithTag("DoorStart");

            if (doorStart == null)
            {
                if (player != null)
                {
                    player.transform.position = new Vector3(0, 0, 0);
                    GameData.PlayerPos = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if(player != null)
                { 
                  player.transform.position = new Vector3(doorStart.transform.position.x, doorStart.transform.position.y - 0.4f,0);
                  GameData.PlayerPos = new Vector3(doorStart.transform.position.x, doorStart.transform.position.y - 0.4f, 0);
                }
            }

            // Reset Player Velocity on Scene Loaded
            Player.Instance.ResetPlayerVel();

            if(ignoreFirstSceneLoad == false && dataSwitchRoutine == null)
            { 
                dataSwitchRoutine = StartCoroutine(SwitchSceneData(scene.name));
            }
            // So it won't load objects twice since the switchSceneData also Loads the data
            else if(ignoreFirstSceneLoad == true)
            {
                StartCoroutine(SceneDataStart(scene.name));
            }
        }

    }

    // Data Related

    bool checkedScene = false;
    public void HasObjectsSpawnedOnce(string sceneName)
    {
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

            LoadObjects(sceneName);

            checkedScene = false;
        }
        else
        {
            LoadObjects(sceneName);
            checkedScene = false;
        }

        //Debug.Log(sceneLoaded.Count);
    }

    void CheckpointLoadData()
    {
        player.transform.position = GameData.PlayerPos;
        GameData.Hp = maxHP;
        DisplayHp();
        DisplayScore();
    }

    private void LoadObjects(string sceneName)
    {
        if (checkedScene == false)
        {
            foreach (PrefabSpawner spawn in spawner)
            {
                if (spawn != null)
                {
                    spawn.SpawnItemOnce();
                }
            }

            sceneLoaded.Add(sceneName);
            SaveLoadManager.Instance.SaveLevelData(sceneName);

        }
    }

    // Routines

    private IEnumerator SwitchSceneData(string sceneName)
    {
        yield return new WaitForSeconds(0.01f);
        HasObjectsSpawnedOnce(sceneName);
        yield return new WaitForSeconds(0.01f);
        SaveLoadManager.Instance.LoadLevelData(sceneName);
        yield return new WaitForSeconds(0.01f);
        dataSwitchRoutine = null;
    }

    private IEnumerator SceneDataStart(string sceneName)
    {
        yield return new WaitForSeconds(0.01f);
        HasObjectsSpawnedOnce(sceneName);
        yield return new WaitForSeconds(0.01f);
    }

    public IEnumerator DamagePlayer()
    {
        PlayerState.IsBounceMode = false;
        PlayerState.IsStickActive = false;

        PlayerState.IsDamaged = true;

        // KnockBack
        playerRB.gravityScale = 0.5f;

        powerX = Mathf.Sign(playerRB.velocity.x) < 0 ? 6 : -6;
        playerRB.velocity = new Vector3(powerX, 4, 1);

        if(flashRoutine == null)
        { 
             flashRoutine = StartCoroutine(ColorFlash(player));
        }

        yield return new WaitForSeconds(0.7f);

        GameData.Hp--;
        DisplayHp();

        PlayerState.IsDamaged = false;

        StopCoroutine(flashRoutine);
        flashRoutine = null;

        playerRB.gravityScale = 10f;
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        // Delay before getting damaged again
        yield return new WaitForSeconds(1.5f);
        damageRoutine = null;
    }

    public IEnumerator ColorFlash(GameObject entity)
    {
        bool mode;

        //Debug.Log(entity.name);

        mode = entity.name == "Player" ? true : false;

        while (true)
        {
            yield return new WaitForSeconds(0.20f);

            if (mode == true)
            {
                entity.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                entity.GetComponent<SpriteRenderer>().color = Color.red;
            }
            yield return new WaitForSeconds(0.25f);

            if (mode == true)
            {
                entity.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                entity.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    // Displays

    public void DisplayHp()
    {
        if(GameData.Hp > maxHP)
        {
            GameData.Hp = maxHP;
        }

        hpUI.text = GameData.Hp.ToString() + "/10";
        // save data here?
    }

    public void DisplayScore()
    {
        string scoreDefualt = "0000000";
        int scoreData = GameData.Score;
        string score;

        if (GameData.Score.ToString().Length > scoreDefualt.Length)
        {
            // Maxes out of 999999 if it over loads
            score = "9999999";
        }
        else
        { 
                                                        // Range
           score = (scoreDefualt + scoreData.ToString())[scoreData.ToString().Length..];
        }

        scoreUI.text = score;
        // save data here?
    }

    public void DisplayBounces(int bounces)
    {
        bounceUI.text = "Bounces: " + bounces;
    }

    public void BouncesTotalCounted(int bounces)
    {
        if(GameData.TotalBounces <= bounces)
        {
            GameData.TotalBounces = bounces;
        }
    }

}
