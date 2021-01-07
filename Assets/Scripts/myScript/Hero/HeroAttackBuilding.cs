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
        
        if (player.attackMode.Equals("RANGED"))
            return;
        if (attack)
        {
            //testing
            //
            timeInterval += Time.deltaTime;
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            Animation.runToAttack(ref anim);
            if (timeInterval >= (1 / player.attackSpeed))
            {
                AttackTower.attackBuilding(tower, player.damage);
                timeInterval = 0.0f;
            }
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
