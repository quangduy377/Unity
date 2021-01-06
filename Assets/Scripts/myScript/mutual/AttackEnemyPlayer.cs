using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AttackEnemyPlayer
{
    public static void goToBuilding(GameObject player, GameObject targetBuilding)
    {
        Vector3 destination = new Vector3(targetBuilding.transform.position.x, player.transform.position.y, player.transform.position.z);
        player.GetComponent<NavMeshAgent>().SetDestination(destination);
    }

    

    public static void findTargetAttack(GameObject player,GameObject targetBuilding, GameObject[] enemies, string tag)
    {
        int randomId = -10;
        randomId = findRandomIdTarget(ref enemies,tag);
        //return to ally wall. Remmeber we are enemies
        if (randomId == -1)
        {
            //we now attack the target wall
            player.GetComponent<NavMeshAgent>().SetDestination(targetBuilding.transform.position);
        }
        else
        {
            //else we still have targets to take down. REMEMBER WE ARE ENEMIES
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

    public static void attack(ref HeroData dataEnemy, HeroData dataHero, GameObject enemy)
    {

        float enemyHp = dataEnemy.health;
        if (enemyHp <= 0)
            return;
        float enemyArmor = dataEnemy.armor;
        //remember we are player
        float playerDamage = dataHero.damage;
        //now make an attack
        enemyHp -= playerDamage;
        enemyHp += enemyArmor;
        dataEnemy.health = enemyHp;

        Debug.Log(Time.frameCount + ": " + enemy.name + " enemy HP" + dataEnemy.health);
        //should deduct the enemy Health
        EnemyAllyManager.deductHealthBar(enemy, playerDamage - enemyArmor);
        EnemyAllyManager.increasePowBar(enemy, playerDamage - enemyArmor);
    }

    public static void Pow(ref HeroData dataEnemy, HeroData dataHero, GameObject enemy)
    {
        PowData pow = JsonUtility.FromJson<PowData>(GameLoader.Instance.Pow.text);
        float damageTime = pow.powTimeDamage;
        float powAccumulatedTime = pow.powAccumulated;
        dataEnemy.health -= dataHero.damage*damageTime;
        EnemyAllyManager.deductHealthBar(enemy, dataHero.damage*damageTime);
        EnemyAllyManager.increasePowBar(enemy, dataHero.damage*powAccumulatedTime);
    }

    public static void PowAOE(ref HeroData dataEnemy, HeroData dataHero, GameObject enemy)
    {
        PowData pow = JsonUtility.FromJson<PowData>(GameLoader.Instance.Pow.text);
        float damageTime = pow.powTimeDamage;
        float powAccumulatedTime = pow.powAccumulated;
        dataEnemy.health -= dataHero.damage * damageTime;
        EnemyAllyManager.deductHealthBar(enemy, dataHero.damage * damageTime);
        EnemyAllyManager.increasePowBar(enemy, dataHero.damage * powAccumulatedTime);
    }
}
