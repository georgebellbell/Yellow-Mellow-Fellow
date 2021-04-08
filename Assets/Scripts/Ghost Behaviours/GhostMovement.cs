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
        Debug.Log("Patrolling!");
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
    void UpdateHunt()
    {
        //TODO 
        switch (ghostType)
        {
            case "ambusher":
                Debug.Log(behaviour.ambusher());
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GotoNextPoint();
                break;
            case "stalker": Debug.Log(behaviour.stalker()); break;
            case "TBA": UpdateHunt(); break;
            case "TBA2": UpdateDead(); break;

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



