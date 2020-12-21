using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHero : MonoBehaviour
{
    /*private Vector3 mOffset;
    private float zCoord;
    // Start is called before the first frame update
   
    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;    
    }*/
    private MeshRenderer mesh;
    private Rigidbody rigidbody;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();

    }
    void OnMouseDown()
    {
        Debug.Log("down");
    }
    void OnMouseUp()
    {
        Debug.Log("up");
    }
}
