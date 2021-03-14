using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public bool showText;
    public int Batteries;
    public GameObject Flight;
    public int mainBattery;
    public FlashlightUI flashlightUI;
    public bool safeRemove;
    
    void Start()
    {
        showText = false;
    }

    void OnTriggerStay(Collider other)
    {
        showText = true;
        if (!safeRemove)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                mainBattery = Flight.GetComponent<Flashlight>().currentBattery;
                Batteries = 1;
                other.GetComponent<FirstPersonController>().pickedItemUp = true;
                Flight.GetComponent<Flashlight>().currentBattery = Batteries + mainBattery;
                flashlightUI.SetBatteryLevel(Flight.GetComponent<Flashlight>().currentBattery);
                safeRemove = true;
                if (safeRemove)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        showText = false;
    }

}
