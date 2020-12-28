using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTower
{
    public static void attackBuilding(ref float timeInterval, float attackPeriod,
        TowerHandler tower, HeroData player)
    {
        Debug.Log("inside static funct");
        timeInterval += Time.deltaTime;
        Debug.Log("Time interval" + timeInterval);
        float towerHp = tower.getCurrentHp();
        Debug.Log("inside static funct, towerHp" + towerHp);
        if (towerHp <= 0)
            return;
        float towerArmor = tower.getTowerData().armor;
        float heroDamage = player.damage;
        Debug.Log("inside static funct, playerDamn" + heroDamage);

        //now make an attack
        if (timeInterval >= attackPeriod)
        {
            towerHp -= heroDamage;
            towerHp += towerArmor;
            Debug.Log("inside static funct, current towerHp" + towerHp);
            tower.setCurrentHp(towerHp);
            timeInterval = 0;
        }
    }
}
