using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class DontDestroyManager : Singleton<DontDestroyManager>
{
    string scene;

    private UnityEngine.SceneManagement.Scene sceneStart;

    string sceneName;

    [SerializeField] private GameObject levelDontDest;
    [SerializeField] private GameObject hubDontDest;
    [SerializeField] private GameObject menuDest;

    public float offset;

    GameObject curManager;

    public Coroutine screenTransRoutine, delayDoorRoutine;

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
        PlayerState.DisableAllMove = false;
        GameData.HasSceneTransAnim = false;
        GameData.HasEnteredScreneTrig = false;
        GameData.HasLevelDoor = false;
        GameData.HasEnteredDoor = false;
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
        else if (scene.name.StartsWith("TestRoom") || scene.name.StartsWith("Bonus") || scene.name.StartsWith("TutorialRoom") || scene.name.StartsWith("ForestLevel"))
        {
            if (GameObject.FindFirstObjectByType<DontDestroyGroup>() == null)
            {
                StartCoroutine(SpawnUntil(levelDontDest, "hub"));
            }
        }
        else if (scene.name.StartsWith("MainMenu"))
        {
            if (GameObject.FindFirstObjectByType<DontDestroyMenu>() == null)
            {
                StartCoroutine(SpawnUntil(menuDest, "menu"));
            }
        }
        else
        {
            Destroy(curManager);
        }

        // Scene
        sceneStart = SceneManager.GetActiveScene();

        if (sceneStart.name.Any(char.IsDigit))
        {
            sceneName = sceneStart.name.Substring(0, sceneStart.name.Length - 1);
        }
        else
        {
            sceneName = sceneStart.name;
        }

        GameData.LevelState = sceneName;

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
            case "menu":
                TryGetComponent<DontDestroyMenu>(out DontDestroyMenu compM);
                yield return new WaitUntil(() => compM == null);

                break;
        }

        curManager = Instantiate(manager);
    }

    public IEnumerator ScreenTrans(bool switchRoom, string level = "", int nextRoomNum = 0)
    {
        GameObject blackObj = TransScreen.Instance.screenObj;
        Image blackTrans = TransScreen.Instance.blackTrans;

        blackObj.SetActive(true);
        blackTrans.color = new Color(0, 0, 0, 0);

        if (delayDoorRoutine == null)
        {
            delayDoorRoutine = StartCoroutine(DelayDoorEnter());
        }

        while (blackTrans.color.a <= 1)
        {
            // Fade In
            blackTrans.color = new Color(0, 0, 0, blackTrans.color.a + Time.deltaTime + 0.01f);
            yield return new WaitForSeconds(0.001f);
        }

        // Load next room num
        if (switchRoom == true)
        {
            SaveLoadManager.Instance.SaveLevelData(SceneManager.GetActiveScene().name);
            yield return new WaitForSeconds(0.01f);
            SceneTransFunct(nextRoomNum);
        }
        else // Loads level directly
        {
            SceneManager.LoadScene(level);
        }


        yield return new WaitForSeconds(0.35f);

        if (GameObject.FindFirstObjectByType<DontDestroyMenu>() == null)
        {
            CameraFollow.Instance.UpdateCam();
        }

        while (blackTrans.color.a >= 0)
        {
            // Fade Out
            blackTrans.color = new Color(0, 0, 0, blackTrans.color.a - Time.deltaTime - 0.01f);
            yield return new WaitForSeconds(0.001f);
        }

        PlayerState.DisableAllMove = false;
        GameData.HasSceneTransAnim = false;
        GameData.HasEnteredScreneTrig = false;
        GameData.HasLevelDoor = false;
        GameData.HasEnteredDoor = false;
        blackTrans.color = new Color(0, 0, 0, 0);
        blackTrans.gameObject.SetActive(false);
        screenTransRoutine = null;
    }


    private void SceneTransFunct(int nextRoomNum)
    {
        if (nextRoomNum <= 0)
        {
            SceneManager.LoadScene(GameData.LevelState);
        }
        else
        {
            SceneManager.LoadScene(GameData.LevelState + nextRoomNum);
        }
    }

    public IEnumerator DelayDoorEnter()
    {
        GameData.DoorDelay = true;
        yield return new WaitForSeconds(1.5f);
        GameData.DoorDelay = false;
        delayDoorRoutine = null;
    }
}