using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAllyManager
{
    public static int alliesId = 0;
    public static int enemiesId = 0;

    /*public static List<ComponentManager> allies = new List<ComponentManager>();
    public static List<ComponentManager> enemies = new List<ComponentManager>();

    

    public static void addComponent(GameObject healthBar, GameObject powBar, GameObject obj)
    {
        ComponentManager newSet = new ComponentManager(healthBar, powBar, obj);
        if (obj.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            allies.Add(newSet);
        }
        else
        {
            enemies.Add(newSet);
        }
    }
    public static void removeComponent(GameObject obj)
    {
        if (obj == null)
        {
            removeNull();
            return;
        }
        if (obj.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            int i;
            for (i = 0; i < allies.Count; i++)
            {
                if (allies[i].gameObj.Equals(obj))
                {
                    break;
                }
            }
            allies.Remove(allies[i]);
        }
        else
        {
            int i;
            for (i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].gameObj.Equals(obj))
                {
                    break;
                }
            }
            enemies.Remove(enemies[i]);
        }
    }
    private static void removeNull()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i].gameObj== null)
            {
                allies.Remove(allies[i]);
                break;
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].gameObj == null)
            {
                enemies.Remove(enemies[i]);
                break;
            }
        }
    }

    public static GameObject getHealthBar(GameObject gameObj)
    {
        if (gameObj == null)
            return null;
        if (gameObj.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            int i;
            for (i = 0; i < allies.Count; i++)
            {
                if (allies[i].gameObj.Equals(gameObj))
                {
                    return allies[i].healthBar;
                }
            }
        }
        else if (gameObj.transform.tag.Equals(PlayerPrefs.GetString("enemySide")))
        {
            int i;
            for (i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].gameObj.Equals(gameObj))
                {
                    return enemies[i].healthBar;
                }
            }
        }
        return null;
    }

    public static GameObject getPowBar(GameObject gameObj)
    {
        if (gameObj == null)
            return null;
        if (gameObj.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            int i;
            for (i = 0; i < allies.Count; i++)
            {
                if (allies[i].gameObj.Equals(gameObj))
                {
                    return allies[i].powBar;
                }
            }
        }
        else if (gameObj.transform.tag.Equals(PlayerPrefs.GetString("enemySide")))
        {
            int i;
            for (i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].gameObj.Equals(gameObj))
                {
                    return enemies[i].powBar;
                }
            }
        }
        return null;
    }*/
    public static void deductHealthBar(GameObject gameObj, float damage)
    {
        GameObject healthBar;
        if (gameObj == null)
            return;
        //this obj is a player
        if (gameObj.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            healthBar = gameObj.GetComponent<Hero>().getHealthBar();
        }
        else
        {
            healthBar = gameObj.GetComponent<Enemy>().getHealthBar();
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
            powBar = gameObj.GetComponent<Hero>().getPowBar();
        }
        //this is enemy object
        else
        {
            powBar = gameObj.GetComponent<Enemy>().getPowBar();

        }
        powBar.GetComponent<Slider>().value += damage;
    }
    
}
