using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateController : MonoBehaviour
{
    //Level number
    //TODO get from scene controller
    private int levelNumber;

    //Level intro text
    private TextMeshProUGUI levelIntroText;

    //How long to show the level intro
    [Tooltip("Duration of level intro with level number")]
    public float LevelIntroDuration = 3;

    //Game clock
    private float gameClock;

    //Game clock event
    public delegate void OnGameClockChange(float gameClock);
    public event OnGameClockChange onGameClockChange;

    //State machine states
    public enum State
    {
        None = 0,
        Start,
        Run,
        Win,
        Lose
    }

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

        //Initialize level intro text
        levelIntroText = GameObject.Find("/Canvas/Game State Controller/Level Intro").GetComponent<TextMeshProUGUI>();
        levelIntroText.enabled = false;
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

        }

        //Win state: level passed
        if (currentState == State.Win)
        {

        }

        //Lose state: level failed
        if (currentState == State.Lose)
        {

        }

    }

    private void ShowLevelIntro()
    {
        if (levelIntroText != null)
        {
            levelIntroText.text = $"Level {levelNumber}";
            levelIntroText.enabled = true;
        }
    }

    private void HideLevelIntro()
    {
        if (levelIntroText != null)
        {
            levelIntroText.enabled = false;
        }
    }
}
