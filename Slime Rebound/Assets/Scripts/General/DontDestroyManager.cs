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

    public float offset;

    Coroutine delaySpawnRoutine;

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
                Instantiate(hubDontDest);
            }
        }

        if (scene.name.StartsWith("TutorialRoom") || scene.name.StartsWith("ForestLevel"))
        {
            if (GameObject.Find("DontDestroyGroup") == false)
            {
                Instantiate(levelDontDest);
            }
        }
        
    }

    public void DoorSpawnIn()
    {

        // Spawns in the starting Door
        GameObject doorStart = GameObject.FindGameObjectWithTag("DoorStart");
        sceneTrigger = GameObject.FindGameObjectsWithTag("SceneTrigger");

        player = FindAnyObjectByType<Player>().gameObject;

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
        
        else if(GameData.HasEnteredDoor == false && GameData.HasEnteredScreneTrig == true)
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
