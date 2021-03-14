using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPacks : MonoBehaviour
{
    public bool showText;
    public int Health;
    public GameObject Player;
    public int playerHealth;
    public Healthbar healthBar;
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
                playerHealth = Player.GetComponent<FirstPersonController>().currentHealth;
                Health = 1;
                Player.GetComponent<FirstPersonController>().currentHealth = Health + playerHealth;
                Player.GetComponent<FirstPersonController>().pickedItemUp = true;
                healthBar.SetHealth(Player.GetComponent<FirstPersonController>().currentHealth);
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
