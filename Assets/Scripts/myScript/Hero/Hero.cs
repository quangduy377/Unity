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


    private bool attacking;
    private bool removed;

    private Collider[] encounteredEnemies;
    public LayerMask enemyLayer;

    private Collider[] toBeMergedAlly;
    public LayerMask playerLayer;


    //public LayerMask playerLayer;

    private float rangedAttackPeriod;

    private bool rotated;
    public void Start()
    {
        rotated = false;
        Debug.Log("new instatiation");
        attacking = false;
        anim = GetComponent<Animator>();
        size = new Vector3(0.75f, 0.75f, 0.75f);
        if (character.Equals("Mickey"))
        {
            dataHero = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Mickey.text);
            Debug.Log("Ally Mickey level:" + dataHero.level);
            PlayerPrefs.SetInt("MICKEY_goldToBuy", dataHero.goldToBuy);
        }
        else if (character.Equals("Ralph"))
        {
            dataHero = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Ralph.text);
            Debug.Log("Ally Ralph level:" + dataHero.level);
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
        Debug.Log("hero ID " + dataHero.id);
        //TESTING DISABLE THE CUBE
        if (!dataHero.attackMode.Equals("RANGED"))
        {
            GameObject cube = GameObject.Find("Cube");
            cube.SetActive(false);
            Destroy(gameObject.GetComponent<RangedHero>());
        }
        //this is a ranged type
        else
        {
            Destroy(gameObject.GetComponent<HeroAttackBuilding>());
        }
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
        Debug.Log("convert to screen point x:" + vector.x + ", y:" + vector.y + ", z:" + vector.z);
        healthBar.transform.position = new Vector3(vector.x, vector.y + 29.0f, vector.z);
        powerBar.transform.position = new Vector3(vector.x, vector.y + 25.0f, vector.z);
    }
    /// ////////////////////////////////////
    // Update is called once per frame
    public void Update()
    {
        Debug.Log("inside hero.cs, rigidbody.velo = " + GetComponent<Rigidbody>().velocity.magnitude);

        Debug.Log("inside hero.cs, playerpref enemyattackingbuilding" + PlayerPrefs.GetInt("EnemyAttackingBuilding"));
        barsFollowObject();
        encounteredEnemies = Physics.OverlapBox(transform.position, new Vector3(3.0f, 3.0f, 3.0f), Quaternion.identity, enemyLayer);
        //testing, we have enemies to attack, stop fighting building 
        if (encounteredEnemies.Length > 0)
        {
            agent.isStopped = true;
            //GetComponent<HeroAttackBuilding>().enabled = false;
        }

        if (dataHero.health <= 0)
        {
            Debug.Log("hero hp <0 ");
            //add gold for enemy
            AddGold.addGold(dataHero.goldOnDeath, "Enemy");
            //removed = true;
            Debug.Log("dataHero <0");
            //we are death, make it real
            Animation.dead(ref anim);
            removeAllComponents();
            Debug.Log("REMOVED hero");
            return;
        }
        //TESTING, this is attacking building, no attacking enemy

        //this file has nothing to do with ranged attack
        if (dataHero.attackMode.Equals("RANGED"))
        {
            Debug.Log("skipped Hero.cs");
            return;
        }
        Debug.Log("hero HP:" + dataHero.health);

        if (moveable)
        {
            agent.isStopped = false;
            if (encounteredEnemies.Length > 0)
            {
                Debug.Log("inside hero.cs, encoutered enemy: " + encounteredEnemies.Length);
                int id = randomId(0, encounteredEnemies.Length);
                Debug.Log("inside hero.cs, randomID = " + id);
                enemyObject = encounteredEnemies[id].gameObject;
                dataEnemy = enemyObject.GetComponent<Enemy>().getHeroData();
                if (dataEnemy == null)
                {
                    return;
                }
                moveable = false;
                Debug.Log("got enemy, damage: " + dataEnemy.damage);
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
                    Debug.Log("attacking enemy");
                    timeInterval = 0.0f;
                    AttackEnemyPlayer.attack(ref dataEnemy, dataHero, enemyObject);
                }
                //if (dataEnemy != null)
                //{
                if (dataEnemy.health <= 0)
                {
                    Debug.Log("attacking enemy: 0hp left");
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
                Debug.Log("enemy is now null");
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
                    if (encounteredEnemies[i].gameObject.GetComponent<Enemy>().getHeroData().Equals(dataEnemy))
                    {
                        AttackEnemyPlayer.Pow(ref dataEnemy, dataHero, encounteredEnemies[i].gameObject);
                    }
                    //deal splash damage to around enemies
                    else
                    {
                        HeroData enemy = encounteredEnemies[i].gameObject.GetComponent<Enemy>().getHeroData();
                        AttackEnemyPlayer.PowAOE(ref enemy, dataHero, encounteredEnemies[i].gameObject);
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
            Debug.Log("detect ally!!, playerPref" + PlayerPrefs.GetInt("combine"));

            //must be the same type
            if ((PlayerPrefs.GetInt("combine")) == 1 && (gameObject.GetComponent<Hero>().getHeroData().type.Equals(other.GetComponent<Hero>().getHeroData().type)))
            {
                Debug.Log("about to merge");
                Debug.Log("same type");
                int levelHero = gameObject.GetComponent<Hero>().getHeroData().level;
                int levelDraggedHero = other.GetComponent<Hero>().getHeroData().level;
                //only the same level can be combined
                if (levelDraggedHero == levelHero)
                {
                    //first we must combine the 2 hp together
                    dataHero.health += other.gameObject.GetComponent<Hero>().getHeroData().health;
                    increaseAttributes();
                    Destroy(other.gameObject);
                    Destroy(other.gameObject.GetComponent<Hero>().getHealthBar());
                    Destroy(other.gameObject.GetComponent<Hero>().getPowBar());
                    PlayerPrefs.SetInt("combine", 0);
                    Animation.runToMerge(ref anim);
                }
            }
            else
            {
                float range = UnityEngine.Random.Range(-0.25f, 0.25f);
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
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -135.0f, this.transform.eulerAngles.z);
            }
            //we are above
            else
            {
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, -45.0f, this.transform.eulerAngles.z);
            }
        }
        else
        {
            //we are below
            if (difference > 0)
            {
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 135.0f, this.transform.eulerAngles.z);
            }
            //we are above
            else
            {
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 45.0f, this.transform.eulerAngles.z);
            }
        }
    }
}
