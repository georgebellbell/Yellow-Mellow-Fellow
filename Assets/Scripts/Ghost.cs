using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update

    [SerializeField]
    Fellow playerObject;

    [SerializeField]
    Material scaredMaterial;

    [SerializeField]
    Material deadMaterial;


    Material normalMaterial;

    [SerializeField]
    GameObject GhostHouse;

    Vector3 ghostHouseVector;

    Vector3 ghostSpawn;

    void Start()
    {
        ghostSpawn = gameObject.transform.position;
        ghostHouseVector = GhostHouse.transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = PickRandomPosition();
        normalMaterial = GetComponent<Renderer>().material;
    }

    bool hiding = false;
    // Update is called once per frame
    void Update()
    {
        if (gameObject.layer == LayerMask.NameToLayer("DeadGhost"))
        {
            agent.destination = ghostHouseVector;
            GetComponent<Renderer>().material = deadMaterial;
            Debug.Log("I am dead!");
        }
        else
        {
            if (player.PowerupActive())
            {
                UpdateHide();
            }
            else
            {
                UpdateChase();
            }
        }
        
    }

    void UpdateChase()
    {
        if (hiding)
        {
            GetComponent<Renderer>().material = normalMaterial;
        }

        if (agent.remainingDistance < 0.5f)
        {
            agent.destination = PickRandomPosition();
            hiding = false;
            GetComponent<Renderer>().material = normalMaterial;
        }
        if (CanSeePlayer())
        {
            Debug.Log("I can see you!");
            agent.destination = player.transform.position;
        }
        else
        {
            if (agent.remainingDistance < 0.5f)
            {
                agent.destination = PickRandomPosition();
            }

        }
    }

    void UpdateHide()
    {
        if (!hiding || agent.remainingDistance < 0.5f)
        {
            hiding = true;
            agent.destination = PickHidingPlace();
            GetComponent<Renderer>().material = scaredMaterial;
        }
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

    [SerializeField]
    Fellow player;

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

    Vector3 PickHidingPlace()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        NavMeshHit navHit;
        NavMesh.SamplePosition(transform.position - (directionToPlayer * 8.0f), out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }


    [SerializeField]
    BoxCollider LeftTeleporter;

    [SerializeField]
    BoxCollider RightTeleporter;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 pos = transform.position;


        if (other.gameObject.CompareTag("LeftTeleporter"))
        {
            pos = RightTeleporter.ClosestPoint(pos);
            Debug.Log("LeftTeleporter found");
            pos.x = pos.x - 0.5f;
            transform.position = pos;
        }

        if (other.gameObject.CompareTag("RightTeleporter"))
        {
            pos = LeftTeleporter.center;
            Debug.Log("RightTeleporter found");
            pos.x = pos.x + 0.5f;
            transform.position = pos;
        }
        if(other.gameObject.CompareTag("GhostHouse")&& gameObject.layer == LayerMask.NameToLayer("DeadGhost"))
        {
            gameObject.layer = LayerMask.NameToLayer("Ghost");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fellow"))
        {
            if (player.PowerupActive())
            {
                gameObject.layer = LayerMask.NameToLayer("DeadGhost");
            }
            else
            {
                gameObject.transform.position = ghostSpawn;
            }
                           
        }
    }

   
}
