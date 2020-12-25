using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    private GameObject[] targetAllies;
    private GameObject targetBuilding;

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

    private float timeInterval;

    private void Start()
    {
        string data = file.text;        
        enemy = JsonUtility.FromJson<HeroData>(data);
        moveable = true;
        identifyTags();
        //get all enemies to attack. REMEMBER we are enemies
        targetAllies = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("playerSide"));
        agent = gameObject.GetComponent<NavMeshAgent>();
        //findTargetAttack();
        AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, targetAllies, PlayerPrefs.GetString("playerSide"));
        agent.speed = enemy.moveSpeed;
    }

    private GameObject behindBuilding;
    void identifyTags()
    {
        //remember we are enemies
        if (PlayerPrefs.GetString("enemySide").Equals("LEFT"))
        {
            usTag = "LEFT";
            targetTag = "RIGHT";
            Debug.Log("insdie ENEMY, enemies on the LEFT");
            targetBuilding = GameObject.Find("TeamRight");

        }
        else
        {
            usTag = "RIGHT";
            targetTag = "LEFT";
            Debug.Log("insdie ENEMY, enemies on the RIGHT");
            targetBuilding = GameObject.Find("TeamLeft");
        }
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
                //findTargetAttack();
                AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, targetAllies, PlayerPrefs.GetString("playerSide"));

            }
        }
        else
        {
            //stop moving
            agent.isStopped = true;
            //we received information about the enemy, we can now attack them
            if (dataTarget != null)
            {
                //attackPlayer(1 / enemy.attackSpeed);
                AttackEnemyPlayer.attack(ref timeInterval, ref dataTarget, enemy, 1 / enemy.attackSpeed);
            }
            else
            {
                //that enemy no longer exists, should return
                moveable = true;
            }
            //if the enemy health falls below zero, we exterminate it, and can move to find another enemy
            //and we need to add the gold we achieve by killing it
            if (dataTarget.health <= 0)
            {
                Destroy(target);
                //now you need to move to find another opponent
                moveable = !moveable;
                //findTargetAttack();
                AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, targetAllies, PlayerPrefs.GetString("playerSide"));
                //add gold collected
                //addGoldCollected(dataTarget.goldOnDeath);
                AddGold.addGold(dataTarget.goldOnDeath, "Enemy");
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
        //if hit our ally, spread the ally out
        else if(collision.transform.tag.Equals(usTag))
        {
            float range = Random.Range(-1.0f, 1.0f);
            collision.gameObject.transform.position = new Vector3(collision.gameObject.transform.position.x
                , collision.gameObject.transform.position.y, collision.gameObject.transform.position.z + range);
        }
    }
    public HeroData getHeroData()
    {
        return enemy;
    }
    public float getMovespeed()
    {
        return enemy.moveSpeed;
    }

    
}
