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

    private float sec;

    /// TESTING
    public GameObject healthBarSliderPrefab;
    public GameObject powBarSliderPrefab;
    private GameObject healthBar;
    private GameObject powerBar;
    //////////////////////////////////////////
    public GameObject heroSkill;
    public void Start()
    {
        
        sec = 1.0f;
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
        //TESTING///
        instantiateHealthPowBar();
        EnemyAllyManager.addAlly(healthBar, powerBar, gameObject);
        /////////////////
    }
    //TESTING
    public void instantiateHealthPowBar()
    {
        //testing INSTANTIATE HEALTHBAR
        healthBar = Instantiate(healthBarSliderPrefab,
            new Vector3(0.0f, 0.0f, 0.0f),
            gameObject.transform.rotation, GameObject.Find("CanvasView").transform) as GameObject;
        //healthBar.transform.SetParent(GameObject.Find("CanvasView").transform);
        healthBar.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        healthBar.GetComponent<Slider>().minValue = 0;
        healthBar.GetComponent<Slider>().maxValue = dataHero.health;
        healthBar.GetComponent<Slider>().value = healthBar.GetComponent<Slider>().maxValue;

        powerBar = Instantiate(powBarSliderPrefab,
            new Vector3(0.0f, 0.0f, 0.0f),
            gameObject.transform.rotation, GameObject.Find("CanvasView").transform) as GameObject;
        //healthBar.transform.SetParent(GameObject.Find("CanvasView").transform);
        powerBar.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        powerBar.GetComponent<Slider>().minValue = 0;
        powerBar.GetComponent<Slider>().maxValue = dataHero.health / 2;
        powerBar.GetComponent<Slider>().value = powerBar.GetComponent<Slider>().minValue;
    }

    public void barsFollowObject()
    {
        Vector3 vector = Camera.main.WorldToScreenPoint(this.transform.position);
        //Camera.main.
        Debug.Log("convert to screen point x:" + vector.x + ", y:" + vector.y + ", z:" + vector.z);

        healthBar.transform.position = new Vector3(vector.x, vector.y + 29.0f, vector.z);
        powerBar.transform.position = new Vector3(vector.x, vector.y + 25.0f, vector.z);
    }
    /// ////////////////////////////////////
    // Update is called once per frame
    public void Update()
    {
        barsFollowObject();//TESTING
        
        if (dataHero.health <= 0)
        {
            //we are death, make it real
            Animation.attackToDead(ref anim);
            Destroy(gameObject);
            Destroy(healthBar);
            Destroy(powerBar);
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
                AttackEnemyPlayer.attack(ref timeInterval, ref dataEnemy, dataHero, 1 / dataHero.attackSpeed, enemyObject);
                //Debug.Log("Enemy hpBar" + EnemyAllyManager.getHealthBar(enemyObject).GetComponent<Slider>().value);
                /////////////////////////
                //Check if we have enough POW to make a tremendous damage
                if(powerBar.GetComponent<Slider>().value >= powerBar.GetComponent<Slider>().maxValue)
                {
                    AttackEnemyPlayer.Pow(ref dataEnemy, dataHero, enemyObject);
                    Instantiate(heroSkill, this.transform.position, this.transform.rotation);
                    powerBar.GetComponent<Slider>().value = powerBar.GetComponent<Slider>().minValue;
                }
            }
            else
            {
                //please find another target
                Animation.attackToRun(ref anim);
                moveable = true;
            }
            //if the enemy health falls below zero, we exterminate it, and can move to find another enemy
            //and we need to add the gold we achieve by killing it
            if (dataEnemy.health <= 0)
            {
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
                Animation.attackToRun(ref anim);

                //TESTING
                EnemyAllyManager.removeEnemy(enemyObject);
                /////////
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
            //set attacking animation
            Animation.runToAttack(ref anim);
            Debug.Log("running: " + anim.GetBool("running") + ", attack:" + anim.GetBool("attacking"));
            //need to make them face to face literally, PENDINGGGG
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
                    PlayerPrefs.SetInt("combine", 0);
                    Animation.runToMerge(ref anim);
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
    public GameObject getHealthBar()
    {
        return healthBar;
    }
    public GameObject getPowBar()
    {
        return powerBar;
    }

}
