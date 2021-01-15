using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveScripts : MonoBehaviour
{
    public GameObject cubeForFire;
    // Start is called before the first frame update
    void Start()
    {
        HeroData herodata =null;
        while (herodata == null)
        {
            if (gameObject.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
            {
                herodata = gameObject.GetComponent<Hero>().getHeroData();
            }
            else if (gameObject.transform.tag.Equals(PlayerPrefs.GetString("enemySide")))
            {
                herodata = gameObject.GetComponent<Enemy>().getHeroData();
            }
        }
        Debug.Log("inside RemoveScript, got herodata");
        //it is a melee hero, we need to remove cube only
        if (!herodata.attackMode.Equals("RANGED"))
        {
            Debug.Log("RANGED MODE");
            Destroy(cubeForFire);
        }
        //if it is a ranged hero, we remove attack building
        else
        {
            Destroy(gameObject.GetComponent<HeroAttackBuilding>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
