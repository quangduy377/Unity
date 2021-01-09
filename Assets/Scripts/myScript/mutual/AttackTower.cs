using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTower
{
    public static void attackBuilding(TowerHandler tower, float damage)
    {
        
        float towerHp = tower.getCurrentHp();
        if (towerHp <= 0)
            return;
        float towerArmor = tower.getTowerData().armor;
        //now make an attack
        towerHp -= damage;
        towerHp += towerArmor;
        tower.setCurrentHp(towerHp);
        tower.healthBar.GetComponent<Slider>().value = towerHp;
    }
}
