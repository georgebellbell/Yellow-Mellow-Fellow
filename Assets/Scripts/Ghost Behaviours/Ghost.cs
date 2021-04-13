using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost: MonoBehaviour
{
   
    Fellow player;

    GhostTypes behaviour = new GhostTypes();

    public Transform[] waypoints;
    private int destPoint = 0;
    private NavMeshAgent agent;
    enum GhostState
    {
        normal,
        hide,
        dead
    }
    GhostState ghostState = GhostState.normal;
   
    string ghostType;

    GameObject GhostHouse;
    Vector3 ghostHouseVector;

    [SerializeField]
    Material scaredMaterial, deadMaterial;

    Material normalMaterial;

    Collider g_collider, gh_collider;

    void Start()
    {
        normalMaterial = GetComponent<Renderer>().material;

        player = GameObject.Find("Fellow").GetComponent<Fellow>();
        ghostType = gameObject.name;

        GhostHouse = GameObject.Find("GhostHouse");
        ghostHouseVector = GhostHouse.transform.position;
        
        agent = GetComponent<NavMeshAgent>();
        g_collider = GetComponent<Collider>();
        gh_collider = GhostHouse.GetComponent<Collider>();
    }

    void Update()
    {
       
        if (ghostState == GhostState.dead)
            UpdateDead();

        else
        {
            gameObject.layer = LayerMask.NameToLayer("Ghost");
            if (player.PowerupActive()) 
                UpdateHide();
            else
            {
                ghostState = GhostState.normal;
                GetComponent<Renderer>().material = normalMaterial;
      
                //WAVED MOVEMENT
                if (Time.time <= 7 || Time.time > 27 && Time.time <= 34 ||
                Time.time > 54 && Time.time <= 59 || Time.time > 79 && Time.time <= 84)
                    UpdatePatrol();
                else
                    UpdateHunt();   
            }
        }
    }

    //----------------------------------------------GHOST MOVEMENT-------------------------------------------------//
    void UpdatePatrol()
    {
        if (CanSeePlayer())
        {
            Debug.Log("I can see you!");
            agent.destination = player.transform.position;
        }
        else
        {
           // Debug.Log("Patrolling!");
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
        
    }
    void UpdateHunt()
    {
        //TODO 
        switch (ghostType)
        {
            case "pink":
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    agent.destination = behaviour.ambusher(player.transform.position, player.getDirection());
                break;
            case "red":
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    agent.destination = behaviour.stalker(player.transform.position);
                break;
                //needs further look at
            case "orange": 
                //if more than 4 units away will move straight for player
                if (Vector3.Distance(player.transform.position, transform.position) > 4.0f)
                {
                    agent.destination = behaviour.stalker(player.transform.position);
                }
                // but when it gets too close, will move in random direction
                else
                {
                    if (!agent.pathPending && agent.remainingDistance < 0.5f)
                        agent.destination = PickRandomPosition();
                }
                break;
            case "cyan":
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    agent.destination = behaviour.teamplayer(player.transform.position, player.getDirection());
                break;

        }
    }
    void UpdateHide()
    {
        GetComponent<Renderer>().material = scaredMaterial;
        if (agent.remainingDistance < 0.5f)
            agent.destination = PickHidingPlace();
       
    }

   
    void UpdateDead()
    {
        agent.destination = ghostHouseVector;
        if (g_collider.bounds.Intersects(gh_collider.bounds)) ghostState = GhostState.normal;   
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (waypoints.Length == 0)
            return;

        agent.destination = waypoints[destPoint].position;

        destPoint = (destPoint + 1) % waypoints.Length;
    }

    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f), out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }
    Vector3 PickRandomPosition()
    {
        Vector3 destination = transform.position;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * 8.0f;
        destination.x += randomDirection.x;
        destination.y += randomDirection.y;

        NavMeshHit navHit;
        NavMesh.SamplePosition(destination, out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
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
    void ghostDies()
    {
        ghostState = GhostState.dead;
        gameObject.layer = LayerMask.NameToLayer("DeadGhost");
        GetComponent<Renderer>().material = deadMaterial;
    }



}



