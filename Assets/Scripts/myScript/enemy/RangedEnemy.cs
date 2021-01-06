using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedEnemy : MonoBehaviour
{
    private HeroData enemy;
    public string character;
    public GameObject enemySkill;

    private Slider healthbar;
    private Slider powbar;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        enemy = gameObject.GetComponent<Enemy>().getHeroData();
        healthbar = gameObject.GetComponent<Enemy>().getHealthBar().GetComponent<Slider>();
        powbar = gameObject.GetComponent<Enemy>().getPowBar().GetComponent<Slider>();
    }

    // only usable for ranged enemy
    void Update()
    {
        if (!enemy.attackMode.Equals("RANGED"))
            return;
        Debug.Log("inside ranged enemy, enemy hp: " + enemy.health);
        if (enemy.health <= 0)
        {
            Animation.dead(ref anim);
            Destroy(healthbar.gameObject);
            Destroy(powbar.gameObject);
            Destroy(gameObject);
            return;   
        }
        //reset powbar when it reaches the maximum value
        if (powbar.value >= powbar.maxValue)
        {
            Instantiate(enemySkill, this.transform.position, this.transform.rotation);
            powbar.value = powbar.minValue;
        }
    }
}
