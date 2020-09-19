using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [Tooltip("Array of level names in order of play")]
    public string[] Levels;

    private static int CurrentLevelIndex;

    // Start is called before the first frame update
    void Start()
    {
        GameObject PlayButtonObject = GameObject.Find("/Canvas/Play Button");
        GameObject CreditsButtonObject = GameObject.Find("/Canvas/Credits Button");
        GameObject ExitButtonObject = GameObject.Find("/Canvas/Exit Button");

        Button PlayButton = PlayButtonObject.GetComponent<Button>();
        Button CreditsButton = CreditsButtonObject.GetComponent<Button>();
        Button ExitButton = ExitButtonObject.GetComponent<Button>();

        PlayButton.onClick.AddListener(OnPlayClicked);
        CreditsButton.onClick.AddListener(OnCreditsClicked);
        ExitButton.onClick.AddListener(OnExitClicked);

        CurrentLevelIndex = 0;
    }

    void OnPlayClicked() {
        //load first level scene
        if(Levels.Length > 0 && !string.IsNullOrEmpty(Levels[0])) {
            CurrentLevelIndex = 0;
            SceneManager.LoadScene(Levels[0], LoadSceneMode.Single);
        }
    }

    void OnCreditsClicked() {
        //load credits scene
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    void OnExitClicked() {
        //exit game
        Application.Quit();
    }

}
