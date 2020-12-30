using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAllyManager
{
    public static List<ComponentManager> allies = new List<ComponentManager>();
    public static List<ComponentManager> enemies = new List<ComponentManager>();

    public static void addAlly(GameObject healthBar, GameObject powBar, GameObject ally)
    {
        ComponentManager newSet = new ComponentManager(healthBar, powBar, ally);
        allies.Add(newSet);
    }
    public static void addEnemies(GameObject healthBar, GameObject powBar, GameObject enemy)
    {
        ComponentManager newSet = new ComponentManager(healthBar, powBar, enemy);
        enemies.Add(newSet);
    }

    public static void removeAlly(GameObject ally)
    {
        int i;
        for(i = 0; i < allies.Count; i++)
        {
            if (allies[i].gameObj.Equals(ally))
            {
                break;
            }
        }
        allies.Remove(allies[i]);
    }

    public static void removeEnemy(GameObject enemy)
    {
        int i;
        for (i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].gameObj.Equals(enemy))
            {
                break;
            }
        }
        enemies.Remove(enemies[i]);
    }
    public static GameObject getHealthBar(GameObject gameObj)
    {
        int i;
        for (i = 0; i < allies.Count; i++)
        {
            if (allies[i].gameObj.Equals(gameObj))
            {
                return allies[i].healthBar;
            }
        }
        return null;
    }

    public static void deductHealthBar(GameObject gameObj, float damage)
    {
        GameObject healthBar = getHealthBar(gameObj);
        healthBar.GetComponent<Slider>().value -= damage;
    }
    
    public static void increasePowBar(GameObject gameObj, float damage)
    {
        GameObject powBar = getPowBar(gameObj);
        powBar.GetComponent<Slider>().value += damage;
    }

    public static GameObject getPowBar(GameObject gameObj)
    {
        int i;
        for (i = 0; i < allies.Count; i++)
        {
            if (allies[i].gameObj.Equals(gameObj))
            {
                return allies[i].powBar;
            }
        }
        return null;
    }


}
