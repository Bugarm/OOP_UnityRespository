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
    float savePlayerVel;
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
            if (GameObject.Find("DontDestroyHUB") == false)
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




}