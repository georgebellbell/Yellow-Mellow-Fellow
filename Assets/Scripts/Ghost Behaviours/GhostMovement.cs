using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour
{

    [SerializeField]
    Fellow player;

    GhostTypes behaviour = new GhostTypes();

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    private string[] ghostStates = { "scatter", "patrol", "hunt", "dead" };
    private string currentState;

    public string ghostType;
 
    void Start()
    {
        currentState = ghostStates[1];

        agent = GetComponent<NavMeshAgent>();

        GotoNextPoint();

    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case "scatter": UpdateScatter(); break;
            case "patrol":  UpdatePatrol(); break;
            case "hide":    UpdateHide(); break;
            case "hunt":    UpdateHunt(); break;
            case "dead":    UpdateDead(); break;

        }
       
    }

    void UpdateScatter()
    {
        
    }
    void UpdatePatrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
    void UpdateHunt()
    {
        switch (ghostType)
        {
            case "ambusher": Debug.Log(behaviour.ambusher()); break;
            case "stalker": Debug.Log(behaviour.stalker()); break;
            case "hunt": UpdateHunt(); break;
            case "dead": UpdateDead(); break;

        }
    }
    void UpdateHide()
    {
        agent.destination = PickHidingPlace();
    }
    void UpdateDead()
    {

    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;

        destPoint = (destPoint + 1) % points.Length;
    }

    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f), out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }

}
