using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float created_time;
    public float shooting_speed = 8f;
    public Vector3 shooting_direction;
    public Vector3 move_direction;
    private float alive_duration = 2f;
    private float timer;

    public GameObject flashLight;
    public FlashlightUI flashlightUI;

    // Start is called before the first frame update
    void Start()
    {
        timer = alive_duration;
        Debug.Log("Hell from Pumkin");
        flashLight = GameObject.Find("Spot Light");
        flashlightUI = GameObject.Find("Flashlight").GetComponent<FlashlightUI>();

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            Destroy(transform.gameObject);
        }
        
        if(transform.position.y <0.0f)
        {
            transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "Wall")
        {
            if(other.tag == "Player"){
                other.GetComponent<FirstPersonController>().gotHit = true;
                flashLight.GetComponent<Flashlight>().currentBattery--;
                flashlightUI.SetBatteryLevel(flashLight.GetComponent<Flashlight>().currentBattery);
            }
            Debug.Log("Destroy");
            Destroy(transform.gameObject);

        }
    }
}
