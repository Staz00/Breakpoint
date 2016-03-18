using UnityEngine;
using System.Collections;

public class StartParticle : MonoBehaviour {

    public GameObject particle;

    void Start()
    {
        particle.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            particle.SetActive(true);
        }
    }

}
