using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System;

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

    public Camera cam;                          

    private void Awake()
    {
        simpleMovement = robot.GetComponent<Movement>();
    }

    private IEnumerator ProcessActions(string[] actions)
    {
        foreach (string action in actions)
        {
            Debug.Log("HERE");
            Debug.Log(action);
            string trimmedAction = action.Trim().ToLower();
            Thread.Sleep(500);

            switch (trimmedAction)
            {
                default:
                case "idle":
                    break;

                case "step forward":
                    yield return StartCoroutine(simpleMovement.Walking(Vector3.forward, robot));
                    break;

                case "step back":
                    yield return StartCoroutine(simpleMovement.Walking(Vector3.back, robot));
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

            Debug.Log(prompt);

            string[] actions = SendToPython(prompt);

            StartCoroutine(ProcessActions(actions));

            inputField.text = "";
        }
    }

    // Send user prompt to python to input into ollama, recieve back list of commands
    private string[] SendToPython(string message)
    {
        try
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 65432))
            using (NetworkStream stream = client.GetStream())
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                byte[] responseData = new byte[1024];
                int bytes = stream.Read(responseData, 0, responseData.Length);
                string response = Encoding.ASCII.GetString(responseData, 0, bytes);
                Debug.Log("Response recieved from Python: " + response);

                string[] actions = response.Split(',');
                return actions;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to send data: " + ex.Message);
            return new string[0];
        }
    }
}