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
    private GameObject targetBuilding;

    private float timeInterval;

    public void Start()
    {
        string data = heroData.text;
        dataHero = JsonUtility.FromJson<HeroData>(data);
        moveable = true;

        timeInterval = 0;
        team = PlayerPrefs.GetString("playerSide");
        agent = GetComponent<NavMeshAgent>();
        //we are on the left
        if (PlayerPrefs.GetString("playerSide") == "LEFT")
        {
            //building is on the right
            targetBuilding = GameObject.Find("TeamRight");
        }
        else
        {
            targetBuilding = GameObject.Find("TeamLeft");
            
        }
        //load data of the hero
        
        if (dataHero.type.Equals ("MICKEY"))
        {
            PlayerPrefs.SetInt("MICKEY_goldToBuy", dataHero.goldToBuy);
        }
        else if (dataHero.type.Equals("RALPH"))
        {
            PlayerPrefs.SetInt("RALPH_goldToBuy", dataHero.goldToBuy);
        }
        //initially reach the random enemy
        //findTargetAttack();
        AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, enemies, PlayerPrefs.GetString("enemySide"));

        agent.speed = dataHero.moveSpeed;
    }

    // Update is called once per frame
    public void Update()
    {
   
        //only move the character when it's allowed
        if (moveable)
        {
            agent.isStopped = false;
            //check if he reach the destination
            if (agent.remainingDistance <= 0)
            {
                //find another object to attack
                AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, enemies, PlayerPrefs.GetString("enemySide"));
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
                AttackEnemyPlayer.attack(ref timeInterval, ref dataEnemy, dataHero, 1 / dataHero.attackSpeed);
            }
            else
            {
                //please find another target
                moveable = true;
            }
            //if the enemy health falls below zero, we exterminate it, and can move to find another enemy
            //and we need to add the gold we achieve by killing it
            if (dataEnemy.health <= 0)
            {
                Destroy(enemyObject);
                //now you need to move to find another opponent
                moveable = !moveable;
                //findTargetAttack();
                AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, enemies, PlayerPrefs.GetString("enemySide"));
                //add gold collected
                AddGold.addGold(dataEnemy.goldOnDeath, "Player");
                //addGoldCollected(dataEnemy.goldOnDeath);
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
        //we need to have allies spread out
        else if(other.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            float range = UnityEngine.Random.Range(-1.0f, 1.0f);
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x
                , other.gameObject.transform.position.y, other.gameObject.transform.position.z + range);
        }
    }


    public HeroData getHeroData()
    {
        return dataHero;
    }

    /*public void addGoldCollected(int gold)
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
    }*/
    public float getMovespeed()
    {
        return dataHero.moveSpeed;
    }

    /*public static void addGold(int gold, string name)
    {
        GoldLoader[] goldAmountText = FindObjectsOfType<GoldLoader>();
        for(int i = 0; i < goldAmountText.Length; i++)
        {
            if (goldAmountText[i].type.Equals(name))
            {
                goldAmountText[i].addGold(gold);
                break;
            }
        }
    }*/
}
