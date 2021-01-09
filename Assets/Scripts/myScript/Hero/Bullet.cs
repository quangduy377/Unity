using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletPeriod;
    //public GameObject bulletPit;
    public GameObject bulletParticle;


    private float currentBulletPeriod;

    private GameObject enemy;
    private GameObject building;

    private float shurikenDamage;

    //public LayerMask obstacle;

    private bool attackBuilding;
    private bool attackEnemy;
    private string targetBuilding;
    private float damage;
    void Start()
    {
        //instantiate the particle
        Instantiate(bulletParticle, this.transform.position, this.transform.rotation);
        damage = JsonUtility.FromJson<BulletData>(GameLoader.Instance.bullet.text).damage;
        attackBuilding = false;
        attackEnemy = false;
        if (PlayerPrefs.GetString("playerSide").Equals("LEFT"))
        {
            targetBuilding = "TeamRight";
        }
        else
        {
            targetBuilding = "TeamLeft";
        }
        if (PlayerPrefs.GetString("playerSide").Equals("LEFT"))
        {
            bulletSpeed = -bulletSpeed;
        }
        currentBulletPeriod = bulletPeriod;
    }

    // Update is called once per frame
    void Update()
    {

        currentBulletPeriod -= Time.deltaTime;
        //it reaches the maximal life span.
        if (currentBulletPeriod <= 0)
        {
            Destroy(gameObject);
        }
        gameObject.GetComponent<Rigidbody>().velocity = new Vector3(bulletSpeed, gameObject.GetComponent<Rigidbody>().velocity.y
            , gameObject.GetComponent<Rigidbody>().velocity.z);
        
        if (enemy != null && attackEnemy)
        {
            enemy.GetComponent<Enemy>().getHeroData().health -= damage;
            EnemyAllyManager.deductHealthBar(enemy, damage);
            EnemyAllyManager.increasePowBar(enemy, damage);
            Destroy(gameObject);
        }
        if (building!=null && attackBuilding)
        {
            AttackTower.attackBuilding(building.GetComponent<TowerHandler>(), damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(PlayerPrefs.GetString("enemySide")))
        {
            attackEnemy = true;
            enemy = other.gameObject;
        }
        else if (other.transform.name.Equals(targetBuilding))
        {
            attackBuilding = true;
            building = other.gameObject;
        }

    }
}
