using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is used to control the behavior of our Robot (State Machine and Utility function)
/// </summary>
public class JammoBehaviorTwo : MonoBehaviour
{
    /// <summary>
    /// The Robot Action List
    /// </summary>
    [System.Serializable]
    public struct Actions
    {
        public string sentence;
        public string verb;
        public string noun;
    }

    /// <summary>
    /// Enum of the different possible states of our Robot
    /// </summary>
    private enum State
    {
        Idle,
        WalkForward,
        WalkBackward, 
        SlideForward, 
        SlideBackward
    }

    [Header("Robot Brain")]
    public JammoBrain jammoBrain;

    [Header("Robot list of actions")]
    public List<Actions> actionsList;

    [Header("NavMesh and Animation")]
    public GameObject robot;
    public float reachedPositionDistance;       // Tolerance distance between the robot and object.
    public float reachedObjectPositionDistance; // Tolerance distance between the robot and object.
    public Transform playerPosition;            // Our position
    public GameObject goalObject;               
    public GameObject grabPosition;             // Position where the object will be placed during the grab

    private Movement simpleMovement;

    public Camera cam;                          // Main Camera

    [Header("Input UI")]
    public TMPro.TMP_InputField inputField;     // Our Input Field UI

    private State state;

    [HideInInspector]
    public List<string> sentences; // Robot list of sentences (actions)
    public string[] sentencesArray;

    [HideInInspector]
    public float maxScore;
    public int maxScoreIndex;

    private void Awake()
    {
        // Set the State to Idle
        state = State.Idle;

        // Take all the possible actions in actionsList
        foreach (JammoBehaviorTwo.Actions actions in actionsList)
        {
            sentences.Add(actions.sentence);
        }
        sentencesArray = sentences.ToArray();

        simpleMovement = robot.GetComponent<Movement>();
    }

    public void Utility(float maxScore, int maxScoreIndex)
    {
        // First we check that the score is > of 0.2, otherwise we let our agent perplexed;
        // This way we can handle strange input text (for instance if we write "Go see the dog!" the agent will be puzzled).
        if (maxScore < 0.20f)
        {
            state = State.Idle;
        }
        else
        {
            // Get the verb and noun (if there is one)
            goalObject = GameObject.Find(actionsList[maxScoreIndex].noun);

            string verb = actionsList[maxScoreIndex].verb;

            // Set the Robot State == verb
            state = (State)System.Enum.Parse(typeof(State), verb, true);
        }
    }

    /// <summary>
    /// When the user finished to type the order
    /// </summary>
    /// <param name="prompt"></param>
    public void OnOrderGiven(string prompt)
    {
        jammoBrain.RankSimilarityScores(prompt, sentencesArray);
    }


    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            string prompt = inputField.text;
            StartCoroutine(simpleMovement.ProcessInput(prompt, robot));
            inputField.text = "";
        }*/

        // Here's the State Machine, where given its current state, the agent will act accordingly'
        switch (state)
        {
            default:
            case State.Idle:
                break;

            case State.WalkForward:
                StartCoroutine(simpleMovement.ProcessInput("move forward", robot));
                state = State.Idle;
                break;
        }

        /*switch(state)
        {
            default:
            case State.Idle:
                break;

            case State.Hello:
                agent.SetDestination(playerPosition.position);
                if (Vector3.Distance(transform.position, playerPosition.position) < reachedPositionDistance)
                {
                    RotateTo();
                    anim.SetBool("hello", true);
                    state = State.Idle;
                }
                break;

            case State.Happy:
                agent.SetDestination(playerPosition.position);
                if (Vector3.Distance(transform.position, playerPosition.position) < reachedPositionDistance)
                {
                    RotateTo();
                    anim.SetBool("happy", true);
                    state = State.Idle;
                }
                break;

            case State.Puzzled:
                agent.SetDestination(playerPosition.position);
                if (Vector3.Distance(transform.position, playerPosition.position) < reachedPositionDistance)
                {
                    RotateTo();
                    anim.SetBool("puzzled", true);
                    state = State.Idle;
                }
                break;

            case State.MoveTo:
                agent.SetDestination(goalObject.transform.position);
                
                if (Vector3.Distance(transform.position, goalObject.transform.position) < reachedPositionDistance)
                {
                    state = State.Idle;
                }
                break;

            case State.BringObject:
                // First move to the object
                agent.SetDestination(goalObject.transform.position);
                if (Vector3.Distance(transform.position, goalObject.transform.position) < reachedObjectPositionDistance)
                {
                    Grab(goalObject);
                    state = State.BringObjectToPlayer;
                }
                break;

            case State.BringObjectToPlayer:
                agent.SetDestination(playerPosition.transform.position);
                if (Vector3.Distance(transform.position, playerPosition.transform.position) < reachedObjectPositionDistance)
                {
                    Drop(goalObject);
                    state = State.Idle;
                }
                break;
        }*/
    }
}
