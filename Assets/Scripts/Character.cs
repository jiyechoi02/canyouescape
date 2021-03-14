using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    float timer;
    [SerializeField] private Animator animation_controller;
    //[SerializeField] private Animation animation_player;
    // Use this for initialization
    void Start()
    {
        timer = 5f;
        animation_controller = GetComponent<Animator>();
        //animation_player = GetComponent<Animation>();
    }
    // Update is called once per frame
    void Update()
    {
        if (animation_controller.GetBool("isIdle") == false)
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                animation_controller.SetBool("isIdle", true);
            }
        }
        if (animation_controller.GetBool("isIdle") == true)
        {
            timer += Time.deltaTime;
            if (timer > 5f)
            {
                animation_controller.SetBool("isIdle", false);
            }
        }
    }
}
