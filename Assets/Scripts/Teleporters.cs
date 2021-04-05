using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporters : MonoBehaviour
{
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
            pos = LeftTeleporter.ClosestPoint(pos);
            Debug.Log("RightTeleporter found");
            pos.x = pos.x + 0.5f;
            transform.position = pos;
        }
    }
}
