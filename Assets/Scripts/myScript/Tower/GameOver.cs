using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject image;
    public GameObject text;
    // Start is called before the first frame update

    private TowerHandler[] towers;
    private bool destroyAll;
    void Start()
    {
        destroyAll = false;
        //Debug.Log("Run gameover"+image.enabled);
        towers = FindObjectsOfType<TowerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if player Lost
        if (Lost(PlayerPrefs.GetString("playerSide")))
        {
            text.GetComponent<Text>().text = "YOU LOST";
            removeAllObject();
        }
        //enemy lost
        else if(Lost(PlayerPrefs.GetString("enemySide")))
        {
            text.GetComponent<Text>().text = "YOU WON";
            removeAllObject();
        }
    }
    bool Lost(string team)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i].team.Equals(team))
            {
                if (towers[i].getCurrentHp() <= 0)
                {
                    image.SetActive(true);
                    text.SetActive(true);
                    return true;
                }
            }
        }
        return false;
    }
    void removeAllObject()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("playerSide"));
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(PlayerPrefs.GetString("enemySide"));
        for (int i = 0; i < players.Length; i++)
        {
            //TESTING powbar
            Destroy(players[i].GetComponent<Hero>().getHealthBar().gameObject);
            Destroy(players[i].GetComponent<Hero>().getPowBar().gameObject);
            Destroy(players[i]);
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            //TESTING powbar
            Destroy(enemies[i].GetComponent<Enemy>().getHealthBar().gameObject);
            Destroy(enemies[i].GetComponent<Enemy>().getPowBar().gameObject);
            Destroy(enemies[i]);
        }
    }
}
