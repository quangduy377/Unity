using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyText : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {

        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //ready is about to end
        StartCoroutine(changeState());
        //StartCoroutine(changeState("OK"));
    }
    IEnumerator changeState()
    {
        yield return new WaitForSeconds(0.7f);
        text.text = "GO";
        yield return new WaitForSeconds(0.7f);
        text.text = "OK";
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
