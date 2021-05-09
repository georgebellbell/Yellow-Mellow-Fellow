using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    public Material doubleScoreMat;
    Material defaultMat;

    FellowInteractions fellow;
    Renderer itemRenderer;

    void Start()
    {
        // if the collectable isn't a pellet, nothing will be done
        if (!CompareTag("Pellet")) return;

        fellow = GameObject.Find("Fellow").GetComponent<FellowInteractions>();
        itemRenderer = GetComponent<Renderer>();
        defaultMat = itemRenderer.material;
    }

    void Update()
    {
        // if the collectable isn't a pellet, nothing will be done
        if (!CompareTag("Pellet")) return;

        // if player has the DoubleScorePowerup active, change the item colour
        if (fellow.IsDoublePointsActive())
        {
            itemRenderer.material = doubleScoreMat;
            return;
        }

        itemRenderer.material = defaultMat;
    }

    // when player interacts with this object, it will disappear
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
