using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    //private NavMeshAgent nav;
    //public GameObject wall;
    private float twoSec;
    void Start()
    {
        twoSec = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (twoSec < 0)
        {
            Debug.Log("end of 5 sec");
            return;
        }
        twoSec -= Time.deltaTime;
    }
}
