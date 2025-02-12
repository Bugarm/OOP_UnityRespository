using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyManager : Singleton<DontDestroyManager>
{
    string scene;

    [SerializeField] private GameObject levelDontDest;
    [SerializeField] private GameObject hubDontDest;

    public float offset;


    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);

        scene = SceneManager.GetActiveScene().name;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (scene.StartsWith("HUB"))
        {
            if (GameObject.FindFirstObjectByType<DontDestroyHUB>() == null)
            {
                Instantiate(hubDontDest);
            }
        }
        
        if (scene.StartsWith("TestRoom") || scene.StartsWith("TutorialRoom") || scene.StartsWith("ForestLevel"))
        {
            if (GameObject.FindFirstObjectByType<DontDestroyGroup>() == null)
            {
                Instantiate(levelDontDest);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }





}