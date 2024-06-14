using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is used to control the behavior of our Robot 
/// </summary>
public class ToyRobot : MonoBehaviour
{

    [Header("Game Object")]
    public GameObject robot;

    private Movement simpleMovement;

    public Camera cam;                          // Main Camera

    [Header("Input UI")]
    public TMPro.TMP_InputField inputField;     // Our Input Field UI

    private void Awake()
    {
        simpleMovement = robot.GetComponent<Movement>();
    }

    private IEnumerator ProcessActions(string[] actions)
    {
        foreach (string action in actions)
        {
            string trimmedAction = action.Trim().ToLower();

            switch (trimmedAction)
            {
                default:
                case "idle":
                    break;

                case "move forward":
                    yield return StartCoroutine(simpleMovement.Walking(Vector3.forward, robot));
                    break;

                case "move backward":
                    yield return StartCoroutine(simpleMovement.Walking(Vector3.back, robot));
                    break;

                case "slide forward":
                    yield return StartCoroutine(simpleMovement.Sliding(Vector3.forward, robot));
                    break;

                case "slide backward":
                    yield return StartCoroutine(simpleMovement.Sliding(Vector3.back, robot));
                    break;

                case "turn right":
                    yield return StartCoroutine(simpleMovement.Turn(true, robot));
                    break;

                case "turn left":
                    yield return StartCoroutine(simpleMovement.Turn(false, robot));
                    break;
            }
        }
        yield break;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string prompt = inputField.text;

            string[] actions = prompt.Split(',');

            StartCoroutine(ProcessActions(actions));

            inputField.text = "";
        }
    }
}