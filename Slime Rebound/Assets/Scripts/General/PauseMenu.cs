using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Singleton<PauseMenu>
{
    public Canvas pauseMenu;

    public GameObject playerHUD;

    private Canvas canvasVar;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameData.IsPaused = false;
        //canvasVar = Instantiate(pauseMenu);
        pauseMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameData.IsPaused = !GameData.IsPaused;
            PauseFunction();
        }
    }

    public void PauseFunction()
    {
        if (GameData.IsPaused == true)
        {
            Time.timeScale = 0f;

            pauseMenu.gameObject.SetActive(true);
            playerHUD.SetActive(false);
        }
        else
        {
            Time.timeScale = 1.0f;
            pauseMenu.gameObject.SetActive(false);
            playerHUD.SetActive(true);
        }
    }
}
