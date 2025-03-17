// Author: Peter Dickx https://github.com/dickxpe
// MIT License - Copyright (c) 2024 Peter Dickx

using UnityEngine;
using UltEvents;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentEvents : MonoBehaviour
{

    public Transform destination;

    Vector3 prevPosition;

    public UltEvent onDestinationReached;

    private bool hasReachedDestination = false;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (destination)
        {
            agent.SetDestination(destination.position);
        }
    }

    private bool hasStartedMoving = false;


    public void SetDestination(Transform destination)
    {
        this.destination = destination;
        if (!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.SetDestination(this.destination.position);
    }

    void Update()
    {
        if (agent == null || !agent.enabled)
            return;


        if (prevPosition != destination.position)
        {
            Debug.Log("DESTINATION");
            agent.SetDestination(destination.position);
        }

        prevPosition = destination.position;

        FaceTarget();

        // Ensure the agent has started moving before checking for arrival
        if (!hasStartedMoving && agent.velocity.sqrMagnitude > 0.01f)
        {
            hasStartedMoving = true;
        }

        // Check if the agent has reached its destination and stopped moving
        if (hasStartedMoving && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!hasReachedDestination && (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f))
            {
                hasReachedDestination = true;
                StopAgent();  // Ensure agent stops moving completely
                onDestinationReached?.Invoke();
            }
        }
        else if (agent.remainingDistance > agent.stoppingDistance)
        {
            hasReachedDestination = false;
        }
    }

    void FaceTarget()
    {
        if (agent.velocity.magnitude > 0.1)
        {
            Vector3 direction = agent.velocity;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        }
    }

    private void StopAgent()
    {
        agent.velocity = Vector3.zero;  // Stop all movement
        agent.isStopped = true;         // Pause pathfinding
    }
}