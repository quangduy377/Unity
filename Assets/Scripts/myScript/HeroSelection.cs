using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelection : MonoBehaviour
{
    private Vector3 mOffset;
    private float zCoord;

    public GameObject mickeyrespawnPlace;
    public GameObject ralphrespawnPlace;


    public GameObject mickeyPrefab;
    public GameObject ralphPrefab;
    public void MickeySelected()
    {
        Debug.Log("Mickey selected");
        //now we instantiate a new Mickey
        Vector3 respaw = new Vector3(mickeyrespawnPlace.transform.position.x -3.0f, mickeyrespawnPlace.transform.position.y + 2.0f, mickeyrespawnPlace.transform.position.z);
        GameObject mickeyClone = Instantiate(mickeyPrefab, respaw, transform.rotation) as GameObject;
        mickeyClone.transform.eulerAngles = new Vector3(mickeyClone.transform.eulerAngles.x, mickeyClone.transform.eulerAngles.y - 90, mickeyClone.transform.eulerAngles.z);
        //assign back the name, so we make sure enemy can detect it and attack
        mickeyClone.transform.name = "LEFT";

        //since we bought a mickey, we must deduct the money we have in the pocket
        deductMoney(-PlayerPrefs.GetInt("MICKEY_goldToBuy"));
    }
    public void RalphSelected()
    {
        Debug.Log("Ralph selected");
        Vector3 respaw = new Vector3(ralphrespawnPlace.transform.position.x, ralphrespawnPlace.transform.position.y + 2.0f, ralphrespawnPlace.transform.position.z);
        GameObject ralphClone = Instantiate(ralphPrefab, ralphrespawnPlace.transform.position, transform.rotation) as GameObject;
        ralphClone.transform.eulerAngles = new Vector3(ralphClone.transform.eulerAngles.x, ralphClone.transform.eulerAngles.y + 90, ralphClone.transform.eulerAngles.z);
        //since we bought a RALPh, we must deduct the money we have in the pocket
        ralphClone.transform.name = "RIGHT";
        deductMoney(-PlayerPrefs.GetInt("RALPH_goldToBuy"));

    }

    //decrease the amount of money we have in budget
    public void deductMoney(int money)
    {
        GoldLoader goldData = FindObjectOfType<GoldLoader>();
        goldData.addGold(money);
    }
    /*private void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - getMouseWorldPos();
    }
    private Vector3 getMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    private void OnMouseDrag()
    {
        transform.position = getMouseWorldPos() + mOffset;
    }*/
}








