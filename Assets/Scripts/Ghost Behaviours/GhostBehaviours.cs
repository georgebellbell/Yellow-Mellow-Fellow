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
            case "left": target.x = target.x - 1; break;
            case "right": target.x = target.x + 1; break;
            case "up": target.z = target.z + 1; break;
            case "down": target.x = target.x + 1; break;
        }

        return target;
    }
}
