using UnityEngine;
using System.Collections;

/// <summary>
/// This class is used to write out the commands of actions the robot can perform
/// </summary>
public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float distance = 1f;
    public float stepSize = 0.2f;
    public float stepDuration = 0.1f;
    public float turnAngle = 90f;

    public IEnumerator Walking(Vector3 direction, GameObject robot)
    {
        // Transform direction to local space
        Vector3 localDirection = robot.transform.TransformDirection(direction);

        Vector3 targetPosition = robot.transform.position + localDirection * distance;
        while (Vector3.Distance(robot.transform.position, targetPosition) > 0.01f)
        {
            Vector3 stepTarget = robot.transform.position + localDirection * stepSize;
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

    public IEnumerator Sliding(Vector3 direction, GameObject robot)
    {
        // Transform direction to local space
        Vector3 localDirection = robot.transform.TransformDirection(direction);

        Vector3 targetPosition = robot.transform.position + localDirection * distance;
        while (Vector3.Distance(robot.transform.position, targetPosition) > 0.01f)
        {
            robot.transform.position = Vector3.MoveTowards(robot.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        } 
    }

    public IEnumerator Turn(bool rightTurn, GameObject robot)
    {
        if (rightTurn)
        {
            robot.transform.Rotate(Vector3.up, turnAngle);
        }
        else
        {
            robot.transform.Rotate(Vector3.up, -turnAngle);
        }
        yield return null;
    }
}