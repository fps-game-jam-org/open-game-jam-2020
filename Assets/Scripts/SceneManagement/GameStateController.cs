using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    //Game clock
    private float gameClock;

    //Game clock event
    public delegate void OnGameClockChange(float gameClock);
    public event OnGameClockChange onGameClockChange;

    //State machine states
    public enum State
    {
        Start,
        Run,
        Win,
        Lose
    }

    //Current state
    private State currentState;

    //Next state
    private State nextState;

    //State machine state change event
    public delegate void OnStateChange(State state);
    public event OnStateChange onStateChange;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize current state
        currentState = State.Start;

        //Set initial state
        nextState = State.Start; 

        //Initialize game clock
        gameClock = 0;       
    }

    // Update is called once per frame
    void Update()
    {
        //Update game clock
        gameClock += Time.deltaTime;
        
        //Call on game clock change event
        if(onGameClockChange != null) {
            onGameClockChange(gameClock);
        }

        //------ State Machine ------

        //Call on state change event
        if(nextState != currentState && onStateChange != null) {
            onStateChange(nextState);
        }

        //Update current state
        currentState = nextState;

        //Start state: show level intro
        if(currentState == State.Start) {

        }

        //Game state: level is being played
        if(currentState == State.Run) {

        }

        //Win state: level passed
        if(currentState == State.Win) {

        }

        //Lose state: level failed
        if(currentState == State.Lose) {

        }

    }
}
