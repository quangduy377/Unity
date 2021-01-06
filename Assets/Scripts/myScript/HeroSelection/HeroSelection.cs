using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelection : MonoBehaviour
{

    private GameObject wall;
    public GameObject mickeyPrefab;
    public GameObject ralphPrefab;

    /*public GameObject healthBar;
    public GameObject powBar;*/

    private Vector3 respawn;
    private int rotation;
    public void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        if (PlayerPrefs.GetString("playerSide") == "LEFT")
        {
            //we need to find the left wall
            wall = GameObject.Find("LeftWall");
            //respawn = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z + Random.Range(-3.89f, 6.06f));
            //rotation = -90;
        }
        else if (PlayerPrefs.GetString("playerSide") == "RIGHT")
        {
            wall = GameObject.Find("RightWall");
            //respawn = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z + Random.Range(-3.31f, 3.79f));
            //rotation = 90;
        }
    }

    public void randomArea()
    {
        if (wall.name.Equals("LeftWall"))
        {
            respawn = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z + Random.Range(-3.89f, 6.06f));
            rotation = -90;
        }
        else if(wall.name.Equals("RightWall"))
        {
            respawn = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z + Random.Range(-3.31f, 3.79f));
            rotation = 90;
        }
    }
    public void MickeySelected()
    {
        Debug.Log("just instantiated a mickey");
        //we don't have enough money to buy
        if (!buySuccessfully(-PlayerPrefs.GetInt("MICKEY_goldToBuy")))
            return;
        randomArea();
        GameObject mickeyClone = Instantiate(mickeyPrefab, respawn, transform.rotation) as GameObject;
        mickeyClone.transform.eulerAngles = new Vector3(mickeyClone.transform.eulerAngles.x, mickeyClone.transform.eulerAngles.y +rotation, mickeyClone.transform.eulerAngles.z);
        //assign back the name, so we make sure enemy can detect it and attack
        mickeyClone.transform.name = "ALLY";
        mickeyClone.transform.tag = PlayerPrefs.GetString("playerSide");
        Debug.Log("Mickey selected, with respawn place:"+mickeyClone.transform.position);

    }
    public void RalphSelected()
    {
        //we don't have enough money to buy
        if (!buySuccessfully(-PlayerPrefs.GetInt("RALPH_goldToBuy")))
            return;
        randomArea();
        Debug.Log("Ralph selected");
        GameObject ralphClone = Instantiate(ralphPrefab, respawn, transform.rotation) as GameObject;
        ralphClone.transform.eulerAngles = new Vector3(ralphClone.transform.eulerAngles.x, ralphClone.transform.eulerAngles.y + rotation, ralphClone.transform.eulerAngles.z);
        //since we bought a RALPh, we must deduct the money we have in the pocket
        ralphClone.transform.name = "ALLY";
        ralphClone.transform.tag = PlayerPrefs.GetString("playerSide");
        Debug.Log("Ralph selected, with respawn place:" + ralphClone.transform.position);
    }

    //decrease the amount of money we have in budget
    public bool buySuccessfully(int money)
    {
        int totalMoney = 0;
        GoldLoader[] goldData = FindObjectsOfType<GoldLoader>();
        for (int i = 0; i < goldData.Length; i++)
        {
            if (goldData[i].type == "Player")
            {
                //money is a negative number
                totalMoney = goldData[i].getCurrentGold() + money;
                if (totalMoney >= 0)
                {
                    goldData[i].addGold(money);
                    return true;
                }
            }
        }
        return false;
    }

    /*public void instantiateBars()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(this.transform.position);

    }*/
}








