using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroAttackBuilding : MonoBehaviour
{
    // Start is called before the first frame update

    //we have gameObject
    private HeroData player;
    private TowerHandler tower;
    private bool attack;
    private float timeInterval;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Hero>().getHeroData();
        attack = false;
        timeInterval = 0;
        if (PlayerPrefs.GetString("playerSide") == "LEFT")
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
        if (attack)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            AttackTower.attackBuilding(ref timeInterval, 1 / player.attackSpeed, tower, player);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //detect if we hit the building
        if ((other.transform.name.Equals("TeamLeft") && PlayerPrefs.GetString("playerSide").Equals("RIGHT")) 
            || (other.transform.name.Equals("TeamRight")&& PlayerPrefs.GetString("playerSide").Equals("LEFT")))
        {
            attack = true;
            Animation.runToAttack(ref anim);
        }
    }
}
