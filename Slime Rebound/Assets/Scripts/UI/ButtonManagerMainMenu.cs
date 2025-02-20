using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManagerMainMenu : MonoBehaviour
{

    [SerializeField] List<Button> buttons;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in buttons)
        {

            if (button.name == "Start")
            {
                button.onClick.AddListener(StartGame);
            }
            if (button.name == "Option")
            {
                button.onClick.AddListener(OptionState);
            }
            if (button.name == "Quit")
            {
                button.onClick.AddListener(QuitGame);
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

    void OptionState()
    {
        SceneSwitchManager.Instance.SwitchToLevel("OptionScreen");
    }

    void StartGame()
    {
        SceneSwitchManager.Instance.SwitchToLevel("HUB");
    }

}
