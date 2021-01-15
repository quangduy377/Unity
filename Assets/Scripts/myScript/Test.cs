using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void OnMouseDown()
    {
        Debug.Log("Mouse pos: " + Input.mousePosition);
    }
}
