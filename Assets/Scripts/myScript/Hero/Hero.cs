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


    /// TESTING
    public GameObject healthBarSliderPrefab;
    public GameObject powBarSliderPrefab;
    private GameObject healthBar;
    private GameObject powerBar;
    //////////////////////////////////////////

    //hero pow effect
    public GameObject heroSkill;

    //private bool attacking;
    //private bool removed;

    private Collider[] encounteredEnemies;
    public LayerMask enemyLayer;

    //private Collider[] toBeMergedAlly;
    public LayerMask playerLayer;


    //public LayerMask playerLayer;

    private float rangedAttackPeriod;

    private bool rotated;

    public GameObject fightingParticle;
    public Transform emitParticle;
    public void Start()
    {
        rotated = false;
        //attacking = false;
        anim = GetComponent<Animator>();
        size = new Vector3(0.75f, 0.75f, 0.75f);
        if (character.Equals("Mickey"))
        {
            dataHero = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Mickey.text);
            PlayerPrefs.SetInt("MICKEY_goldToBuy", dataHero.goldToBuy);
        }
        else if (character.Equals("Ralph"))
        {
            dataHero = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Ralph.text);
            PlayerPrefs.GetInt("RALPH_goldToBuy", dataHero.goldToBuy);
        }
        moveable = true;
        timeInterval = 0.0f;
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
        agent.speed = dataHero.moveSpeed;
        AttackEnemyPlayer.goToBuilding(gameObject, targetBuilding);
        instantiateHealthPowBar();
        EnemyAllyManager.alliesId++;
        dataHero.id = EnemyAllyManager.alliesId;
        //TESTING DISABLE THE CUBE
        /*if (!dataHero.attackMode.Equals("RANGED"))
        {
            Debug.Log("inside hero.cs, destroy cube");
            GameObject cube = GameObject.Find("Cube");
            //cube.SetActive(false);
            Destroy(cube);
            Destroy(gameObject.GetComponent<RangedHero>());
        }*/
        //this is a ranged type
        /*if(dataHero.attackMode.Equals("RANGED"))
        {
            Debug.Log("inside hero.cs, this is ranged mode");

            Destroy(gameObject.GetComponent<HeroAttackBuilding>());
        }*/
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

        healthBar.GetComponent<Slider>().interactable = false;
        powerBar = Instantiate(powBarSliderPrefab,
            new Vector3(0.0f, 0.0f, 0.0f),
            gameObject.transform.rotation, GameObject.Find("CanvasView").transform) as GameObject;
        //healthBar.transform.SetParent(GameObject.Find("CanvasView").transform);
        powerBar.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        powerBar.GetComponent<Slider>().minValue = 0;
        powerBar.GetComponent<Slider>().maxValue = dataHero.health / 2;
        powerBar.GetComponent<Slider>().value = powerBar.GetComponent<Slider>().minValue;
        powerBar.GetComponent<Slider>().interactable = false;

    }

    public void barsFollowObject()
    {
        Vector3 vector = Camera.main.WorldToScreenPoint(this.transform.position);
        //Camera.main.
        healthBar.transform.position = new Vector3(vector.x, vector.y + 29.0f, vector.z);
        powerBar.transform.position = new Vector3(vector.x, vector.y + 25.0f, vector.z);
    }
    /// ////////////////////////////////////
    // Update is called once per frame
    public void Update()
    {

        barsFollowObject();
        encounteredEnemies = Physics.OverlapBox(transform.position, new Vector3(3.0f, 3.0f, 3.0f), Quaternion.identity, enemyLayer);

        if (dataHero.health <= 0)
        {
            //add gold for enemy
            AddGold.addGold(dataHero.goldOnDeath, "Enemy");
            //removed = true;
            //we are death, make it real
            Animation.dead(ref anim);
            removeAllComponents();
            return;
        }

        //this file has nothing to do with ranged attack
        if (dataHero.attackMode.Equals("RANGED"))
        {
            Debug.Log("inside hero.cs, in RANGED");
            return;
        }
        //testing, we have enemies to attack, stop fighting building 
        if (encounteredEnemies.Length > 0)
        {
            agent.isStopped = true;
            //GetComponent<HeroAttackBuilding>().enabled = false;
        }

        if (moveable)
        {
            agent.isStopped = false;
            if (encounteredEnemies.Length > 0)
            {
                int id = randomId(0, encounteredEnemies.Length);
                enemyObject = encounteredEnemies[id].gameObject;
                dataEnemy = enemyObject.GetComponent<Enemy>().getHeroData();
                if (dataEnemy == null)
                {
                    return;
                }
                moveable = false;
                Animation.runToAttack(ref anim);
            }
        }
        //attacking the enemy
        else
        {
            //TESTING, STOP THE OBJECT IMEDIATELY
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //if (enemyObject != null)
            if (dataEnemy != null && enemyObject != null)
            {

                //face toward enemy
                stopAndRotate(enemyObject);

                agent.isStopped = true;
                timeInterval += Time.deltaTime;
                float interval = 1 / dataHero.attackSpeed;
                if (timeInterval >= interval)
                {
                    Vector3 rot = Quaternion.identity.eulerAngles;
                    rot = new Vector3(rot.x - 90.0f, rot.y, rot.z);
                    //TESTING PARTICLE
                    Instantiate(fightingParticle, emitParticle.position, Quaternion.Euler(rot));
                    timeInterval = 0.0f;
                    AttackEnemyPlayer.attack(ref dataEnemy, dataHero, enemyObject);
                }
                //if (dataEnemy != null)
                //{
                if (dataEnemy.health <= 0)
                {
                    //now you need to move to find another opponent
                    moveable = true;
                    //add gold collected
                    //change animation
                    Animation.attackToRun(ref anim);
                    //now get to the tower
                    agent.ResetPath();
                    agent.SetDestination(targetBuilding.transform.position);
                    //TESTING, now set active attacking building
                    GetComponent<HeroAttackBuilding>().enabled = true;
                }
                //}
            }
            else
            {
                agent.ResetPath();
                agent.SetDestination(targetBuilding.transform.position);
                Animation.attackToRun(ref anim);
                moveable = true;
                //TESTING
                GetComponent<HeroAttackBuilding>().enabled = true;
                //return;
            }
            //time to pow
            if (powerBar.GetComponent<Slider>().value >= powerBar.GetComponent<Slider>().maxValue)
            {
                //deal special damage to enemy
                //instatiate the effect
                Instantiate(heroSkill, this.transform.position, this.transform.rotation);
                for (int i = 0; i < encounteredEnemies.Length; i++)
                {
                    //deal special damage to this unit
                    if (encounteredEnemies[i].gameObject == enemyObject)
                    {
                        AttackEnemyPlayer.Pow(ref dataEnemy, dataHero, encounteredEnemies[i].gameObject);
                    }
                    //deal splash damage to around enemies
                    else
                    {
                        //don't deal damage to our allies !!!
                        if (encounteredEnemies[i].gameObject.tag.Equals(PlayerPrefs.GetString("playerSide")))
                            continue;
                   
                        HeroData aroundEnemy = encounteredEnemies[i].gameObject.GetComponent<Enemy>().getHeroData();
                        AttackEnemyPlayer.PowAOE(ref aroundEnemy, dataHero, encounteredEnemies[i].gameObject);
                    }
                }
                //reassign the value of powBar
                powerBar.GetComponent<Slider>().value = powerBar.GetComponent<Slider>().minValue;
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //we need to have allies spread out
        if (other.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            float range = UnityEngine.Random.Range(-0.25f, 0.25f);
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x + range
                , other.gameObject.transform.position.y, other.gameObject.transform.position.z + range);
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
        gameObject.transform.localScale = dataHero.level * size;

        //increase the size and max value of healthBar and powerBar;
        healthBar.transform.localScale = dataHero.level * size;
        powerBar.transform.localScale = dataHero.level * size;
        healthBar.GetComponent<Slider>().maxValue = dataHero.health;
        healthBar.GetComponent<Slider>().value = dataHero.health;
        powerBar.GetComponent<Slider>().maxValue = dataHero.health / 2;
        powerBar.GetComponent<Slider>().value = 0.0f;
    }
    public GameObject getHealthBar()
    {
        return healthBar;
    }
    public GameObject getPowBar()
    {
        return powerBar;
    }
    public void removeAllComponents()
    {
        //delete healthBar
        Destroy(healthBar);
        //delete powBar
        Destroy(powerBar);
        //delete gameObject
        Destroy(gameObject);
    }

    public int randomId(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public void stopAndRotate(GameObject enemy)
    {
        float difference = this.transform.position.z - enemy.transform.position.z;
        //we are on the left
        if (PlayerPrefs.GetString("playerSide").Equals("LEFT"))
        {
            //we are below
            if (difference > 0)
            {
                //we are left compared to enemy
                if (this.transform.position.x > enemy.transform.position.x)
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -135.0f, this.transform.eulerAngles.z);
                }
                //we are right 
                else
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 135.0f, this.transform.eulerAngles.z);
                }
            }
            //we are above
            else
            {
                //we are left compared to enemy
                if (this.transform.position.x > enemy.transform.position.x)
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -45.0f, this.transform.eulerAngles.z);
                }
                //we are right 
                else
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 45.0f, this.transform.eulerAngles.z);
                }
                //this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -45.0f, this.transform.eulerAngles.z);
            }
        }
        else
        {
            //we are below
            if (difference > 0)
            {
                //we are left compared to enemy
                if (this.transform.position.x > enemy.transform.position.x)
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -135.0f, this.transform.eulerAngles.z);
                }
                //we are right 
                else
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 135.0f, this.transform.eulerAngles.z);
                }
                //this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 135.0f, this.transform.eulerAngles.z);
            }
            //we are above
            else
            {
                //we are left compared to enemy
                if (this.transform.position.x > enemy.transform.position.x)
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -45.0f, this.transform.eulerAngles.z);
                }
                //we are right 
                else
                {
                    this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 45.0f, this.transform.eulerAngles.z);
                }
                //this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 45.0f, this.transform.eulerAngles.z);
            }
        }
    }
}
