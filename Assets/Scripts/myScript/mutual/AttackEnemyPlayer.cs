﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackEnemyPlayer
{
    public static void findTargetAttack(GameObject player,GameObject targetBuilding, GameObject[] enemies, string tag)
    {
        int randomId = -10;
        randomId = findRandomIdTarget(ref enemies,tag);
        //return to ally wall. Remmeber we are enemies
        if (randomId == -1)
        {
            //we now attack the target wall
            //agent.SetDestination(targetBuilding.transform.position);
            player.GetComponent<NavMeshAgent>().SetDestination(targetBuilding.transform.position);
        }
        else
        {
            //else we still have targets to take down. REMEMBER WE ARE ENEMIES
            //agent.SetDestination(enemies[randomId].transform.position);
            player.GetComponent<NavMeshAgent>().SetDestination(enemies[randomId].transform.position);
        }
    }
    private static int findRandomIdTarget(ref GameObject[] enemies, string tag)
    {
        enemies = GameObject.FindGameObjectsWithTag(tag);
        if (enemies.Length == 0)
            return -1;
        return UnityEngine.Random.Range(0, enemies.Length);
    }

    public static void attack(ref float timeInterval, ref HeroData dataEnemy, HeroData dataHero, float attackPeriod)
    {

        timeInterval += Time.deltaTime;
        float enemyHp = dataEnemy.health;
        if (enemyHp <= 0)
            return;
        int enemyArmor = dataEnemy.armor;
        //remember we are player
        float playerDamage = dataHero.damage;
        //now make an attack
        if (timeInterval >= attackPeriod)
        {
            enemyHp -= playerDamage;
            enemyHp += enemyArmor;
            dataEnemy.health = enemyHp;
            timeInterval = 0;
        }
    }
}