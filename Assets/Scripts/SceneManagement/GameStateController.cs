using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStateController : MonoBehaviour
{
    //Level score
    private float score;

    //Level number
    //TODO get level number from scene controller
    private int levelNumber;
    //Level intro game object
    private GameObject levelIntro;
    //Level intro text
    private TextMeshProUGUI levelIntroText;
    //How long to show the level intro
    [Tooltip("Duration of level intro with level number")]
    public float LevelIntroDuration = 3;

    //Level end
    private GameObject levelEnd;
    //Next level click event
    public delegate void OnNextLevelClick();
    public event OnNextLevelClick onNextLevelClick;
    //Retry level click event
    public delegate void OnRetryLevelClick();
    public event OnRetryLevelClick onRetryLevelClick;

    //Game clock
    private float gameClock;
    //Game clock event
    public delegate void OnGameClockChange(float gameClock);
    public event OnGameClockChange onGameClockChange;

    //State machine states
    public enum State { None = 0, Start, Run, Win, Lose }
    //Current state
    private State currentState;
    //Next state
    private State nextState;
    //Previous state
    private State previousState;
    //State machine state change event
    public delegate void OnStateChange(State state);
    public event OnStateChange onStateChange;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize current state
        currentState = State.None;

        //Set initial state
        nextState = State.Start;

        //Initialize game clock
        gameClock = 0;

        //Initialize level intro game object
        levelIntro = GameObject.Find("/Game State Controller/Level Intro");
        //Initialize level intro text
        levelIntroText = GameObject.Find("/Game State Controller/Level Intro/Canvas/Level").GetComponent<TextMeshProUGUI>();
        levelIntro.SetActive(false);

        //initialize level end game object
        levelEnd = GameObject.Find("/Game State Controller/Level End");

        //Initialize next level click event
        EventTrigger nextLevelButton = GameObject.Find("/Game State Controller/Level End/Canvas/Next Level").GetComponent<EventTrigger>();
        EventTrigger.Entry nextLevelEntry = new EventTrigger.Entry();
        nextLevelEntry.eventID = EventTriggerType.PointerClick;
        nextLevelEntry.callback.AddListener((eventData) => { NextLevelClickCallback(); });
        nextLevelButton.triggers.Add(nextLevelEntry);

        //Initialize retry level click event
        EventTrigger retryLevelButton = GameObject.Find("/Game State Controller/Level End/Canvas/Retry Level").GetComponent<EventTrigger>();
        EventTrigger.Entry retryLevelEntry = new EventTrigger.Entry();
        retryLevelEntry.eventID = EventTriggerType.PointerClick;
        retryLevelEntry.callback.AddListener((eventData) => { RetryLevelClickCallback(); });
        retryLevelButton.triggers.Add(retryLevelEntry);

        //Initialize exit level click event
        EventTrigger exitLevelButton = GameObject.Find("/Game State Controller/Level End/Canvas/Exit Level").GetComponent<EventTrigger>();
        EventTrigger.Entry exitLevelEntry = new EventTrigger.Entry();
        exitLevelEntry.eventID = EventTriggerType.PointerClick;
        exitLevelEntry.callback.AddListener((eventData) => { ExitLevelClickCallback(); });
        exitLevelButton.triggers.Add(exitLevelEntry);

        levelEnd.SetActive(false);


        //Initialize level score
        score = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //Update game clock
        gameClock += Time.deltaTime;

        //Call on game clock change event
        if (onGameClockChange != null)
        {
            onGameClockChange(gameClock);
        }

        //------ State Machine ------

        //Call on state change event
        if (nextState != currentState && onStateChange != null)
        {
            onStateChange(nextState);
        }

        //Update current state
        previousState = currentState;
        currentState = nextState;

        //Start state: show level intro
        if (currentState == State.Start)
        {
            if (previousState == State.None)
            {
                ShowLevelIntro();
            }

            if (gameClock >= LevelIntroDuration)
            {
                HideLevelIntro();
                nextState = State.Run;
            }
        }

        //Game state: level is being played
        if (currentState == State.Run)
        {
            //TODO used for testing, remove
            if (gameClock > LevelIntroDuration + 3)
            {
                score = Random.Range(100.0f, 10000.0f);

                if (Random.Range(0.0f, 1.0f) > 0.5f)
                {
                    nextState = State.Win;
                }
                else
                {
                    nextState = State.Lose;
                }
            }
        }

        //Win state: level passed
        if (currentState == State.Win)
        {
            ShowLevelEnd();
        }

        //Lose state: level failed
        if (currentState == State.Lose)
        {
            ShowLevelEnd();
        }

    }

    private void ShowLevelIntro()
    {
        if (levelIntroText != null)
        {
            levelIntroText.text = $"Level {levelNumber}";
            levelIntro.SetActive(true);
        }
    }

    private void HideLevelIntro()
    {
        if (levelIntroText != null)
        {
            levelIntro.SetActive(false);
        }
    }

    private void ShowLevelEnd()
    {
        //show level end game object
        levelEnd.SetActive(true);

        //set score text
        TextMeshProUGUI scoreText = GameObject.Find("/Game State Controller/Level End/Canvas/Score").GetComponent<TextMeshProUGUI>();
        scoreText.text = $"Score: {score}";

        TextMeshProUGUI endStateText = GameObject.Find("/Game State Controller/Level End/Canvas/End State").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nextLevelText = GameObject.Find("/Game State Controller/Level End/Canvas/Next Level").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI retryLevelText = GameObject.Find("/Game State Controller/Level End/Canvas/Retry Level").GetComponent<TextMeshProUGUI>();
        if (currentState == State.Win)
        {
            //show win text
            endStateText.text = "You Won!";

            //enable next level button
            nextLevelText.enabled = true;

            //set retry button text to 'replay'
            retryLevelText.text = "Replay Level";
        }

        if (currentState == State.Lose)
        {
            //show lose text
            endStateText.text = "You Lost :(";

            //disable next level button
            nextLevelText.enabled = false;

            //set retry button text to retry
            retryLevelText.text = "Retry Level";
        }
    }

    //on next level click call next level event
    private void NextLevelClickCallback()
    {
        if (onNextLevelClick != null)
        {
            onNextLevelClick();
        }
    }

    //on retry / replay level click call retry level event
    private void RetryLevelClickCallback()
    {
        if (onRetryLevelClick != null)
        {
            onRetryLevelClick();
        }
    }

    //on exit click send back to title scene
    private void ExitLevelClickCallback()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }
}
