using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    /// TESTING
    public GameObject healthBarSliderPrefab;
    public GameObject powBarSliderPrefab;

    private GameObject healthBar;
    private GameObject powerBar;
    //////////////////////////////////////////
    public GameObject enemySkill;
    private void Start()
    {
        anim = GetComponent<Animator>();

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
        ////////////////////
        moveable = true;
        identifyTags();
        //get all enemies to attack. REMEMBER we are enemies
        targetAllies = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("playerSide"));
        agent = gameObject.GetComponent<NavMeshAgent>();
        //findTargetAttack();
        AttackEnemyPlayer.findTargetAttack(gameObject, targetBuilding, targetAllies, PlayerPrefs.GetString("playerSide"));
        agent.speed = enemy.moveSpeed;

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
    /// ////////////////////////////////////

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
        barsFollowObject();//TESTING
        //we death
        if (enemy.health <= 0)
        {
            anim.SetBool("dead", true);
            Destroy(gameObject);
            Destroy(healthBar);
            Destroy(powerBar);
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
                AttackEnemyPlayer.attack(ref timeInterval, ref dataTarget, enemy, 1 / enemy.attackSpeed,target);
                Debug.Log("Ally hpBar" + EnemyAllyManager.getHealthBar(target).GetComponent<Slider>().value);
                /////////////////////////
                if(powerBar.GetComponent<Slider>().value >= powerBar.GetComponent<Slider>().maxValue)
                {
                    AttackEnemyPlayer.Pow(ref dataTarget, enemy, target);
                    Instantiate(enemySkill, this.transform.position, this.transform.rotation);
                    powerBar.GetComponent<Slider>().value = powerBar.GetComponent<Slider>().minValue;
                }
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
                Debug.Log("enemies hp: "+target.GetComponent<Hero>().getHeroData().health);
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

                //TESTING
                EnemyAllyManager.removeAlly(target);
                /////////
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
