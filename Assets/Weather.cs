using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public Transform startingPoint;
    public GameObject rain;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(rain, startingPoint.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
