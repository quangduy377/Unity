using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    private Vector3 size;
    private bool moveable;
    public string character;
    //store original data of a hero
    private HeroData dataHero;

    private HeroData dataEnemy;
    private GameObject enemyObject;
    //MESH
    private NavMeshAgent agent;

    //enemies to chase after
    private GameObject[] enemies;

    private GameObject targetBuilding;

    private float timeInterval;

    private Animator anim;
    public void Start()
    {
        anim = GetComponent<Animator>();
        size = new Vector3(0.75f, 0.75f, 0.75f);
        if (character.Equals("Mickey"))
        {
            dataHero = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Mickey.text);
            Debug.Log("Ally Mickey level:" + dataHero.level);

        }
        else if(character.Equals("Ralph"))
        {
            dataHero = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Ralph.text);
            Debug.Log("Ally Ralph level:" + dataHero.level);

        }

        moveable = true;

        timeInterval = 0;
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
        AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, enemies, PlayerPrefs.GetString("enemySide"));

        agent.speed = dataHero.moveSpeed;
    }

    // Update is called once per frame
    public void Update()
    {
        if (dataHero.health <= 0)
        {
            //we are death, make it real
            anim.SetBool("dead", true);
            Destroy(gameObject);
            return;
        }
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
                //Destroy(enemyObject);
                //now you need to move to find another opponent
                moveable = !moveable;
                //findTargetAttack();
                AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, enemies, PlayerPrefs.GetString("enemySide"));
                //add gold collected
                AddGold.addGold(dataEnemy.goldOnDeath, "Player");
                Debug.Log("players just add " + dataEnemy.goldOnDeath + " gold");
                //addGoldCollected(dataEnemy.goldOnDeath);
                Debug.Log("enemy DIE!!!");
                //change animation
                anim.SetBool("enemyDead", true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
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
            anim.SetBool("attacking", true);
        }
        //we need to have allies spread out
        else if (other.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            //must be the same type
            if (PlayerPrefs.GetInt("combine") == 1 && (gameObject.GetComponent<Hero>().getHeroData().type.Equals(other.GetComponent<Hero>().getHeroData().type)))
            {
                int levelHero = gameObject.GetComponent<Hero>().getHeroData().level;
                int levelDraggedHero = other.GetComponent<Hero>().getHeroData().level;
                //only the same level can be combined
                if ( levelDraggedHero==levelHero )
                {
                    increaseAttributes();
                    Destroy(other.gameObject);
                    Debug.Log("combine!");
                    Debug.Log("new Attributes");
                    Debug.Log(gameObject.GetComponent<Hero>().getHeroData().health);
                    Debug.Log(gameObject.GetComponent<Hero>().getHeroData().damage);
                    PlayerPrefs.SetInt("combine", 0);
                    anim.SetBool("merge", true);
                    anim.SetBool("merge", false);

                }
            }
            else
            {
                float range = UnityEngine.Random.Range(-0.5f, 0.5f);
                other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x + range
                    , other.gameObject.transform.position.y, other.gameObject.transform.position.z + range);
            }

        }
    }


    public HeroData getHeroData()
    {
        return dataHero;
    }
    public float getMovespeed()
    {
        return dataHero.moveSpeed;
    }

    public void increaseAttributes()
    {
        //increase original attributes
        Enhanced newAttributes = JsonUtility.FromJson<Enhanced>(GameLoader.Instance.EnhanceScale.text);
        dataHero.health *= newAttributes.health;
        dataHero.damage *= newAttributes.damage;
        dataHero.armor *= newAttributes.armor;
        dataHero.attackSpeed *= newAttributes.attackSpeed;
        dataHero.goldOnDeath *= newAttributes.goldOnDeath;
        //remember to increase level
        dataHero.level++;
        //increase the size of it
        gameObject.transform.localScale = dataHero.level*size;
    }
}
