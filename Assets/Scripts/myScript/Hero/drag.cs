using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class drag : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    public GameObject cloneType;

    private GameObject clone;

    private float yCoord;
    public LayerMask allyMask;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*Collider[] allies = Physics.OverlapBox(gameObject.transform.position,
            new Vector3(1.0f, 1.0f, 1.0f), Quaternion.identity, allyMask);
        Debug.Log("inside drag, allies length" + allies.Length);
        if (allies.Length > 0)
        {
            Debug.Log("inside drag, merge!@!");
        }*/
        Debug.Log("playerPref merge " + PlayerPrefs.GetInt("combine"));
    }
    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - getMouseWorldPos();
        Debug.Log("on mouse down");
        //INSTANTIATE NOW
        clone = Instantiate(cloneType, transform.position, transform.rotation) as GameObject;
    }


    private void OnMouseUp()
    {
        gameObject.transform.position = clone.transform.position;
        Destroy(clone);
        PlayerPrefs.SetInt("combine", 1);
    }
    private void OnMouseDrag()
    { 

        clone.transform.position = getMouseWorldPos() + mOffset;
        clone.transform.position = new Vector3(clone.transform.position.x, 0.5f, clone.transform.position.z);

        Debug.Log("clone pos" + clone.transform.position);
    }
    private Vector3 getMouseWorldPos()
    {
        //pixel coordinate (x,y) 
        Vector3 mousePoint = Input.mousePosition;
        //z coordinate of game object on screen
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);

        /*Vector3 mousePoint = Input.mousePosition;
        mousePoint.y = yCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
            Debug.Log("merge in drag");
    }
}
