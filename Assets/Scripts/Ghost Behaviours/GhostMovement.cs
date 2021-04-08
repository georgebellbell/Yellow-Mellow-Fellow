using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostMovement: MonoBehaviour
{
    Collider g_collider;

    [SerializeField]
    Fellow player;

    GhostTypes behaviour = new GhostTypes();

    public Transform[] points;
    private int destPoint = 0;
    private NavMeshAgent agent;

    private string[] ghostStates = {"normal", "hide", "dead" };
    private string currentState;

    public string ghostType;

    [SerializeField]
    GameObject GhostHouse;
    Vector3 ghostHouseVector;
    Collider gh_collider;

    [SerializeField]
    Material scaredMaterial;

    [SerializeField]
    Material deadMaterial;

    Material normalMaterial;

    
    void Start()
    {
        ghostHouseVector = GhostHouse.transform.position;
        normalMaterial = GetComponent<Renderer>().material;
        g_collider = GetComponent<Collider>();
        gh_collider = GhostHouse.GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update()
    {
        if(currentState == ghostStates[2])
        {
            UpdateDead();
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Ghost");
            if (player.PowerupActive())
            {
                currentState = ghostStates[1];
                GetComponent<Renderer>().material = scaredMaterial;
                UpdateHide();
            }
            else
            {

                GetComponent<Renderer>().material = normalMaterial;
                currentState = ghostStates[0];

                //WAVED MOVEMENT
                if (Time.time <= 7 || Time.time > 27 && Time.time <= 34 ||
                Time.time > 54 && Time.time <= 59 || Time.time > 79 && Time.time <= 84)
                {
                    
                    UpdatePatrol();
                }
                else
                {
                   
                    UpdateHunt();
                }
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
            Debug.Log("Patrolling!");
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
        }
        
    }
    void UpdateHunt()
    {
        //TODO 
        switch (ghostType)
        {
            case "ambusher":
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    agent.destination = behaviour.ambusher(player.transform.position, player.getDirection());
                break;
            case "stalker":
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    agent.destination = behaviour.stalker(player.transform.position);
                break;
                //needs further look at
            case "coward": 
                //if more than 4 units away will move straight for player
                if (Vector3.Distance(player.transform.position, transform.position) > 4.0f)
                {
                    agent.destination = behaviour.stalker(player.transform.position);
                }
                // but when it gets too close, will get scared and return to patrol route
                else
                {
                    Debug.Log("WOAH TOO CLOSE RUN AWAY");
                    GotoNextPoint();
                }
                break;
            case "teamplayer":
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    agent.destination = behaviour.teamplayer(player.transform.position, player.getDirection());
                break;

        }
    }
    void UpdateHide()
    {
        if (agent.remainingDistance < 0.5f)
        {
            agent.destination = PickHidingPlace();
        }
    }

    void ghostDies()
    {
        currentState = ghostStates[2];
        gameObject.layer = LayerMask.NameToLayer("DeadGhost");
        GetComponent<Renderer>().material = deadMaterial;
    }
    void UpdateDead()
    {
        agent.destination = ghostHouseVector;
        if (g_collider.bounds.Intersects(gh_collider.bounds))
        {
            currentState = ghostStates[0];
        }
        
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

    bool CanSeePlayer()
    {
        Vector3 rayPos = transform.position;
        Vector3 rayDir = (player.transform.position - rayPos).normalized;

        RaycastHit info;
        if (Physics.Raycast(rayPos, rayDir, out info))
        {
            if (info.transform.CompareTag("Fellow"))
            {
                return true;
            }
        }

        return false;
    }


    //----------------------------------------------GHOST INTERACTION-------------------------------------------------//



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fellow"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            if (player.PowerupActive())
            {

                ghostDies();
            }
            else
            {
                gameObject.transform.position = ghostHouseVector;
            }

        }
    }

}



