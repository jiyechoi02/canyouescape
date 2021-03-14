using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightUI : MonoBehaviour
{
    public Slider slider;

    public void SetMaxBattery(int batteryLevel)
    {
        slider.maxValue = batteryLevel;
        slider.value = batteryLevel;
    }

    public void SetBatteryLevel(int batteryLevel)
    {
        slider.value = batteryLevel;
    }
}
