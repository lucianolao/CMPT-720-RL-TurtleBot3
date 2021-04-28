using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleManager : MonoBehaviour
{
    public GameObject[] obstacles;
    public Vector3 obstacleMinPos, obstacleMaxPos;
    public int obstacleCount = 10;

    private GameObject[] obstacleInstances;


    void Start()
    {
        obstacleInstances = new GameObject[obstacleCount];
        InitializeObstacles();
    }


    void Update()
    {
    }


    public GameObject[] GetObstacles()
    {
        return obstacleInstances;
    }


    public void InitializeObstacles()
    {
        GameObject finalGoal = GameObject.FindWithTag("Final Goal");
        GameObject player = GameObject.FindWithTag("Player");

        GameObject obstacleInstance;
        Vector3 obstaclePosition;
        int randomIndex;
        float positionX, positionZ;

        for (int obstacleIndex = 0; obstacleIndex < obstacleCount; obstacleIndex++)
        {
            randomIndex = Random.Range(0, obstacles.Length);
            obstacleInstance = GameObject.Instantiate(obstacles[randomIndex]);

            do {
                positionX = Random.Range(obstacleMinPos.x, obstacleMaxPos.x);
                positionZ = Random.Range(obstacleMinPos.z, obstacleMaxPos.z);
                obstaclePosition = new Vector3(positionX, obstacleInstance.transform.position.y, positionZ);
            }
            while (!IsPositionValid(finalGoal, player, obstaclePosition, 1.0f, 2.0f, 1.0f));

            obstacleInstance.transform.position = obstaclePosition;
            obstacleInstances[obstacleIndex] = obstacleInstance;
        }
    }


    public void ResetObstacles()
    {
        for (int obstacleIndex = 0; obstacleIndex < obstacleCount; obstacleIndex++)
        {
            Destroy(obstacleInstances[obstacleIndex]);
        }

        InitializeObstacles();
    }


    private float GetFlatDistance(Vector3 vectorA, Vector3 vectorB)
    {
        vectorA.y = 0;
        vectorB.y = 0;

        return Vector3.Distance(vectorA, vectorB);
    }


    private bool IsPositionValid(GameObject finalGoal, GameObject player, Vector3 position, float finalGoalThreshold, float playerThreshold, float obstacleThreshold)
    {
        bool isValid = GetFlatDistance(position, finalGoal.transform.position) > finalGoalThreshold;
        isValid = isValid && GetFlatDistance(position, player.transform.position) > playerThreshold;

        foreach (GameObject obstacle in obstacleInstances)
        {
            if (!isValid) break;
            if (obstacle)
            {
                isValid = isValid && GetFlatDistance(position, obstacle.transform.position) > obstacleThreshold;
            }
        }

        return isValid;
    }
}
