using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireOnApproach : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPit;

    private bool fire;
    private GameObject hero;
    private HeroData heroData;
    private float timeInterval;

    private GameObject enemy;
    private bool attackingBuilding;
    private bool attackingEnemy;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = FindObjectOfType<Hero>().gameObject.GetComponent<Animator>();
        attackingBuilding = false;
        attackingEnemy = false;
        hero = FindObjectOfType<Hero>().gameObject;
        heroData = hero.GetComponent<Hero>().getHeroData();
        timeInterval = 1 / heroData.attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!heroData.attackMode.Equals("RANGED"))
            return;*/
        if (!attackingBuilding && !attackingEnemy)
            return;
        //we are able to fire
        hero.GetComponent<NavMeshAgent>().isStopped = true;
        timeInterval -= Time.deltaTime;
        //only fire when it counts less than 0
        if (timeInterval <= 0)
        {
            Instantiate(bullet, bulletPit.transform.position, gameObject.transform.rotation);
            timeInterval = 1 / heroData.attackSpeed;
        }
        //now we can move
        if (attackingEnemy)
        {
            //face toward enemy
            stopAndRotate(enemy);
            if (enemy.GetComponent<Enemy>().getHeroData().health <= 0 || enemy == null)
            {
                Animation.fireToRun(ref anim);
                hero.GetComponent<NavMeshAgent>().isStopped = false;
                attackingEnemy = false;
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.transform.tag.Equals(PlayerPrefs.GetString("enemySide")))
        {
            Debug.Log("inside fireApproach, faced enemy");
            attackingEnemy = true;
            enemy = other.gameObject;
            Animation.fire(ref anim);
        }
        //face the front of building, fire
        else if ((other.transform.name.Equals("BorderAttackRight")&&PlayerPrefs.GetString("playerSide").Equals("LEFT"))
            || other.transform.name.Equals("BorderAttackLeft") && PlayerPrefs.GetString("playerSide").Equals("RIGHT"))
        {
            Debug.Log("detect border attack");
            attackingBuilding = true;
            Animation.fire(ref anim);
        }
    }
    public void stopAndRotate(GameObject enemy)
    {
        float difference = hero.transform.position.z - enemy.transform.position.z;
        //we are on the left
        if (PlayerPrefs.GetString("playerSide").Equals("LEFT"))
        {
            //we are below
            if (difference > 0)
            {
                hero.transform.eulerAngles = new Vector3(hero.transform.eulerAngles.x, -135.0f, hero.transform.eulerAngles.z);
            }
            //we are above
            else
            {
                hero.transform.eulerAngles = new Vector3(hero.transform.eulerAngles.x, -45.0f, hero.transform.eulerAngles.z);
            }
        }
        else
        {
            //we are below
            if (difference > 0)
            {
                hero.transform.eulerAngles = new Vector3(hero.transform.eulerAngles.x, 135.0f, hero.transform.eulerAngles.z);
            }
            //we are above
            else
            {
                hero.transform.eulerAngles = new Vector3(hero.transform.eulerAngles.x, 45.0f, hero.transform.eulerAngles.z);
            }
        }
    }
}
