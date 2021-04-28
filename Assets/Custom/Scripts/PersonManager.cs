using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersonManager : MonoBehaviour
{
    public GameObject person;
    public Vector3 personMinPos, personMaxPos;
    public int personCount = 10;
    public float personSpeedVariation = 0.2f;

    private GameObject[] personInstances;


    void Start()
    {
        personInstances = new GameObject[personCount];
        InitializePersons();
    }


    void Update()
    {
    }


    public GameObject[] GetPersons()
    {
        return personInstances;
    }


    public void InitializePersons()
    {
        GameObject finalGoal = GameObject.FindWithTag("Final Goal");
        GameObject player = GameObject.FindWithTag("Player");
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Static Obstacle");

        GameObject personInstance;
        PersonController personController;
        Vector3 personPosition;
        float positionX, positionZ;

        for (int personIndex = 0; personIndex < personCount; personIndex++)
        {
            personInstance = GameObject.Instantiate(person);
            personController = personInstance.GetComponent<PersonController>();

            do {
                positionX = Random.Range(personMinPos.x, personMaxPos.x);
                positionZ = Random.Range(personMinPos.z, personMaxPos.z);
                personPosition = new Vector3(positionX, personInstance.transform.position.y, positionZ);
            }
            while (!IsPositionValid(finalGoal, player, obstacles, personPosition, 0.5f, 2.0f, 0.5f));

            personController.SetTransformPosition(personPosition);
            personController.SetSpeed(Random.Range(personController.speed - personSpeedVariation, personController.speed + personSpeedVariation));

            personInstances[personIndex] = personInstance;
        }
    }


    public void ResetPersons()
    {
        for (int personIndex = 0; personIndex < personCount; personIndex++)
        {
            Destroy(personInstances[personIndex]);
        }

        InitializePersons();
    }



    private float GetFlatDistance(Vector3 vectorA, Vector3 vectorB)
    {
        vectorA.y = 0;
        vectorB.y = 0;

        return Vector3.Distance(vectorA, vectorB);
    }


    private bool IsPositionValid(GameObject finalGoal, GameObject player, GameObject[] obstacles, Vector3 position, float finalGoalThreshold, float playerThreshold, float obstacleThreshold)
    {
        bool isValid = GetFlatDistance(position, finalGoal.transform.position) > finalGoalThreshold;
        isValid = isValid && GetFlatDistance(position, player.transform.position) > playerThreshold;

        foreach (GameObject obstacle in obstacles)
        {
            if (!isValid) break;
            isValid = isValid && GetFlatDistance(position, obstacle.transform.position) > obstacleThreshold;
        }

        return isValid;
    }
}
