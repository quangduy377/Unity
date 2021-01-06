using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FacedTargets
{
    public static List<GameObject> facedAllies = new List<GameObject>();
    public static List<GameObject> facedEnemies = new List<GameObject>();
    
    public static void addEnemyEncountered(GameObject enemy)
    {
        facedEnemies.Add(enemy);
    }

    public static void addAllyEncountered(GameObject ally)
    {
        facedAllies.Add(ally);
    }
    public static bool findNearbyAllyToAttack(GameObject enemy)
    {
        Debug.Log("nearby allies = " + facedAllies.Count);
        //random a nearest ally to attack
        for(int i = 0; i < facedAllies.Count; i++)
        {
            if (facedAllies[i] != null)
            {
                Debug.Log("allies is not null!!!!!");
                enemy.GetComponent<NavMeshAgent>().ResetPath();
                enemy.GetComponent<NavMeshAgent>().SetDestination(facedAllies[i].transform.position);
                return true;
            }
        }
        Debug.Log("allies is null!!!!!");

        return false;
    }
    public static bool findNearbyEnemyToAttack(GameObject ally)
    {
        //random a nearest enemies to attack
        for (int i = 0; i < facedEnemies.Count; i++)
        {
            if (facedEnemies[i] != null)
            {
                ally.GetComponent<NavMeshAgent>().ResetPath();
                ally.GetComponent<NavMeshAgent>().SetDestination(facedEnemies[i].transform.position);
                return true;
            }
        }
        return false;
    }

}
