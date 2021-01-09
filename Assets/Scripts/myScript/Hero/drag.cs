using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class drag : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    public GameObject cloneType;

    private GameObject clone;

    private float yCoord;
    public LayerMask allyMask;


    private Collider[] colliders;
    private GameObject mergedObject;

    //public GameObject upgradeEffect;
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
        //INSTANTIATE NOW
        clone = Instantiate(cloneType, transform.position, transform.rotation) as GameObject;
    }


    private void OnMouseUp()
    {
        //if we have nearby ally to merge
        if (colliders.Length > 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                //if they have the same type, both mickey or both ralph. They also need to have the same level
                if (colliders[i].gameObject.GetComponent<Hero>().getHeroData().type.Equals(gameObject.GetComponent<Hero>().getHeroData().type) && 
                    colliders[i].gameObject.GetComponent<Hero>().getHeroData().level == gameObject.GetComponent<Hero>().getHeroData().level
                    )
                {
                    mergedObject = colliders[i].gameObject;
                    gameObject.transform.position = mergedObject.transform.position;
                    Animator anim = mergedObject.GetComponent<Animator>();
                    //increase attributes of the merged object, and its level as well
                    mergedObject.GetComponent<Hero>().increaseAttributes();
                    mergedObject.GetComponent<Hero>().getHeroData().level++;
                    //change the animation
                    Animation.runToMerge(ref anim);
                    //pop the effect
                    //Vector3 rot = Quaternion.identity.eulerAngles;
                    //rot = new Vector3(rot.x - 90.0f, rot.y, rot.z);
                    //Instantiate(upgradeEffect, mergedObject.transform.position, Quaternion.Euler(rot));
                    //Destroy this gameObject so that we only have mergedObject left;
                    gameObject.GetComponent<Hero>().removeAllComponents();
                    break;
                }
            }
        }
        Destroy(clone);


        //ORIGINAL
        /*gameObject.transform.position = clone.transform.position;
        //reset gameObject agent
        gameObject.GetComponent<NavMeshAgent>().ResetPath();
        Destroy(clone);
        PlayerPrefs.SetInt("combine_id", gameObject.GetComponent<Hero>().getHeroData().id);*/
    }
    private void OnMouseDrag()
    {
        //get all ally nearby to merge
        colliders = Physics.OverlapBox(clone.transform.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, allyMask);
        clone.transform.position = getMouseWorldPos() + mOffset;
        clone.transform.position = new Vector3(clone.transform.position.x, 0.5f, clone.transform.position.z);
    }
    private Vector3 getMouseWorldPos()
    {
        //pixel coordinate (x,y) 
        Vector3 mousePoint = Input.mousePosition;
        //z coordinate of game object on screen
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.transform.tag.Equals(PlayerPrefs.GetString("playerSide"))
    }
}
