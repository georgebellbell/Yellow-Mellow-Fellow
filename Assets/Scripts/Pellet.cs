using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{

    Fellow fellow;
    public Material doubleScoreMat;
    Material defaultMat;
    Renderer renderer;

    void Start()
    {
        if (!CompareTag("Pellet"))
            return;
        fellow = GameObject.Find("Fellow").GetComponent<Fellow>();
        renderer = GetComponent<Renderer>();
        defaultMat = renderer.material;
    }

    void Update()
    {
        if (!CompareTag("Pellet"))
            return;

        if (fellow.DoublePointsActive())
        {
            renderer.material = doubleScoreMat;
            return;
        }
        
        renderer.material = defaultMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
