using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [Tooltip("Array of level names in order of play")]
    public string[] Levels;

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

    void OnPlayClicked()
    {
        //load first level scene
        if (Levels.Length > 0 && !string.IsNullOrEmpty(Levels[0]))
        {
            CurrentLevelIndex = 0;
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(Levels[CurrentLevelIndex.Value], LoadSceneMode.Additive);
            loadScene.completed += LevelLoadCallback;
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
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(Levels[CurrentLevelIndex.Value]);

        unloadScene.completed += (asyncOperation) =>
        {
            if (CurrentLevelIndex.Value < Levels.Length - 1)
            {
                //load next level scene
                CurrentLevelIndex++;
                AsyncOperation loadScene = SceneManager.LoadSceneAsync(Levels[CurrentLevelIndex.Value], LoadSceneMode.Additive);
                loadScene.completed += LevelLoadCallback;
            }
            else
            {
                CurrentLevelIndex = null;
            }
        };
    }

    void RetryLevelCallback()
    {
        //unload current level scene
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(Levels[CurrentLevelIndex.Value]);

        unloadScene.completed += (asyncOperation) =>
        {
            //reload current level
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(Levels[CurrentLevelIndex.Value], LoadSceneMode.Additive);
            loadScene.completed += LevelLoadCallback;
        };
    }

    private void LevelLoadCallback(AsyncOperation operation)
    {
        SetLevelEndScereenCallbacks();
    }

    void SetLevelEndScereenCallbacks()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(Levels[CurrentLevelIndex.Value]));

        GameStateController levelGameStateController = GameObject.Find("/Game State Controller").GetComponent<GameStateController>();
        levelGameStateController.onNextLevelClick += NextLevelCallback;
        levelGameStateController.onRetryLevelClick += RetryLevelCallback;
    }

    public int? GetCurrentLevelIndex()
    {
        return CurrentLevelIndex;
    }

}
