using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    //State machine states
    private enum State
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

    // Start is called before the first frame update
    void Start()
    {
        //Initialize current state
        currentState = State.Start;

        //Set initial state
        nextState = State.Start;        
    }

    // Update is called once per frame
    void Update()
    {
        //------ State Machine ------

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
