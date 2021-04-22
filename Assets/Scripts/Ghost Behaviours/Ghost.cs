using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{

    [SerializeField] float trackingDuration = 3.0f;

    [SerializeField] Material scaredMaterial, deadMaterial;
    Material normalMaterial;

    Fellow player;

    public Transform[] waypoints;
    GhostBehaviour behaviour;
    NavMeshAgent agent;
    Collider g_collider, gh_collider;
    GameObject GhostHouse;
    Vector3 ghostHouseVector;

    int destPoint = 0;

    enum GhostState
    {
        normal,
        hide,
        dead
    }
    GhostState ghostState = GhostState.normal;

    void Start()
    {
        behaviour = GetComponent<GhostBehaviour>();

        normalMaterial = GetComponent<Renderer>().material;

        player = GameObject.Find("Fellow").GetComponent<Fellow>();

        GhostHouse = GameObject.Find("GhostHouse");
        ghostHouseVector = GhostHouse.transform.position;

        agent = GetComponent<NavMeshAgent>();
        g_collider = GetComponent<Collider>();
        gh_collider = GhostHouse.GetComponent<Collider>();
    }



    void Update()
    {
        if (ghostState == GhostState.dead)
        {
            UpdateDead();
            return;
        }

        gameObject.layer = LayerMask.NameToLayer("Ghost");

        if (player.PowerupActive())
        {
            UpdateHide();
            return;
        }

        ghostState = GhostState.normal;
        GetComponent<Renderer>().material = normalMaterial;

        //WAVED MOVEMENT
        if (Time.time <= 7 || Time.time > 27 && Time.time <= 34 ||
            Time.time > 54 && Time.time <= 59 || Time.time > 79 && Time.time <= 84)
            UpdatePatrol();
        else
            UpdateHunt();
    }

    void UpdatePatrol()
    {
        if (CanSeePlayer())
        {
            agent.destination = player.transform.position;
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
                
        }
    }
    bool CanSeePlayer()
    {
        Vector3 rayPos = transform.position;
        Vector3 rayDir = (player.transform.position - rayPos).normalized;

        RaycastHit info;
        if (Physics.Raycast(rayPos, rayDir, out info))
        {
            if (info.transform.CompareTag("Fellow")) return true;
        }
        return false;
    }
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (waypoints.Length == 0)
            return;
        agent.destination = waypoints[destPoint].position;

        destPoint = (destPoint + 1) % waypoints.Length;
    }

   

    void UpdateHunt()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f || trackingDuration < 0.0f)
        {
            trackingDuration = 5.0f;
            agent.destination = behaviour.GetTarget();
        }
       
        trackingDuration = Mathf.Max(0.0f, trackingDuration - Time.deltaTime);
    }

    void UpdateHide()
    {
        GetComponent<Renderer>().material = scaredMaterial;
        if (agent.remainingDistance < 0.5f)
            agent.destination = PickHidingPlace();
    }

    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f), out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }

    void UpdateDead()
    {
        agent.destination = ghostHouseVector;
        if (g_collider.bounds.Intersects(gh_collider.bounds)) ghostState = GhostState.normal;   
    }

    public void toSpawn()
    {
        agent.Warp(ghostHouseVector);
    }


    //----------------------------------------------GHOST INTERACTION-------------------------------------------------//



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fellow"))
        {
            GetComponent<Rigidbody>().isKinematic = true;

            if (player.PowerupActive()) ghostDies();
        }
    }
    void ghostDies()
    {
        ghostState = GhostState.dead;
        gameObject.layer = LayerMask.NameToLayer("DeadGhost");
        GetComponent<Renderer>().material = deadMaterial;
    }
}



