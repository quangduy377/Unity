using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarAttached : MonoBehaviour
{
    public Slider healthBar;
    public Slider powBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        healthBar.transform.position = newPos;
        powBar.transform.position = new Vector3(newPos.x,newPos.y-3.0f,newPos.z);
    }
}
