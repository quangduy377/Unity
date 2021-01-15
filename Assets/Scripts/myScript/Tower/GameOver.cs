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

    //particle effects
    public GameObject fireWork_1;
    public GameObject fireWork_2;
    public GameObject fireWork_3;

    public Transform placeHolderFW_1;
    public Transform placeHolderFW_2;
    public Transform placeHolderFW_3;
    public GameObject weather;

    public bool allFire = false;
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
            //automatically move to the menu screen after 4 seconds
            StartCoroutine(loadMenu());
        }
        //enemy lost
        else if(Lost(PlayerPrefs.GetString("enemySide")))
        {
            text.GetComponent<Text>().text = "YOU WON";
            image.SetActive(false);
            if (!allFire)
            {
                allFire = true;
                Destroy(weather);
                StartCoroutine(fireWork(fireWork_1, placeHolderFW_1, fireWork_2, placeHolderFW_2, fireWork_3, placeHolderFW_3));
            }
            removeAllObject();
            StartCoroutine(loadMenu());
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

    IEnumerator loadMenu()
    {
        yield return new WaitForSeconds(4.0f);
        Application.LoadLevel("intro");
    }

    IEnumerator fireWork(GameObject fireWork_1, Transform placeHolder1,
        GameObject fireWork_2, Transform placeHolder2,
        GameObject fireWork_3, Transform placeHolder3)
    {
        Instantiate(fireWork_1,placeHolder1.position,Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Instantiate(fireWork_2, placeHolder2.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Instantiate(fireWork_3, placeHolder3.position, Quaternion.identity);

    }
}
