using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedHero : MonoBehaviour
{
    private HeroData heroData;
    public string character;
    public GameObject heroSkill;

    private Slider healthbar;
    private Slider powbar;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        heroData = gameObject.GetComponent<Hero>().getHeroData();
        //testing pow
        healthbar = gameObject.GetComponent<Hero>().getHealthBar().GetComponent<Slider>();
        powbar = gameObject.GetComponent<Hero>().getPowBar().GetComponent<Slider>();
    }

    // only usable for ranged hero
    void Update()
    {
        if (!heroData.attackMode.Equals("RANGED"))
            return;
        Debug.Log("inside rangedHero");
        if (heroData.health <= 0)
        {
            Animation.dead(ref anim);
            Destroy(healthbar.gameObject);
            Destroy(powbar.gameObject);
            Destroy(gameObject);
            return;
        }
        
        if (powbar.value >= powbar.maxValue)
        {
            Instantiate(heroSkill, this.transform.position, this.transform.rotation);
            powbar.value = powbar.minValue;
        }
    }
}
