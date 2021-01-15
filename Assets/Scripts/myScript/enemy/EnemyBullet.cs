using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletPeriod;
    private float currentBulletPeriod;

    public GameObject bulletParticle;

    private GameObject ally;
    private GameObject building;

    private float shurikenDamage;

    //public LayerMask obstacle;

    private bool attackBuilding;
    private bool attackAlly;
    private string targetBuilding;
    private float damage;
    void Start()
    {
        //instantiate the particle
        Instantiate(bulletParticle, this.transform.position, this.transform.rotation);
        damage = JsonUtility.FromJson<BulletData>(GameLoader.Instance.bullet.text).damage;
        attackBuilding = false;
        attackAlly = false;
        if (PlayerPrefs.GetString("enemySide").Equals("LEFT"))
        {
            targetBuilding = "TeamRight";
        }
        else
        {
            targetBuilding = "TeamLeft";
        }
        if (PlayerPrefs.GetString("enemySide").Equals("LEFT"))
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
        if (ally != null && attackAlly)
        {
            ally.GetComponent<Hero>().getHeroData().health -= damage;
            EnemyAllyManager.deductHealthBar(ally, damage);
            EnemyAllyManager.increasePowBar(ally, damage);
            Destroy(gameObject);
        }
        if (building != null && attackBuilding)
        {
            AttackTower.attackBuilding(building.GetComponent<TowerHandler>(), damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            attackAlly = true;
            ally = other.gameObject;
        }
        else if (other.transform.name.Equals(targetBuilding))
        {
            attackBuilding = true;
            building = other.gameObject;
        }

    }
}
