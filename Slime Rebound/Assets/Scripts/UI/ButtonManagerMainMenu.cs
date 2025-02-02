using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManagerMainMenu : Singleton<ButtonManagerMainMenu>
{
    [SerializeField] List<Button> buttons;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in buttons)
        {
            if (button.name == "StartGameButton")
            {
                button.onClick.AddListener(StartGame);
            }
            if (button.name == "OptionButton")
            {
                button.onClick.AddListener(OptionStateSwitch);
            }
            if (button.name == "ExitGameButton")
            {
                button.onClick.AddListener(QuitGame);
            }


        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("HUD");
    }

    void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
    
        #endif
    }

    void OptionStateSwitch()
    {
        SceneManager.LoadScene("OptionScreen");
    }

}
