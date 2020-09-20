using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [Tooltip("Array of level names in order of play")]
    public string[] Levels;

    //Has the next level callback been added to the current level scene
    bool nextLevelCallbackSet = true;

    //Current level being played
    private static int CurrentLevelIndex;

    // Start is called before the first frame update
    void Start()
    {
        //Add play button listener
        GameObject PlayButtonObject = GameObject.Find("/Canvas/Play Button");
        Button PlayButton = PlayButtonObject.GetComponent<Button>();
        PlayButton.onClick.AddListener(OnPlayClicked);

        //Add credits button listener
        GameObject CreditsButtonObject = GameObject.Find("/Canvas/Credits Button");
        Button CreditsButton = CreditsButtonObject.GetComponent<Button>();
        CreditsButton.onClick.AddListener(OnCreditsClicked);

        //Add exit button listener
        GameObject ExitButtonObject = GameObject.Find("/Canvas/Exit Button");
        Button ExitButton = ExitButtonObject.GetComponent<Button>();
        ExitButton.onClick.AddListener(OnExitClicked);

        //Initiate current level index
        CurrentLevelIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //add next level callback to level scene
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == Levels[CurrentLevelIndex] && !nextLevelCallbackSet)
        {
            GameStateController levelGameStateController = GameObject.Find("/Game State Controller").GetComponent<GameStateController>();
            levelGameStateController.onNextLevelClick += NextLevelCallback;
            nextLevelCallbackSet = true;
        }
        else if (currentScene.name != Levels[CurrentLevelIndex] && !nextLevelCallbackSet)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(Levels[CurrentLevelIndex]));
        }
    }

    void OnPlayClicked()
    {
        //load first level scene
        if (Levels.Length > 0 && !string.IsNullOrEmpty(Levels[0]))
        {
            CurrentLevelIndex = 0;
            nextLevelCallbackSet = false;
            SceneManager.LoadScene(Levels[0], LoadSceneMode.Additive);
        }
    }

    void OnCreditsClicked()
    {
        //load credits scene
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    void OnExitClicked()
    {
        //exit game
        Application.Quit();
    }

    void NextLevelCallback()
    {
        //unload previous level scene
        SceneManager.UnloadScene(Levels[CurrentLevelIndex]);

        if (CurrentLevelIndex < Levels.Length - 1)
        {
            //load next level scene
            CurrentLevelIndex++;
            nextLevelCallbackSet = false;
            SceneManager.LoadScene(Levels[CurrentLevelIndex], LoadSceneMode.Additive);
        }
    }

}
