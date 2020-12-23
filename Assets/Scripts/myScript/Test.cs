using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private NavMeshAgent nav;
    public GameObject wall;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(wall.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Mickey test pos:" + gameObject.transform.position);
    }
}
