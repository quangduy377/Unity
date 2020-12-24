using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
   

    private GameObject enemyWall;
    //LEFT OR RIGHT
    private string team; 


    private string enemy;

    // ['LEFT','RIGHT']
    private string[] teams = new string[] {"LEFT","RIGHT"}; 
    private bool moveable;

    // passing a file data
    public TextAsset heroData; 

    //store original data of a hero
    private HeroData dataHero;
    private GameObject heroObject;

    private HeroData dataEnemy;
    private GameObject enemyObject;
    //private const float delayAttack=0.01f;
    private GoldLoader[] goldAmountText;
    private int indicator;

    //indicate if one of the team hits the wall of the opponent

    //MESH
    private NavMeshAgent agent;

    //enemy Id to chase after
    private int enemyId;

    //enemies to chase after
    private GameObject[] enemies;

    //current chasing enemy
    private GameObject currentEnemy;
    // Start is called before the first frame update

    private string enemyTag;


    public void Start()
    {
        

        team = PlayerPrefs.GetString("playerSide");

        agent = GetComponent<NavMeshAgent>();
        moveable = true;
        if (PlayerPrefs.GetString("playerSide") == "LEFT")
        {
            enemyWall = GameObject.Find("RightWall");
        }
        else
        {
            enemyWall = GameObject.Find("LeftWall");
            
        }

        //load data of the hero
        string data = heroData.text;
        dataHero = JsonUtility.FromJson<HeroData>(data);

        if (dataHero.type.Equals ("MICKEY"))
        {
            PlayerPrefs.SetInt("MICKEY_goldToBuy", dataHero.goldToBuy);
        }
        else if (dataHero.type.Equals("RALPH"))
        {
            PlayerPrefs.SetInt("RALPH_goldToBuy", dataHero.goldToBuy);
        }
        //initially reach the random enemy
        findTargetAttack();

        agent.speed = dataHero.moveSpeed;
    }

    // Update is called once per frame
    public void Update()
    {
        if (hitWall())
        {
            Debug.Log("hit the wall");
            //Destroy(gameObject);
        }
        //only move the character when it's allowed
        if (moveable)
        {
            agent.isStopped = false;
            //check if he reach the destination
            if (agent.remainingDistance <= 0)
            {
                //find another object to attack
                findTargetAttack();
            }
        }
        //it is attacking
        if(!moveable)
        {
            //stop moving
            agent.isStopped = true ;
           

            //we received information about the enemy, we can now attack them
            if (dataEnemy != null)
            {
                //deduct its health
                dataEnemy.health -= (dataHero.damage * dataHero.attackSpeed *Time.deltaTime);
                //dataEnemy.health += dataEnemy.armor;
                Debug.Log("Health attacked by enemies:" + dataEnemy.health);
            }
            //if the enemy health falls below zero, we exterminate it, and can move to find another enemy
            //and we need to add the gold we achieve by killing it
            if (dataEnemy.health <= 0)
            {
                Destroy(enemyObject);
                //now you need to move to find another opponent
                moveable = !moveable;
                findTargetAttack();
                //add gold collected
                addGoldCollected(dataEnemy.goldOnDeath);
                Debug.Log("enemy DIE!!!");
            }
        }
    }
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision");
        //it is an impact, stop moving, and attack the enemy
        if (other.transform.tag.Equals(PlayerPrefs.GetString("enemySide")))
        {
            moveable = false;
            //get the information of the enemy
            enemyObject = other.gameObject;

            dataEnemy = enemyObject.GetComponent<Enemy>().getHeroData();
            //need to make them face to face literally, PENDINGGGG
        }
    }


    public HeroData getHeroData()
    {
        return dataHero;
    }

    public void addGoldCollected(int gold)
    {
        goldAmountText = FindObjectsOfType<GoldLoader>();
        for(int i = 0; i < goldAmountText.Length; i++)
        {
            if (goldAmountText[i].type.Equals("Player"))
            {
                goldAmountText[i].addGold(gold);
                break;
            }
        }
    }
    
    public bool hitWall()
    {
        //Wall at the right
        if (enemyWall.transform.name== "RightWall")
        {
            if (transform.position.x - 1.5f <= enemyWall.transform.position.x)
            {
                return true;
            }
            return false;
        }
        //Wall at the left otherwise
        else
        {
            if (transform.position.x + 1.5f >= enemyWall.transform.position.x)
            {
                return true;
            }
            return false;
        }
    }
    public float getMovespeed()
    {
        return dataHero.moveSpeed;
    }
 

    void findTargetAttack()
    {
        int randomId = -10;
        randomId = findRanDomIdTarget();
        //return to ally wall. Remmeber we are enemies
        if (randomId == -1)
        {
            //we now attack the target wall
            agent.SetDestination(enemyWall.transform.position);
        }
        else
        {
            Debug.Log("inside Hero, now found player id " + randomId);
            //else we still have targets to take down. REMEMBER WE ARE ENEMIES
            agent.SetDestination(enemies[randomId].transform.position);
        }
    }
    int findRanDomIdTarget()
    {
        //remember we are enemies. This is we are looking for players
        enemies = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("enemySide"));
        if (enemies.Length == 0)
            return -1;
        return UnityEngine.Random.Range(0, enemies.Length);
    }

}
