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
        Vector3 pos = gameObject.transform.position;

        if (other.gameObject.CompareTag("LeftTeleporter"))
        {
            pos = RightTeleporter.transform.position;
            Debug.Log("LeftTeleporter found");
            pos.x = pos.x - 1.5f;
            transform.position = pos;
        }

        if (other.gameObject.CompareTag("RightTeleporter"))
        {
            pos = LeftTeleporter.transform.position;
            Debug.Log("RightTeleporter found");
            pos.x = pos.x + 1.5f;
            transform.position = pos;
        }
    }
}
