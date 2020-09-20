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
    bool levelCallbacksSet = true;

    //Current level being played
    private static int? CurrentLevelIndex;

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
        CurrentLevelIndex = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentLevelIndex != null && !levelCallbacksSet)
        {
            //add next level callback to level scene
            Scene activeScene = SceneManager.GetActiveScene();
            Scene levelScene = SceneManager.GetSceneByName(Levels[CurrentLevelIndex.Value]);

            //If the level scene has been loaded and is set as the active scene
            //but the level end screen callbacks have not been set
            if (activeScene.name == levelScene.name && !levelCallbacksSet)
            {
                GameStateController levelGameStateController = GameObject.Find("/Game State Controller").GetComponent<GameStateController>();
                levelGameStateController.onNextLevelClick += NextLevelCallback;
                levelGameStateController.onRetryLevelClick += RetryLevelCallback;
                levelCallbacksSet = true;
            }
            //if the level scene is not the active scene but has been loaded
            else if (activeScene.name != levelScene.name && levelScene.isLoaded)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(Levels[CurrentLevelIndex.Value]));
            }
        }
    }

    void OnPlayClicked()
    {
        //load first level scene
        if (Levels.Length > 0 && !string.IsNullOrEmpty(Levels[0]))
        {
            CurrentLevelIndex = 0;
            levelCallbacksSet = false;
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
        //unload current level scene
        SceneManager.UnloadScene(Levels[CurrentLevelIndex.Value]);

        if (CurrentLevelIndex.Value < Levels.Length - 1)
        {
            //load next level scene
            CurrentLevelIndex++;
            levelCallbacksSet = false;
            SceneManager.LoadScene(Levels[CurrentLevelIndex.Value], LoadSceneMode.Additive);
        }
        else {
            CurrentLevelIndex = null;
        }
    }

    void RetryLevelCallback()
    {
        //unload current level scene
        SceneManager.UnloadScene(Levels[CurrentLevelIndex.Value]);

        //reload current level
        levelCallbacksSet = false;
        SceneManager.LoadScene(Levels[CurrentLevelIndex.Value], LoadSceneMode.Additive);
    }

    public int? GetCurrentLevelIndex()
    {
        return CurrentLevelIndex;
    }

}
