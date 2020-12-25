using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject image;
    public GameObject text;
    // Start is called before the first frame update
    private bool enemyBuildingExploded;

    private TowerHandler[] towers;
    private TowerHandler tower;

    void Start()
    {
        enemyBuildingExploded = false;
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
        }
        //enemy lost
        else if(Lost(PlayerPrefs.GetString("enemySide")))
        {
            text.GetComponent<Text>().text = "YOU WON";
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
}
