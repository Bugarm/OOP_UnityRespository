using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyHUB : Singleton<DontDestroyHUB>
{
    public GameObject player;
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name.StartsWith("HUB"))
        {
            
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
