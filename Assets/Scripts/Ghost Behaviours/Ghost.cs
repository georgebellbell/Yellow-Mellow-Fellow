using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    public float trackingDuration = 1f;
    public Transform[] waypoints;

    public Material scaredMaterial;
    public Material deadMaterial;
    public Material slowMaterial;
    Material normalMaterial;

    Fellow player;

    public GameObject GhostHouse;
    Vector3 ghostHouseVector;

    GhostBehaviour behaviour;
    NavMeshAgent agent;
    Collider g_collider, gh_collider;

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
        agent = GetComponent<NavMeshAgent>();
        g_collider = GetComponent<Collider>();
        gh_collider = GhostHouse.GetComponent<Collider>();
        player = GameObject.Find("Fellow").GetComponent<Fellow>();

        GhostHouse = GameObject.Find("GhostHouse");
        ghostHouseVector = GhostHouse.transform.position;    
    }

    void Update()
    {
        // agent is only deactivated when game is paused or ended
        if (!agent.isActiveAndEnabled)
            return;
        
        // if ghost is dead, move towards spawn
        if (ghostState == GhostState.dead)
        {
            agent.destination = ghostHouseVector;
            if (g_collider.bounds.Intersects(gh_collider.bounds)) ghostState = GhostState.normal;
            return;
        }
       

        gameObject.layer = LayerMask.NameToLayer("Ghost");

        if (player.IsTimeslowActive())
        {
            agent.speed = 1.75f;
        }
        else
        {
            agent.speed = 3.5f;
        }

        // if player has a powerup active, ghost will try to hide
        if (player.IsPowerupActive())
        {
            UpdateHide();
            return;
        }

        ghostState = GhostState.normal;
        GetComponent<Renderer>().material = normalMaterial;

        //WAVED MOVEMENT
        if (Time.time <= 7 || Time.time > 27 && Time.time <= 34 ||Time.time > 54 && Time.time <= 59 || Time.time > 79 && Time.time <= 84)
            UpdatePatrol();
        else
            UpdateHunt();

        trackingDuration = Mathf.Max(0.0f, trackingDuration - Time.deltaTime);

    }

    void UpdateHide()
    {
        if (ghostState != GhostState.hide || agent.remainingDistance < 1f)
        {
            ghostState = GhostState.hide;
            agent.destination = PickHidingPlace();
            GetComponent<Renderer>().material = scaredMaterial;
        }
        
    }
    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f), out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }

    // Ghost will follow a set path but if it spots player, will chase after them until losing a direct line of sight
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
    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (waypoints.Length == 0)
            return;
        agent.destination = waypoints[destPoint].position;

        destPoint = (destPoint + 1) % waypoints.Length;
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



    void UpdateHunt()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f || trackingDuration < 0.0f)
        {
            trackingDuration = 2.5f;
            agent.destination = behaviour.GetTarget();
        }
       
        
    }

    // Called by YellowFellowGame.cs, resets ghost to spawn when player dies
    public void toSpawn()
    {
        agent.Warp(ghostHouseVector);
    }

    // Called by YellowFellowGame.cs, used to pause and start ghosts depending if game is paused or the game has ended
    public void SetGhostSpeed(float speed)
    {
        agent.speed = speed;
    }


    //----------------------------------------------GHOST INTERACTION-------------------------------------------------//



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fellow"))
        {
            GetComponent<Rigidbody>().isKinematic = true;

            if (player.IsPowerupActive()) ghostDies();
        }
    }
    void ghostDies()
    {
        ghostState = GhostState.dead;
        gameObject.layer = LayerMask.NameToLayer("DeadGhost");
        GetComponent<Renderer>().material = deadMaterial;
    }
}



