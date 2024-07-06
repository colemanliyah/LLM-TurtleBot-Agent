using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;

/// <summary>
/// This class is used to control the behavior of our Robot 
/// </summary>
public class ToyRobot : MonoBehaviour
{
    [Header("Game Object")]
    public GameObject robot;

    [Header("Input UI")]
    public TMPro.TMP_InputField inputField;     // Our Input Field UI

    private Movement simpleMovement;
    private ArduinoController controller;

    private List<string> robot_actions = new List<string> {};

    public Camera cam;                          

    private void Awake()
    {
        simpleMovement = robot.GetComponent<Movement>();
        controller = GetComponent<ArduinoController>();
    }

    private IEnumerator ProcessActions(string[] actions)
    {
        foreach (string action in actions)
        {
            string trimmedAction = action.Trim().ToLower();
            Thread.Sleep(500);

            switch (trimmedAction)
            {
                default:
                case "idle":
                    break;

                case "move forward":
                    yield return StartCoroutine(simpleMovement.Walking(Vector3.forward, robot));
                    robot_actions.Add("slideF");
                    break;

                case "move backward":
                    yield return StartCoroutine(simpleMovement.Walking(Vector3.back, robot));
                    robot_actions.Add("slideB");
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
        //controller.ControlArdunio("slideF");
        foreach (string robot_action in robot_actions)
        {
            Debug.Log("Reached");
            for (int i=1; i<=5; i++) // Sending 5 burts of IR signal
            {
                controller.ControlArdunio(robot_action);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string prompt = inputField.text;

            string[] actions = prompt.Split(',');

            StartCoroutine(ProcessActions(actions));

            inputField.text = "";
        }

        robot_actions.Clear();
    }
}