using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    private GameObject[] targetAllies;
    private GameObject targetWall;

    private string usTag;
    private string targetTag;

    private bool moveable;
    private bool findTarget;

    private NavMeshAgent agent;

    private int randomId;

    private GameObject target;

    public TextAsset file;
    public TextAsset teamloader;
    //remember we are enemy
    public HeroData enemy;
    private TeamConfig teamConfig;

    private HeroData dataTarget;

    private GoldLoader[] goldAmountText;

    private void Start()
    {
        string data = file.text;        
        enemy = JsonUtility.FromJson<HeroData>(data);

        moveable = true;
        Debug.Log("new enemy has just spawn");
        if (PlayerPrefs.GetString("enemySide").Equals("LEFT"))
        {
            usTag = "LEFT";
            targetTag = "RIGHT";
            Debug.Log("insdie ENEMY, enemies on the LEFT");
            targetWall = GameObject.Find("RightWall");
        }
        else
        {
            usTag = "RIGHT";
            targetTag = "LEFT";
            Debug.Log("insdie ENEMY, enemies on the RIGHT");
            targetWall = GameObject.Find("LeftWall");

        }
        //get all enemies to attack. REMEMBER we are enemies
        targetAllies = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("playerSide"));
        agent = gameObject.GetComponent<NavMeshAgent>();
        findTargetAttack();
    }

    // Update is called once per frame
    void Update()
    {
        //always look for a target
        targetAllies = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("playerSide"));
        //if it is moving
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
        else
        {
            //stop moving
            agent.isStopped = true;


            //we received information about the enemy, we can now attack them
            if (dataTarget != null)
            {
                //deduct its health
                dataTarget.health -= (enemy.damage * enemy.attackSpeed * Time.deltaTime);
                //dataEnemy.health += dataEnemy.armor;
                Debug.Log("Health attacked by players:" + dataTarget.health);
            }
            //if the enemy health falls below zero, we exterminate it, and can move to find another enemy
            //and we need to add the gold we achieve by killing it
            if (dataTarget.health <= 0)
            {
                Destroy(target);
                //now you need to move to find another opponent
                moveable = !moveable;
                findTargetAttack();
                //add gold collected
                addGoldCollected(dataTarget.goldOnDeath);
                Debug.Log("player DIE!!!");
            }
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        //it is an impact, stop moving, and attack the enemy
        if (collision.transform.tag.Equals(targetTag))
        {
            moveable = false;
            //get the information of the enemy
            target = collision.gameObject;

            dataTarget = target.GetComponent<Hero>().getHeroData();
            //need to make them face to face literally, PENDINGGGG
        }
    }
    public void addGoldCollected(int gold)
    {
        goldAmountText = FindObjectsOfType<GoldLoader>();
        for (int i = 0; i < goldAmountText.Length; i++)
        {
            if (goldAmountText[i].type.Equals("Enemy"))
            {
                goldAmountText[i].addGold(gold);
                break;
            }
        }
    }
    public HeroData getHeroData()
    {
        return enemy;
    }

    public bool hitWall()
    {
        //Wall at the right
        if (targetWall.transform.tag=="RIGHT")
        {
            if (transform.position.x - 1.5f <= targetWall.transform.position.x)
            {
                return true;
            }
            return false;
        }
        //Wall at the left otherwise
        else
        {
            if (transform.position.x + 1.5f >= targetWall.transform.position.x)
            {
                return true;
            }
            return false;
        }
    }
    public float getMovespeed()
    {
        return enemy.moveSpeed;
    }

    void findTargetAttack() {
        int randomId = -10;
        randomId = findRanDomIdTarget();
        //return to ally wall. Remmeber we are enemies
        if (randomId == -1)
        {
            //we now attack the target wall
            Debug.Log("inside Enemy, now go to the wall");
            agent.SetDestination(targetWall.transform.position);
        }
        else
        {
            Debug.Log("inside Enemy, now found player id "+randomId);

            //else we still have targets to take down. REMEMBER WE ARE ENEMIES
            agent.SetDestination(targetAllies[randomId].transform.position);
        }
    }
    int findRanDomIdTarget() {
        //remember we are enemies. This is we are looking for players
        targetAllies = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("playerSide"));
        if (targetAllies.Length == 0)
            return -1;
        return UnityEngine.Random.Range(0, targetAllies.Length);
    }

    
}
