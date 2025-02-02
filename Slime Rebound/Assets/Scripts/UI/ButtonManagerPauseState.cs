using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManagerPauseState : Singleton<ButtonManagerPauseState>
{
    [SerializeField] List<Button> buttons;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in buttons)
        {

            if (button.name == "ExitGameButton")
            {
                button.onClick.AddListener(QuitGame);
            }
            if (button.name == "ContinueButton")
            {
                button.onClick.AddListener(ReturnToGame);
            }
            if (button.name == "ReturnMenuButton")
            {
                button.onClick.AddListener(ReturnToMenu);
            }

        }
    }

    void QuitGame()
    {
        Debug.Log("You have clicked the exit button");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
    
        #endif
    }
    
    void ReturnToGame()
    {
        GameData.IsPaused = false;
        PauseMenu.Instance.PauseFunction();
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
