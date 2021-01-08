using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem particleManager;
    void Start()
    {
        particleManager = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var emission = particleManager.emission;
        if (PlayerPrefs.GetInt("fire_level_player")==1)
        {
            emission.rateOverTime = 10.0f;
        }
        else if (PlayerPrefs.GetInt("fire_level") == 2)
        {
            emission.rateOverTime = 20.0f;
        }
        else if (PlayerPrefs.GetInt("fire_level") == 3)
        {
            emission.rateOverTime = 30.0f;
        }
        else if (PlayerPrefs.GetInt("fire_level") == 4)
        {
            emission.rateOverTime = 40.0f;
        }
        if (particleManager.isPlaying)
            return;
        Destroy(gameObject);

    }
}
