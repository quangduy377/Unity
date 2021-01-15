using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        while (particle==null)
        {
            particle = GetComponent<ParticleSystem>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("particle is playing");

        if (particle.isPlaying)
            return;
        Debug.Log("particle stop playing");
        Destroy(gameObject);
    }
}
