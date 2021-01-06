using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    
    //private GameObject[] targetAllies;
    private GameObject targetBuilding;

    /*private string usTag;
    private string targetTag;*/

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

    /// TESTING
    public GameObject healthBarSliderPrefab;
    public GameObject powBarSliderPrefab;

    private GameObject healthBar;
    private GameObject powerBar;
    //////////////////////////////////////////
    public GameObject enemySkill;

    private bool attacking;
    private bool removed;

    private Collider[] encounteredAllies;
    public LayerMask allyLayer;
    private void Start()
    {
        attacking = false;
        anim = GetComponent<Animator>();
        timeInterval = 0.0f;
        if (type.Equals("Mickey"))
        {
            enemy = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Mickey.text);
            Debug.Log("Mickey respawn: " + enemy.level);
        }
        else if (type.Equals("Ralph"))
        {
            enemy = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Ralph.text);
            Debug.Log("Ralph respawn: " + enemy.level);

        }
        moveable = true;
        
        agent = gameObject.GetComponent<NavMeshAgent>();

        if (PlayerPrefs.GetString("enemySide").Equals("LEFT"))
        {
            targetBuilding = GameObject.Find("TeamRight");

        }
        else
        {
            targetBuilding = GameObject.Find("TeamLeft");
        }
        agent.speed = enemy.moveSpeed;
        AttackEnemyPlayer.goToBuilding(gameObject, targetBuilding);
        instantiateHealthPowBar();
        EnemyAllyManager.enemiesId++;
        enemy.id = EnemyAllyManager.enemiesId;
        Debug.Log("enemy ID " + enemy.id);
        ///////////////////////
        ///TESTING disable the cube, this is a melee type
        if (!enemy.attackMode.Equals("RANGED"))
        {
            GameObject cube = GameObject.Find("Cube");
            cube.SetActive(false);
            Destroy(gameObject.GetComponent<RangedEnemy>());
        }
        //this is a ranged type
        else
        {
            Destroy(gameObject.GetComponent<EnemyAttackBuilding>());
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
        healthBar.GetComponent<Slider>().maxValue = enemy.health;
        healthBar.GetComponent<Slider>().value = healthBar.GetComponent<Slider>().maxValue;

        powerBar = Instantiate(powBarSliderPrefab,
            new Vector3(0.0f, 0.0f, 0.0f),
            gameObject.transform.rotation, GameObject.Find("CanvasView").transform) as GameObject;
        //healthBar.transform.SetParent(GameObject.Find("CanvasView").transform);
        powerBar.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        powerBar.GetComponent<Slider>().minValue = 0;
        powerBar.GetComponent<Slider>().maxValue = enemy.health / 2;
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
    // Update is called once per frame
    void Update()
    {
        encounteredAllies = Physics.OverlapBox(transform.position, new Vector3(2.0f, 2.0f, 2.0f), Quaternion.identity, allyLayer);
        barsFollowObject();//TESTING
        if (enemy.health <= 0)
        {
            //we add gold for the player
            AddGold.addGold(enemy.goldOnDeath, "Player");
            //removed = true;
            anim.SetBool("dead", true);
            Animation.dead(ref anim);
            removeAllComponents();
            Debug.Log("REMOVED ENEMY");
            return;
        }
        if (enemy.attackMode.Equals("RANGED"))
        {
            Debug.Log("skipped enemy.cs");
            return;
        }
        //we death
        Debug.Log("inside Enemy.cs, enemy HP " + healthBar.GetComponent<Slider>().value);
        Debug.Log("inside Enemy.cs, enemy POW " + powerBar.GetComponent<Slider>().value);

        
        //if it is moving
        if (moveable)
        {   
            agent.isStopped = false;
            if (encounteredAllies.Length > 0)
            {
                Debug.Log("inside enemy.cs, encoutered ally name: "+encounteredAllies[0].gameObject.transform.name);

                moveable = false;
                int id = randomId(0, encounteredAllies.Length);

                Debug.Log("randomID: " + id);
                target = encounteredAllies[id].gameObject;
                dataTarget = target.GetComponent<Hero>().getHeroData();
                Animation.runToAttack(ref anim);
            }
        }
        //attacking ally
        else
        {
            agent.isStopped = true;
            //we received information about the enemy, we can now attack them
            if (target != null)
            {
                timeInterval += Time.deltaTime;
                float attackTime = 1 / enemy.attackSpeed;
                if (timeInterval >= attackTime)
                {
                    timeInterval = 0;
                    AttackEnemyPlayer.attack(ref dataTarget, enemy, target);
                }

                if (dataTarget != null)
                {
                    if (dataTarget.health <= 0)
                    {
                        Debug.Log("we just kill an enemy, tempting to get data:");
                        Debug.Log("enemies hp: " + target.GetComponent<Hero>().getHeroData().health);
                        //now you need to move to find another opponent
                        moveable = true;
                        //findTargetAttack();
                        //add gold collected
                        //AddGold.addGold(dataTarget.goldOnDeath, "Enemy");

                        Debug.Log("enemy just add " + dataTarget.goldOnDeath + " gold");
                        Debug.Log("player DIE!!!");

                        Animation.attackToRun(ref anim);
                        agent.ResetPath();
                        agent.SetDestination(targetBuilding.transform.position);
                    }                    
                }
            }
            else 
            {
                Debug.Log("hero is now null");
                //that enemy no longer exists, should return
                agent.ResetPath();
                agent.SetDestination(targetBuilding.transform.position);
                Animation.attackToRun(ref anim);
                moveable = true;
                return;
            }
            //time to pow
            if (powerBar.GetComponent<Slider>().value >= powerBar.GetComponent<Slider>().maxValue)
            {
                Debug.Log("now enemy POW >100, noom");
                //AttackEnemyPlayer.Pow(ref dataTarget, enemy, target);
                Instantiate(enemySkill, this.transform.position, this.transform.rotation);

                if (encounteredAllies.Length > 0)
                {
                    Debug.Log("inside enemyPOW, with encountered: "+encounteredAllies.Length);
                    for (int i = 0; i < encounteredAllies.Length; i++)
                    {
                        //deal special damage to this unit
                        if (encounteredAllies[i].gameObject.GetComponent<Hero>().getHeroData().Equals(dataTarget))
                        {
                            AttackEnemyPlayer.Pow(ref dataTarget, enemy, encounteredAllies[i].gameObject);
                        }
                        //deal splash damage to around enemies
                        else
                        {
                            HeroData enemy = encounteredAllies[i].gameObject.GetComponent<Hero>().getHeroData();
                            AttackEnemyPlayer.PowAOE(ref dataTarget, enemy, encounteredAllies[i].gameObject);
                        }
                    }
                }
                powerBar.GetComponent<Slider>().value = powerBar.GetComponent<Slider>().minValue;
            }
           
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //we need to spread out a little bit if we face our kind
        if (other.transform.tag.Equals(PlayerPrefs.GetString("enemySide")))
        {
            float range = Random.Range(-0.2f, 0.2f);
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x + range
                , other.gameObject.transform.position.y, other.gameObject.transform.position.z + range);
        }
    }
    //ally is moving away, we can't attack it anymore, we just skip it through;
   
    public HeroData getHeroData()
    {
        return enemy;
    }
    public float getMovespeed()
    {
        return enemy.moveSpeed;
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
}
