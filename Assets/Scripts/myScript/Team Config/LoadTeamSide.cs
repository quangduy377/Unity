using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTeamSide : MonoBehaviour
{
    public TextAsset file;
    // Start is called before the first frame update
    void Start()
    {
        TeamConfig team = JsonUtility.FromJson<TeamConfig>(GameLoader.Instance.TeamSelection.text);
        PlayerPrefs.SetString("playerSide", team.playerSide);
        PlayerPrefs.SetString("enemySide", team.enemySide);
    }
}
