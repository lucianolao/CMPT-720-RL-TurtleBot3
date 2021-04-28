using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGoalManager : MonoBehaviour
{
    public Vector3 finalGoalMinPos, finalGoalMaxPos;


    void Start()
    {
        InitializeFinalGoal();
    }


    void Update()
    {
    }


    public void InitializeFinalGoal()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Static Obstacle");

        float positionX, positionZ;
        Vector3 finalGoalPosition;

        do {
            positionX = Random.Range(finalGoalMinPos.x, finalGoalMaxPos.x);
            positionZ = Random.Range(finalGoalMinPos.z, finalGoalMaxPos.z);
            finalGoalPosition = new Vector3(positionX, this.transform.position.y, positionZ);
        }
        while (!IsPositionValid(player, obstacles, finalGoalPosition, 2.0f, 1.0f));

        this.transform.position = finalGoalPosition;
    }


    private float GetFlatDistance(Vector3 vectorA, Vector3 vectorB)
    {
        vectorA.y = 0;
        vectorB.y = 0;

        return Vector3.Distance(vectorA, vectorB);
    }


    private bool IsPositionValid(GameObject player, GameObject[] obstacles, Vector3 position, float playerThreshold, float obstacleThreshold)
    {
        bool isValid = GetFlatDistance(position, player.transform.position) > playerThreshold;

        foreach (GameObject obstacle in obstacles)
        {
            if (!isValid) break;
            isValid = isValid && GetFlatDistance(position, obstacle.transform.position) > obstacleThreshold;
        }

        return isValid;
    }
}
