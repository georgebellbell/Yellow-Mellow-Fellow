using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostBehaviour: MonoBehaviour
{
    public string ghostType;
    Vector3 target;
    Fellow player;

    private void Start()
    {
        player = GameObject.Find("Fellow").GetComponent<Fellow>();
    }

    public Vector3 GetTarget()
    {
        switch (ghostType)
        {
            case "pink":
                   target = Ambusher(player.transform.position, player.GetDirection());
                break;
            case "red":
                target = player.transform.position;
                break;
            case "orange":
                target = Coward(player.transform.position);
                break;
            case "cyan":
                    target = Teamplayer(player.transform.position, player.GetDirection());
                break;

        }
        return target;
    }

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
   
    Vector3 Ambusher(Vector3 playerPosition,string playerDirection)
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

    Vector3 Coward(Vector3 playerPosition)
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
    Vector3 Teamplayer(Vector3 playerPosition, string playerDirection)
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
        return target;
    }
}
