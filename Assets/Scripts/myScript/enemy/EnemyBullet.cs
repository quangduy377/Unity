using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletPeriod;
    private float currentBulletPeriod;


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
        damage = JsonUtility.FromJson<BulletData>(GameLoader.Instance.bullet.text).damage;
        Debug.Log("ranged damage:" + damage);
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
            Debug.Log("player hp hitted by enemybullet" + ally.GetComponent<Hero>().getHeroData().health);
            ally.GetComponent<Hero>().getHeroData().health -= damage;
            Debug.Log("after deduct health from enemybullet");
            EnemyAllyManager.deductHealthBar(ally, damage);
            EnemyAllyManager.increasePowBar(ally, damage);
            Debug.Log("after deduct healthBar and POWBAR from enemybullet");
            Destroy(gameObject);
            Debug.Log("after delete Enemybullet");
        }
        if (building != null && attackBuilding)
        {
            AttackTower.attackBuilding(building.GetComponent<TowerHandler>(), damage);
            Debug.Log("hitted by enemy bullet, building hp: " + building.GetComponent<TowerHandler>().getCurrentHp());
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            attackAlly = true;
            ally = other.gameObject;
            Debug.Log("cube hit ally");
        }
        else if (other.transform.name.Equals(targetBuilding))
        {
            Debug.Log("inside enemy bullet, hit the building");
            attackBuilding = true;
            building = other.gameObject;
        }

    }
}
