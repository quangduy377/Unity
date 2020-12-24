using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().enabled = true;
        //Debug.Log("Run gameover"+image.enabled);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
