using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drag : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - getMouseWorldPos();
        Debug.Log("inside mousedown");

    }
    private void OnMouseDrag()
    {
        transform.position = getMouseWorldPos() + mOffset;
        //Debug.Log("inside mousedrag" + transform.position);
        PlayerPrefs.SetInt("combine", 1);
    }
    private Vector3 getMouseWorldPos()
    {
        //pixel coordinate (x,y) 
        Vector3 mousePoint = Input.mousePosition;
        //z coordinate of game object on screen
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
