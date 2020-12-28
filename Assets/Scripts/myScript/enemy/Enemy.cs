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

    private NavMeshAgent agent;


    private GameObject target;

    //public TextAsset file; SINGLETON
    //SINGLETON
    public string type;


   
    /// ////////////////////
   
    public TextAsset teamloader;
    //remember we are enemy
    public HeroData enemy;
    private TeamConfig teamConfig;

    private HeroData dataTarget;


    private float timeInterval;

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        
        if (type.Equals("Mickey"))
        {
            enemy = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Mickey.text);
            Debug.Log("Mickey respawn: "+enemy.level);
        }
        else if (type.Equals("Ralph"))
        {
            enemy = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Ralph.text);
            Debug.Log("Ralph respawn: " + enemy.level);

        }
        ////////////////////
        moveable = true;
        identifyTags();
        //get all enemies to attack. REMEMBER we are enemies
        targetAllies = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("playerSide"));
        agent = gameObject.GetComponent<NavMeshAgent>();
        //findTargetAttack();
        AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, targetAllies, PlayerPrefs.GetString("playerSide"));
        agent.speed = enemy.moveSpeed;

    }
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
        //we death
        if (enemy.health <= 0)
        {
            anim.SetBool("dead", true);
            Destroy(gameObject);
            return;
        }
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
                //Destroy(target);
                Debug.Log("we just kill an enemy, tempting to get data:");
                Debug.Log("enemy hp: "+target.GetComponent<Hero>().getHeroData().health);
                //now you need to move to find another opponent
                moveable = !moveable;
                //findTargetAttack();
                AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, targetAllies, PlayerPrefs.GetString("playerSide"));
                //add gold collected
                //addGoldCollected(dataTarget.goldOnDeath);
                AddGold.addGold(dataTarget.goldOnDeath, "Enemy");

                Debug.Log("enemy just add " + dataTarget.goldOnDeath + " gold");

                Debug.Log("player DIE!!!");
                anim.SetBool("enemyDead", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //it is an impact, stop moving, and attack the enemy
        if (other.transform.tag.Equals(targetTag))
        {
            moveable = false;
            //get the information of the enemy
            target = other.gameObject;

            dataTarget = target.GetComponent<Hero>().getHeroData();
            //need to make them face to face literally, PENDINGGGG
            anim.SetBool("attacking", true);
        }
        //if hit our ally, spread the ally out
        else if (other.transform.tag.Equals(usTag))
        {
            float range = Random.Range(-0.5f, 0.5f);
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x + range
                , other.gameObject.transform.position.y, other.gameObject.transform.position.z + range);
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
