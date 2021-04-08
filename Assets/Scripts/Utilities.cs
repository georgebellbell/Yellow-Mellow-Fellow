using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public IEnumerator waitTime(int time)
    {
        yield return new WaitForSecondsRealtime(time);
    }
}
