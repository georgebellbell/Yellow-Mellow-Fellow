using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTypes
{

    Vector3 target;
    public Vector3 stalker(Vector3 playerPosition)
    {
        target = playerPosition;
        return target;
    }

    public Vector3 ambusher(Vector3 playerPosition,string playerDirection)
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
    public Vector3 teamplayer(Vector3 playerPosition, string playerDirection)
    {

        Vector3 stalkerLocation = GameObject.Find("stalker").transform.position;
        target = stalkerLocation;
        switch (playerDirection)
        {
            case "left": playerPosition.x = playerPosition.x - 1; break;
            case "right": playerPosition.x = playerPosition.x + 1; break;
            case "up": playerPosition.z = playerPosition.z + 1; break;
            case "down": playerPosition.z = playerPosition.z - 1; break;
        }
        target.x = target.x + ((stalkerLocation.x - playerPosition.x)*2);
        target.z = target.z + ((stalkerLocation.z - playerPosition.z) * 2);
        return target;
    }
}
