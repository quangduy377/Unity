using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireOnApproachEnemy : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPit;

    private bool fire;
    private GameObject enemy;
    private HeroData enemyData;
    private float timeInterval;

    private GameObject ally;
    private bool attackingBuilding;
    private bool attackingAlly;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = FindObjectOfType<Enemy>().gameObject.GetComponent<Animator>();
        attackingBuilding = false;
        attackingAlly = false;
        enemy = FindObjectOfType<Enemy>().gameObject;
        enemyData = enemy.GetComponent<Enemy>().getHeroData();
        timeInterval = 1 / enemyData.attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!enemyData.attackMode.Equals("RANGED"))
            return;*/
        if (!attackingBuilding && !attackingAlly)
            return;
        //we are able to fire
        enemy.GetComponent<NavMeshAgent>().isStopped = true;
        timeInterval -= Time.deltaTime;
        //only fire when it counts less than 0
        if (timeInterval <= 0)
        {
            Instantiate(bullet, bulletPit.transform.position, gameObject.transform.rotation);
            timeInterval = 1 / enemyData.attackSpeed;
        }
        //now we can move
        if (attackingAlly)
        {
            if (ally.GetComponent<Hero>().getHeroData().health <= 0 || ally == null)
            {
                Animation.fireToRun(ref anim);
                enemy.GetComponent<NavMeshAgent>().isStopped = false;
                attackingAlly = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //face enemy
        if (other.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            Debug.Log("inside fireApproach, faced ally");
            attackingAlly = true;
            ally = other.gameObject;
            Animation.fire(ref anim);
        }
        //face the front of building, fire
        else if ((other.transform.name.Equals("BorderAttackRight") && PlayerPrefs.GetString("enemySide").Equals("LEFT"))
            || other.transform.name.Equals("BorderAttackLeft") && PlayerPrefs.GetString("enemySide").Equals("RIGHT"))
        {
            Debug.Log("detect border attack");
            attackingBuilding = true;
            Animation.fire(ref anim);
        }
    }
}
