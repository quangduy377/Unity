using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateHealthPowBar : MonoBehaviour
{
    public GameObject healthBarSliderPrefab;
    public GameObject powBarSliderPrefab;
    //public string sideType;
    private GameObject healthBar;
    private GameObject powerBar;
    private HeroData dataHero;
    private void Start()
    {
        if (gameObject.transform.tag.Equals(PlayerPrefs.GetString("playerSide")))
        {
            dataHero = GetComponent<Hero>().getHeroData();
        }
        else if (gameObject.transform.tag.Equals(PlayerPrefs.GetString("enemySide")))
        {
            dataHero = GetComponent<Enemy>().getHeroData();
        }
        healthBar = Instantiate(healthBarSliderPrefab,
            new Vector3(0.0f, 0.0f, 0.0f),
            gameObject.transform.rotation, GameObject.Find("CanvasView").transform) as GameObject;
        //healthBar.transform.SetParent(GameObject.Find("CanvasView").transform);
        healthBar.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        healthBar.GetComponent<Slider>().minValue = 0;
        healthBar.GetComponent<Slider>().maxValue = dataHero.health;
        healthBar.GetComponent<Slider>().value = healthBar.GetComponent<Slider>().maxValue;
        healthBar.GetComponent<Slider>().interactable = false;
        powerBar = Instantiate(powBarSliderPrefab,
            new Vector3(0.0f, 0.0f, 0.0f),
            gameObject.transform.rotation, GameObject.Find("CanvasView").transform) as GameObject;
        //healthBar.transform.SetParent(GameObject.Find("CanvasView").transform);
        powerBar.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        powerBar.GetComponent<Slider>().minValue = 0;
        powerBar.GetComponent<Slider>().maxValue = dataHero.health / 2;
        powerBar.GetComponent<Slider>().value = powerBar.GetComponent<Slider>().minValue;
        powerBar.GetComponent<Slider>().interactable = false;
    }
    public GameObject getHealthBar()
    {
        return healthBar;
    }
    public GameObject getPowBar()
    {
        return powerBar;
    }
}
