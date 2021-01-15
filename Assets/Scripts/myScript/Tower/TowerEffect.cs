using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEffect : MonoBehaviour
{
    public GameObject Fire;
    public GameObject Smoke;

    public Transform firePit;
    private GameObject fireObj;
    private GameObject smokeObj;
    private float firstFragment;
    private float secondFragment;
    private float thirdFragment;
    private float deductedHp;
    private float currentTimeRemaining = 1.0f;
    private float towerHp;
    private float towerCurrentHp;
    // Start is called before the first frame update
    void Start()
    {
        //yield return new WaitForSeconds(1);
        while (GetComponent<TowerHandler>().getTowerData()==null)
        {}
        towerHp = GetComponent<TowerHandler>().getTowerData().health;
        //devide the health into 4 sections
        firstFragment = towerHp / 4;
        secondFragment = 2 * firstFragment;
        thirdFragment = 3 * firstFragment;
    }
    // Update is called once per frame
    void Update()
    {
        towerCurrentHp = GetComponent<TowerHandler>().getCurrentHp();
        deductedHp = towerHp - towerCurrentHp;
        currentTimeRemaining -= Time.deltaTime;
        if (currentTimeRemaining < 0 && towerCurrentHp < towerHp)
        {
            //need to rotate its y-axis -90*
            Vector3 rot = Quaternion.identity.eulerAngles;
            rot = new Vector3(rot.x - 90.0f, rot.y, rot.z);
            fireObj = Instantiate(Fire, firePit.position, Quaternion.Euler(rot)) as GameObject;
            smokeObj = Instantiate(Smoke, firePit.position, Quaternion.Euler(rot)) as GameObject;
            currentTimeRemaining = 1.0f;
        }
        //var emission = fireObj.GetComponent<ParticleSystem>().emission;
        if (fireObj == null)
            return;
        if (smokeObj == null)
            return;
        var emissionFire = fireObj.GetComponent<ParticleSystem>().emission;
        var emissionSmoke = smokeObj.GetComponent<ParticleSystem>().emission;
        if (deductedHp < firstFragment)
        {
            emissionFire.rateOverTime = 10.0f;
            emissionSmoke.rateOverTime = 1.0f;
        }
        else if ((firstFragment < deductedHp) && (deductedHp < secondFragment))
        {
            emissionFire.rateOverTime = 20.0f;
            emissionSmoke.rateOverTime = 2.0f;
            //emission.rateOverTime = 20.0f;
        }
        else if ((secondFragment < deductedHp) && (deductedHp < thirdFragment))
        {
            emissionFire.rateOverTime = 30.0f;
            emissionSmoke.rateOverTime = 3.0f;
        }
        else
        {
            emissionFire.rateOverTime = 40.0f;
            emissionSmoke.rateOverTime = 4.0f;

            //emission.rateOverTime = 40.0f;
        }
    }
}
