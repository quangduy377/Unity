using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadConfig : MonoBehaviour
{

    public TextAsset file ;
    // Start is called before the first frame update
    void Start()
    {
        string data = file.text;
        Debug.Log(data);
        Player playerData = JsonUtility.FromJson<Player>(data);
        Debug.Log(playerData.type);

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public class Player
    {
        public string type;
        public int health;
        public int damage;
        public int armor;
        public string attackMode;
        public float attackSpeed;
        public int goldOnDeath;
        public int goldToBuy;
    }

}
