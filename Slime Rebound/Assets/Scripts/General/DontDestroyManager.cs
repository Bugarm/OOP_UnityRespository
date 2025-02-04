using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyManager : Singleton<DontDestroyManager>
{
    public GameObject levelDontDest;
    public GameObject hubDontDest;

    private GameObject[] sceneTrigger;
    private GameObject player;
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>().gameObject;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("HUB"))
        {
            if(GameObject.Find("DontDestroyHUB") == false)
            { 
                StartCoroutine(DelaySpawn(hubDontDest));
            }
        }

        if (scene.name.StartsWith("TutorialRoom") || scene.name.StartsWith("ForestLevel"))
        {
            if (GameObject.Find("DontDestroyGroup") == false)
            {
                StartCoroutine(DelaySpawn(levelDontDest));
            }
        }
    }

    // This is done so the singleton won't destroy the child objects for a frame when switching
    private IEnumerator DelaySpawn(GameObject manager)
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(manager);
    }

    public void DoorSpawnIn()
    {

        // Spawns in the starting Door
        GameObject doorStart = GameObject.FindGameObjectWithTag("DoorStart");
        sceneTrigger = GameObject.FindGameObjectsWithTag("SceneTrigger");

        if (sceneTrigger.Length <= 0)
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
        }
        else
        {
            foreach (GameObject sceneTrig in sceneTrigger)
            {
                NextSceneTrigger trig = sceneTrig.GetComponent<NextSceneTrigger>();

                if (GameData.SceneTransID == trig.id)
                {
                    player.transform.position = new Vector3(sceneTrig.transform.position.x, sceneTrig.transform.position.y - 0.4f, 0);
                    GameData.PlayerPos = new Vector3(sceneTrig.transform.position.x, sceneTrig.transform.position.y - 0.4f, 0);
                }
            }

        }
        // Reset Player Velocity on Scene Loaded
        Player.Instance.ResetPlayerVel();
    }
}
