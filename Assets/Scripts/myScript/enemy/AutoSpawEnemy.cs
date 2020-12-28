using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoSpawEnemy : MonoBehaviour
{
    private float timeCount;
    private float currentTimeRemaining;

    private GameObject wall;
    private Vector3 respawn;
    private float rotation;

    public GameObject mickeyEnemyPrefab;
    public GameObject ralphEnemyPrefab;
    void Start()
    {
        timeCount = 1.0f;
        currentTimeRemaining = timeCount;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeRemaining -= Time.deltaTime;
        //time to respawn
        if (currentTimeRemaining <= 0)
        {
            SpawnEnemy();
            currentTimeRemaining = timeCount;
        }
    }
    void SpawnEnemy()
    {
        Debug.Log("Repawn from the " + PlayerPrefs.GetString("enemySide"));
        int idHero = (int)Random.Range(0, 2);
        //we are ENEMIES....
        if (PlayerPrefs.GetString("enemySide") == "LEFT")
        {
            //we need to find the left wall
            wall = GameObject.Find("LeftWall");
            respawn = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z + Random.Range(-3.89f,6.06f));
            rotation = -90;
        }
        //WE ARE ENEMIES
        else if (PlayerPrefs.GetString("enemySide") == "RIGHT")
        {
            wall = GameObject.Find("RightWall");
            respawn = new Vector3(wall.transform.position.x, wall.transform.position.y, wall.transform.position.z + Random.Range(-3.31f,3.79f));
            rotation = 90;
        }
        //MICKEY
        if (idHero == 0)
        {
            if (!buySuccessfully(-PlayerPrefs.GetInt("MICKEY_goldToBuy")))
                return;
            Debug.Log("Mickey selected");
            GameObject mickeyClone = Instantiate(mickeyEnemyPrefab, respawn, transform.rotation) as GameObject;
            mickeyClone.GetComponent<NavMeshAgent>().speed = 5.0f;

            mickeyClone.transform.eulerAngles = new Vector3(mickeyClone.transform.eulerAngles.x, mickeyClone.transform.eulerAngles.y + rotation, mickeyClone.transform.eulerAngles.z);
            //assign back the name, so we make sure enemy can detect it and attack
            mickeyClone.transform.name = "ENEMY";
            mickeyClone.transform.tag = PlayerPrefs.GetString("enemySide");
            Debug.Log("respawed Mickey");         
        }
        else
        {
            if (!buySuccessfully(-PlayerPrefs.GetInt("RALPH_goldToBuy")))
                return;
            GameObject ralphClone = Instantiate(ralphEnemyPrefab, respawn, transform.rotation) as GameObject;
            ralphClone.GetComponent<NavMeshAgent>().speed = 5.0f;
            ralphClone.transform.eulerAngles = new Vector3(ralphClone.transform.eulerAngles.x, ralphClone.transform.eulerAngles.y + rotation, ralphClone.transform.eulerAngles.z);
            //since we bought a RALPh, we must deduct the money we have in the pocket
            ralphClone.transform.name = "ENEMY";
            ralphClone.tag = PlayerPrefs.GetString("enemySide");
            Debug.Log("respawed RALPH");
        }
    }
    public bool buySuccessfully(int money)
    {
        int totalMoney = 0;
        GoldLoader[] goldData = FindObjectsOfType<GoldLoader>();

        for (int i = 0; i < goldData.Length; i++)
        {
            if (goldData[i].type == "Enemy")
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
}
