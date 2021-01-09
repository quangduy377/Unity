using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAllyManager
{
    public static int alliesId = 0;
    public static int enemiesId = 0;
    
    public static void deductHealthBar(GameObject gameObj, float damage)
    {
        GameObject healthBar;
        if (gameObj == null)
            return;
        //this obj is a player
        if (gameObj.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            //TESTING POW
            healthBar = gameObj.GetComponent<Hero>().getHealthBar();
            //healthBar = gameObj.GetComponent<InstantiateHealthPowBar>().getHealthBar();
        }
        else
        {
            //Testing pow
            healthBar = gameObj.GetComponent<Enemy>().getHealthBar();
            //healthBar = gameObj.GetComponent<InstantiateHealthPowBar>().getHealthBar();
        }
        healthBar.GetComponent<Slider>().value -= damage;
    }
    
    public static void increasePowBar(GameObject gameObj, float damage)
    {
        GameObject powBar;
        if (gameObj == null)
            return;
        //this is player object
        if (gameObj.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            //TESTING POW
            powBar = gameObj.GetComponent<Hero>().getPowBar();
            //powBar = gameObj.GetComponent<InstantiateHealthPowBar>().getPowBar();
        }
        //this is enemy object
        else
        {
            //Testing pow
            powBar = gameObj.GetComponent<Enemy>().getPowBar();
            //powBar = gameObj.GetComponent<InstantiateHealthPowBar>().getPowBar();
        }
        powBar.GetComponent<Slider>().value += damage;
    }
    
}
