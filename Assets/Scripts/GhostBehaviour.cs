using UnityEngine;
using UnityEngine.AI;

public class GhostBehaviour: MonoBehaviour
{
    //determines what method the ghost will hunt with
    public string ghostType;

    Vector3 target;
    FellowMovement player;

    private void Start()
    {
        player = GameObject.Find("Fellow").GetComponent<FellowMovement>();
    }

    // called by main Ghost class when ghost is hunting the player
    public Vector3 GetTarget()
    {
        switch (ghostType)
        {
            case "pink":
                  target = PinkGhost(player.transform.position, player.GetDirection());
                break;
            case "red":
                target = player.transform.position;
                break;
            case "orange":
                target = OrangeGhost(player.transform.position);
                break;
            case "cyan":
                target = CyanGhost(player.transform.position, player.GetDirection());
                break;

        }
        return target;
    }

    // finds a target ahead of player by using the players current direction
    Vector3 PinkGhost(Vector3 playerPosition,string playerDirection)
    {
        target = playerPosition;

        switch (playerDirection)
        {
            case "left": target.x = target.x -2; break;
            case "right": target.x = target.x + 2; break;
            case "up": target.z = target.z + 2; break;
            case "down": target.z = target.z - 2; break;
        }
        return target;
    }

    // acts like red ghost until within certain distance, otherwise in a random direction
    Vector3 OrangeGhost(Vector3 playerPosition)
    {
        //if more than 4 units away will move straight for player
        if (Vector3.Distance(playerPosition, transform.position) > 4.0f)
        {
            return playerPosition;
        }
        // but when it gets too close, will move in random direction
        else
        {
            return PickRandomPosition();
        }
    }

    // position determined by both player and red ghost, moving to point halfway between the two
    Vector3 CyanGhost(Vector3 playerPosition, string playerDirection)
    {
        Vector3 stalkerLocation = GameObject.Find("red").transform.position;
        target = stalkerLocation;
        switch (playerDirection)
        {
            case "left": playerPosition.x = playerPosition.x - 1; break;
            case "right": playerPosition.x = playerPosition.x + 1; break;
            case "up": playerPosition.z = playerPosition.z + 1; break;
            case "down": playerPosition.z = playerPosition.z - 1; break;
        }
        target.x = target.x + ((stalkerLocation.x - playerPosition.x) * 0.5f);
        target.z = target.z + ((stalkerLocation.z - playerPosition.z) * 0.5f);

        //point determined by vector is used to sample navmesh for a valid target
        NavMeshHit navHit;
        NavMesh.SamplePosition(target, out navHit, 8.0f, NavMesh.AllAreas);
        return navHit.position;
    }

    // selects a random position that is on the navmesh
    public Vector3 PickRandomPosition()
    {
        Vector3 destination = transform.position;
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle * 8.0f;
        destination.x += randomDirection.x;
        destination.y += randomDirection.y;

        NavMeshHit navHit;
        NavMesh.SamplePosition(destination, out navHit, 8.0f, NavMesh.AllAreas);

        return navHit.position;
    }
}
