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

    private bool rotated;

    public GameObject fightingParticle;
    public Transform emitParticle;
    private void Awake()
    {
        rotated = false;
        attacking = false;
        anim = GetComponent<Animator>();
        timeInterval = 0.0f;
        if (type.Equals("Mickey"))
        {
            enemy = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Mickey.text);
            PlayerPrefs.SetInt("MICKEY_goldToBuy", enemy.goldToBuy);
        }
        else if (type.Equals("Ralph"))
        {
            enemy = JsonUtility.FromJson<HeroData>(GameLoader.Instance.Ralph.text);
            PlayerPrefs.SetInt("RALPH_goldToBuy", enemy.goldToBuy);
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
        ///////////////////////
        ///TESTING disable the cube, this is a melee type
        /*if (!enemy.attackMode.Equals("RANGED"))
        {
            GameObject cube = GameObject.Find("Cube");
            //cube.SetActive(false);
            Destroy(cube);
            Destroy(gameObject.GetComponent<RangedEnemy>());
        }
        //this is a ranged type
        else
        {
            Destroy(gameObject.GetComponent<EnemyAttackBuilding>());
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
        healthBar.transform.position = new Vector3(vector.x, vector.y + 29.0f, vector.z);
        powerBar.transform.position = new Vector3(vector.x, vector.y + 25.0f, vector.z);
    }
    // Update is called once per frame
    void Update()
    {
        encounteredAllies = Physics.OverlapBox(transform.position, new Vector3(3.0f, 3.0f, 3.0f), Quaternion.identity, allyLayer);
        barsFollowObject();
        if (enemy.health <= 0)
        {
            //we add gold for the player
            AddGold.addGold(enemy.goldOnDeath, "Player");
            //removed = true;
            anim.SetBool("dead", true);
            Animation.dead(ref anim);
            removeAllComponents();
            return;
        }
        //TESTING, this is attacking building, no attacking hero

        //////

        if (enemy.attackMode.Equals("RANGED"))
        {
            return;
        }
        //testing, we have allies to attack, stop fighting building 
        if (encounteredAllies.Length > 0)
        {
            //stop the enemy immediately
            agent.isStopped = true;
        }
       
        //if it is moving
        if (moveable)
        {
            agent.isStopped = false;
            if (encounteredAllies.Length > 0)
            {
                int id = randomId(0, encounteredAllies.Length);
                target = encounteredAllies[id].gameObject;
                dataTarget = target.GetComponent<Hero>().getHeroData();
                if (dataTarget == null)
                {
                    return;
                }
                moveable = false;
                Animation.runToAttack(ref anim);
            }
        }
        //attacking ally
        else
        {
            //TESTING, STOP THE OBJECT IMEDIATELY
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            agent.isStopped = true;
            //we received information about the enemy, we can now attack them
            if (target != null && dataTarget != null)
            {
                
                //face toward player
                stopAndRotate(target);

                timeInterval += Time.deltaTime;
                float attackTime = 1 / enemy.attackSpeed;
                if (timeInterval >= attackTime)
                {
                    Vector3 rot = Quaternion.identity.eulerAngles;
                    rot = new Vector3(rot.x - 90.0f, rot.y, rot.z);
                    //TESTING PARTICLE
                    Instantiate(fightingParticle, emitParticle.position, Quaternion.Euler(rot));
                    timeInterval = 0;
                    AttackEnemyPlayer.attack(ref dataTarget, enemy, target);
                }

                //if (dataTarget != null)
                //{
                if (dataTarget.health <= 0)
                {             
                  //now you need to move to find another opponent
                    moveable = true;
                    Animation.attackToRun(ref anim);
                    agent.ResetPath();
                    agent.SetDestination(targetBuilding.transform.position);
                    //TESTING, now set active attacking building
                    GetComponent<EnemyAttackBuilding>().enabled = true;
                }
                //}
            }
            else
            {
                //that enemy no longer exists, should return
                agent.ResetPath();
                agent.SetDestination(targetBuilding.transform.position);
                Animation.attackToRun(ref anim);
                moveable = true;
                //TESTING
                GetComponent<EnemyAttackBuilding>().enabled = true;
                //return;
            }
            //time to pow
            if (powerBar.GetComponent<Slider>().value >= powerBar.GetComponent<Slider>().maxValue)
            {
                //AttackEnemyPlayer.Pow(ref dataTarget, enemy, target);
                Instantiate(enemySkill, emitParticle.position, emitParticle.transform.rotation);
                if (encounteredAllies.Length > 0)
                {
                    for (int i = 0; i < encounteredAllies.Length; i++)
                    {
                        //deal special damage to this unit, TESTING USING ==
                        if(encounteredAllies[i].gameObject==target)
                        {
                            AttackEnemyPlayer.Pow(ref dataTarget, enemy, encounteredAllies[i].gameObject);
                        }
                        //deal splash damage to around enemies
                        else
                        {
                            //don't deal damage to our enemies !!!
                            if (encounteredAllies[i].gameObject.tag.Equals(PlayerPrefs.GetString("enemySide")))
                                continue;
                            HeroData aroundTarget = encounteredAllies[i].gameObject.GetComponent<Hero>().getHeroData();
                            AttackEnemyPlayer.PowAOE(ref aroundTarget, enemy, encounteredAllies[i].gameObject);
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
    public void stopAndRotate(GameObject player)
    {
        float difference = this.transform.position.z - player.transform.position.z;
        //we are on the left
        if (PlayerPrefs.GetString("enemySide").Equals("LEFT"))
        {
            //we are below
            if (difference > 0)
            {
                //we are left compared to enemy
                if (this.transform.position.x > player.transform.position.x)
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
                if (this.transform.position.x > player.transform.position.x)
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
                if (this.transform.position.x > player.transform.position.x)
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
                if (this.transform.position.x > player.transform.position.x)
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
