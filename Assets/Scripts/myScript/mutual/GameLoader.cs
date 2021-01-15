using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance { get; private set; }
    public TextAsset EnemyGold;
    public TextAsset LeftTower;
    public TextAsset Mickey;
    public TextAsset PlayerGold;
    public TextAsset Ralph;
    public TextAsset RightTower;
    public TextAsset TeamSelection;
    public TextAsset EnhanceScale;
    public TextAsset Pow;
    public TextAsset bullet;
    private void Awake()
    {
        if (Instance == null)
        {
            //instantiate all the data 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
