using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light FLight;
    public bool isOn;
    public float timer;
    public int maxBattery = 10;
    public int currentBattery;
    public FlashlightUI flashlightUI;
    
    public bool enemyIsIn;

    // Start is called before the first frame update
    void Start()
    {
        FLight = GetComponent<Light>();
        isOn = true;
        currentBattery = maxBattery;
        flashlightUI.SetMaxBattery(maxBattery);
        enemyIsIn = false;
    }

    // Update is called once per frame
    void Update()
    {
    
        if (timer >= 0)
        {
            if (isOn)
            {
                timer -= Time.deltaTime;
            }
        }

        if (timer <= 0)
        {
            timer = 5;
            if (isOn)
            {
                minusBattery(1);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            FLight.enabled = !FLight.enabled;

            if (!isOn)
            {
                isOn = true;
            }
            else
            {
                isOn = false;
            }
        }

        if (currentBattery == 0)
        {
            FLight.enabled = false;
            isOn = false;
        }
    }
    void minusBattery(int battery)
    {
        currentBattery -= battery;
        flashlightUI.SetBatteryLevel(currentBattery);
    }

    private void OnTriggerStay(Collider other)
    {
        if(isOn)
        {
            if(other.tag == "Enemy")
            {
                Debug.Log("TBat is in");
                enemyIsIn = true;
            }
        }else if(!isOn)
        {
            enemyIsIn = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("TBat is out");
            enemyIsIn = false;
        }            
    }
}
