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

    public GameObject fightingParticle;
    public Transform emitParticle;

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

        if (enemy.attackMode.Equals("RANGED"))
            return;
        if (attack)
        {
            //stop the enemy
            agent.isStopped = true;
            //make its animation
            Animation.runToAttack(ref anim);
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
