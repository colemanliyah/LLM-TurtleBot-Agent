using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float distance = 1f;
    public float stepSize = 0.2f;
    public float stepDuration = 0.1f;
    public float turnAngle = 90f;
    private GameObject robot;

    public IEnumerator ProcessInput(string input, GameObject robot_obj)
    {
        robot = robot_obj;
        string[] actions = input.Split(',');

        foreach (string action in actions)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                yield break;
            }

            string trimmedAction = action.Trim().ToLower();
            if (trimmedAction == "move forward")
            {
                yield return Walking(Vector3.forward);
            }
            else if (trimmedAction == "move backward")
            {
                yield return Walking(Vector3.back);
            }
            else if (trimmedAction == "slide forward")
            {
                yield return Sliding(Vector3.forward);
            }
            else if (trimmedAction == "slide backward")
            {
                yield return Sliding(Vector3.back);
            }
            else if (trimmedAction == "turn right")
            {
                Turn(turnAngle);
            }
            else if (trimmedAction == "turn left")
            {
                Turn(-turnAngle);
            }
        }

        yield break;
    }

    IEnumerator Walking(Vector3 direction)
    {
        Vector3 targetPosition = robot.transform.position + direction * distance;
        while (Vector3.Distance(robot.transform.position, targetPosition) > 0.01f)
        {
            Vector3 stepTarget = robot.transform.position + direction * stepSize;
            if (Vector3.Distance(stepTarget, targetPosition) > Vector3.Distance(robot.transform.position, targetPosition))
            {
                stepTarget = targetPosition;
            }

            float elapsedTime = 0f;
            Vector3 startPosition = robot.transform.position;

            while (elapsedTime < stepDuration)
            {
                robot.transform.position = Vector3.Lerp(startPosition, stepTarget, elapsedTime / stepDuration);
                elapsedTime += Time.deltaTime;
                yield return null; 
            }

            robot.transform.position = stepTarget; // Ensure it reaches the step target

            // Small pause between steps to simulate stepping
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Sliding(Vector3 direction)
    {
        Vector3 targetPosition = robot.transform.position + direction * distance;
        while (Vector3.Distance(robot.transform.position, targetPosition) > 0.01f)
        {
            robot.transform.position = Vector3.MoveTowards(robot.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; 
        }
    }

    void Turn(float turnAngle)
    {
        robot.transform.Rotate(Vector3.up, turnAngle);
    }
}