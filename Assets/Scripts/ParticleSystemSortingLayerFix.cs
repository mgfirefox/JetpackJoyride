using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemSortingLayerFix : MonoBehaviour
{
    private void Start()
    {
        Renderer renderer = GetComponent<ParticleSystem>().GetComponent<Renderer>();
        renderer.sortingLayerName = "Player";
        renderer.sortingOrder = -1;
    }
}
