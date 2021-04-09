using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleporters : MonoBehaviour
{
    public Transform otherTeleporter;
    int offset;
    private void OnTriggerEnter(Collider other)
    {
        if (transform.position.x < otherTeleporter.position.x)
        {
            offset = -1;
        }
        else
        {
            offset = +1;
        }
        Vector3 destination = otherTeleporter.position;
        destination.x = destination.x + offset;
        if (other.CompareTag("Fellow"))
        {
            other.transform.position = destination;
        }
        else if (other.CompareTag("Ghost"))
        {
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            agent.Warp(destination);
        }
        
        
    }
}
