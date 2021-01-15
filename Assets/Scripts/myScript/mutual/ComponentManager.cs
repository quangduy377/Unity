using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentManager
{
    public GameObject healthBar;
    public GameObject powBar;
    public GameObject gameObj;
    public ComponentManager(GameObject healthBar, GameObject powBar, GameObject obj)
    {
        this.healthBar = healthBar;
        this.powBar = powBar;
        this.gameObj = obj;
    }
}
