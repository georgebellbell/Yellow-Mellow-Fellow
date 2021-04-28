using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleporters : MonoBehaviour
{
    public Transform otherTeleporter;
    int offset;

    AudioSource audioSource;
    public AudioClip teleportSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
            audioSource.PlayOneShot(teleportSound);
            other.transform.position = destination;
        }
        else if (other.CompareTag("Ghost"))
        {
            audioSource.PlayOneShot(teleportSound);
            NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
            agent.Warp(destination);
        }
        
        
    }
}
