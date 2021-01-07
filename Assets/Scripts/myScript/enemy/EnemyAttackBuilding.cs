using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackBuilding : MonoBehaviour
{
    //remember we are enemy
    private HeroData enemy;
    private TowerHandler tower;
    private bool attack;
    private float timeInterval;
    private NavMeshAgent agent;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>().getHeroData();
        attack = false;
        timeInterval = 0;
        //remember we are enemy, on the left side
        if (PlayerPrefs.GetString("enemySide") == "LEFT")
        {
            //that means we need to find a right building
            tower = GameObject.Find("TeamRight").GetComponent<TowerHandler>();
            Debug.Log("the tower has " + tower.getCurrentHp());
        }
        //find the left building
        else
        {
            tower = GameObject.Find("TeamLeft").GetComponent<TowerHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("in update, enemy attacking building");

        if (enemy.attackMode.Equals("RANGED"))
            return;
        if (attack)
        {
            Debug.Log("enemy attacking building");
            //stop the enemy
            agent.isStopped = true;
            Debug.Log("agent enemy: " + agent.isStopped);
            //make its animation
            Animation.runToAttack(ref anim);
            Debug.Log("after anim set to attack building");
            timeInterval += Time.deltaTime;
            if (timeInterval >= (1 / enemy.attackSpeed))
            {
                AttackTower.attackBuilding(tower, enemy.damage);
                timeInterval = 0.0f;
            }
        }
    }
    //
    private void OnTriggerEnter(Collider other)
    {
        //detect if we hit the building
        if (PlayerPrefs.GetString("enemySide").Equals("LEFT"))
        {
            if (other.transform.name == "TeamRight")
                attack = true;
        }
        else if (PlayerPrefs.GetString("enemySide").Equals("RIGHT"))
        {
            if (other.transform.name == "TeamLeft")
                attack = true;
        }
    }
}
