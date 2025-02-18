using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DontDestroyManager : Singleton<DontDestroyManager>
{
    string scene;

    [SerializeField] private GameObject levelDontDest;
    [SerializeField] private GameObject hubDontDest;
    GameObject blackObj;
    Image blackTrans;


    public float offset;

    public Coroutine screenTransRoutine;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);

        scene = SceneManager.GetActiveScene().name;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoadedLevel;

    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedLevel;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSceneLoadedLevel(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("HUB"))
        {
            if (GameObject.FindFirstObjectByType<DontDestroyHUB>() == null)
            {
                StartCoroutine(SpawnUntil(hubDontDest, "group"));
            }
        }

        if (scene.name.StartsWith("TestRoom") || scene.name.StartsWith("Bonus") || scene.name.StartsWith("TutorialRoom") || scene.name.StartsWith("ForestLevel"))
        {
            if (GameObject.FindFirstObjectByType<DontDestroyGroup>() == null)
            {
                StartCoroutine(SpawnUntil(levelDontDest, "hub"));
            }
        }


    }

    IEnumerator SpawnUntil(GameObject manager, string find)
    {

        switch(find)
        {
            case "group":
                TryGetComponent<DontDestroyGroup>(out DontDestroyGroup component);
                yield return new WaitUntil(() => component == null);

                break;
            case "hub":
                TryGetComponent<DontDestroyHUB>(out DontDestroyHUB comp);
                yield return new WaitUntil(() => comp == null);

                break;
        }

        Instantiate(manager);
    }

    public IEnumerator ScreenTrans(bool switchRoom, string level = "", int nextRoomNum = 0)
    {
        GameObject blackObj = TransScreen.Instance.screenObj;
        Image blackTrans = TransScreen.Instance.blackTrans;

        blackObj.SetActive(true);
        blackTrans.color = new Color(0, 0, 0, 0);

        while (blackTrans.color.a <= 1)
        {
            // Fade In
            blackTrans.color = new Color(0, 0, 0, blackTrans.color.a + Time.deltaTime + 0.01f);
            yield return new WaitForSeconds(0.001f);

        }

        if (switchRoom == true)
        {
            SceneTransFunct(nextRoomNum);
        }
        else
        {
            SceneManager.LoadScene(level);

        }
        yield return new WaitForSeconds(0.35f);
        CameraFollow.Instance.UpdateCam();


        while (blackTrans.color.a >= 0)
        {
            // Fade Out
            blackTrans.color = new Color(0, 0, 0, blackTrans.color.a - Time.deltaTime - 0.01f);
            yield return new WaitForSeconds(0.001f);
        }

        PlayerState.DisableAllMove = false;
        GameData.HasSceneTransAnim = false;
        blackTrans.color = new Color(0, 0, 0, 0);
        blackTrans.gameObject.SetActive(false);
        screenTransRoutine = null;
    }


    private void SceneTransFunct(int nextRoomNum)
    {
        if (nextRoomNum <= 0)
        {
            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(GameData.LevelState);
        }
        else
        {
            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(GameData.LevelState + nextRoomNum);
        }
    }
}