using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersonController : MonoBehaviour
{
    public float boundaryDistanceThreshold = 1.0f;
    public float obstacleDistanceThreshold = 1.0f;
    public float targetDistanceThreshold = 1.0f;
    public float speed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float candidateAngleOffset = 20.0f;
    public float candidateDistanceThreshold = 5.0f;
    public float maxRaycastHitDistance = 50.0f;
    public float raycastSphereRadius = 0.5f;

    private bool isTargetLocked;
    private Vector3 currentTargetPosition;


    void Start()
    {
        currentTargetPosition = GetNextTargetPosition(transform);
        isTargetLocked = false;
    }


    void Update()
    {
        MoveToTarget();
    }


    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }


    public void SetTransformPosition(Vector3 position)
    {
        transform.position = position;
    }


    private Vector3 GetNextTargetPosition(Transform previousTarget)
    {
        int candidateCount = (int) Mathf.Floor(360.0f / candidateAngleOffset);
        float candidateAngle = 0.0f, candidateDistance = 0.0f, maxCandidateDistance = 0.0f;
        Vector3 bestTargetPosition = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 currentTargetRotation;
        RaycastHit viewHit;

        for (int candidateIndex = 0; candidateIndex < candidateCount; candidateIndex++)
        {
            candidateDistance = 0.0f;
            currentTargetRotation = Quaternion.AngleAxis(candidateAngle, previousTarget.up) * previousTarget.forward;

            if (Physics.SphereCast(previousTarget.transform.position, raycastSphereRadius, currentTargetRotation, out viewHit, maxRaycastHitDistance) &&
                (viewHit.transform.CompareTag("Static Obstacle") || viewHit.transform.CompareTag("Static Boundary")))
            {
                candidateDistance = viewHit.distance;
            }

            if (candidateDistance > maxCandidateDistance)
            {
                maxCandidateDistance = candidateDistance;
                bestTargetPosition = new Vector3(viewHit.point.x, transform.position.y, viewHit.point.z);

                if (maxCandidateDistance > candidateDistanceThreshold)
                {
                    break;
                }
            }

            candidateAngle += candidateAngleOffset;
        }

        return bestTargetPosition;
    }


    private void MoveToTarget()
    {
        if (Vector3.Distance(transform.position, currentTargetPosition) < targetDistanceThreshold)
        {
            currentTargetPosition = GetNextTargetPosition(transform);
        }

        RaycastHit viewHit;

        if (!isTargetLocked && Physics.SphereCast(transform.position, raycastSphereRadius, transform.forward, out viewHit, maxRaycastHitDistance) &&
            ((viewHit.transform.CompareTag("Static Boundary") && viewHit.distance < boundaryDistanceThreshold) ||
            (viewHit.transform.CompareTag("Static Obstacle") && viewHit.distance < obstacleDistanceThreshold)))
        {
            currentTargetPosition = GetNextTargetPosition(transform);
            isTargetLocked = true;
        }
        else
        {
            isTargetLocked = false;
        }

        Quaternion targetRotation = Quaternion.LookRotation(currentTargetPosition - transform.position);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
