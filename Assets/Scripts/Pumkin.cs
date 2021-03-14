using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumkin : MonoBehaviour
{
    public float created_time;
    public float shooting_speed = 8f;
    public Vector3 shooting_direction;
    public Vector3 move_direction;
    private float alive_duration = 2f;
    private float timer;
    public Healthbar healthBar;
    private AudioSource source;
    public AudioClip sound;
    public GameObject bat;

    // Start is called before the first frame update
    void Start()
    {
        timer = alive_duration;
        Debug.Log("Hell from Pumkin");
        healthBar = GameObject.Find("Healthbar").GetComponent<Healthbar>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {


        timer -=Time.deltaTime;
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
                other.GetComponent<FirstPersonController>().currentHealth--;
                healthBar.SetHealth(other.GetComponent<FirstPersonController>().currentHealth);
            }
            Destroy(transform.gameObject);
        }
    }
}
