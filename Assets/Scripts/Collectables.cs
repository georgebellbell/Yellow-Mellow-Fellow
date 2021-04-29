using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    public Material doubleScoreMat;
    Material defaultMat;

    FellowInteractions fellow;
    Renderer renderer;

    void Start()
    {
        // if the collectable isn't a pellet, nothing will be done
        if (!CompareTag("Pellet")) return;

        fellow = GameObject.Find("Fellow").GetComponent<FellowInteractions>();
        renderer = GetComponent<Renderer>();
        defaultMat = renderer.material;
    }

    void Update()
    {
        // if the collectable isn't a pellet, nothing will be done
        if (!CompareTag("Pellet")) return;

        // if player has the DoubleScorePowerup active, change the item colour
        if (fellow.IsDoublePointsActive())
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
