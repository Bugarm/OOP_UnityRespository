using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyMenu : Singleton<DontDestroyMenu>
{
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);

    }

    // Scene Loading Managers
    private void OnEnable()
    {
        //Debug.Log("Enable");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        //Debug.Log("Disable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        if (scene.name.StartsWith("MainMenu"))
        {

        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
