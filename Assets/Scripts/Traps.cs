using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public AudioClip sound;
    //public AudioClip trap2_sound;
    private AudioSource source;

    public Healthbar healthBar;
    void Start()
    {
        healthBar = GameObject.Find("Healthbar").GetComponent<Healthbar>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            source.PlayOneShot(sound);
            if(gameObject.name == "Trap1(Clone)")
            {
                Debug.Log("Enter on trap 1!");
                other.GetComponent<FirstPersonController>().gotTrapped = true;
            }else if(gameObject.name == "Trap2(Clone)")
            {
                Debug.Log("Enter on trap2");
                other.GetComponent<FirstPersonController>().currentHealth--;
                healthBar.SetHealth(other.GetComponent<FirstPersonController>().currentHealth);
            }
        }  
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Exit!");
        }
    }
}
