using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bat : MonoBehaviour
{

    
    public float walking_velocity = 1.0f;
    public float wall_height;
    public Vector3 movement_direction;
    public GameObject player;
    private float max_distance = 50f;
    private float min_distance = 5f;
    private float shooting_range = 30f;

    private Transform player_transform;
    private NavMeshAgent agent;

   // private GameObject pumkin_obj;
    private GameObject projectile_pumkin;
    private float shooting_speed;
    private bool is_player_close;
    private float shooting_delay;
    private bool attacked;

    public GameObject flashLight;
    public GameObject parentObject;

    //Audio
    public AudioClip sound;
    private AudioSource source;
    private bool playedSound;

    void Start()
    {
        Debug.Log("Hello from Bat");
        player =  GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
        playedSound = false;
        if(player != null){
            player_transform = player.transform;
        }else{
            Debug.Log("player is null");
        }
        projectile_pumkin = Resources.Load("Prefabs/Pumkin/Prefab/pumkin",typeof(GameObject)) as GameObject;

        shooting_speed = 3.0f;
        is_player_close = false;
        shooting_delay = 1.0f;
        wall_height = 3.0f;

        attacked = false;
    }
    void Update()
    {
        float distance = Vector3.Distance(player_transform.position, transform.position);        
        if(distance <= max_distance && !flashLight.GetComponent<Flashlight>().enemyIsIn){
            //transform.LookAt(player_transform);
            // is_player_close = true;
            if(distance >= min_distance)
            {
                if(!playedSound){
                    source.PlayOneShot(sound);
                    playedSound = true;
                }
                agent.SetDestination(player_transform.position);
                agent.speed = 15f;
                if(distance <= shooting_range)
                {
                    Shoot();
                }
            }
            
          //  Shoot();
        }else{
            agent.SetDestination(transform.position);
            playedSound = false;
        }
    }

    private void jump()
    {

    }
    private void Shoot()
    {
        transform.LookAt(player_transform);
        agent.SetDestination(transform.position);
        if(!attacked)
        {
            attacked = true;
            GameObject pumkin_obj = Instantiate(projectile_pumkin, new Vector3(transform.position.x, transform.position.y*2, transform.position.z), Quaternion.identity) as GameObject;
            pumkin_obj.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        //    pumkin_obj.GetComponent<Pumkin>().bat = gameObject;
            Rigidbody rd = pumkin_obj.GetComponent<Rigidbody>();
            rd.AddForce(transform.forward * 500f);
            rd.AddForce(transform.up * 70f);
            Invoke("ResetAttacked", 2.0f);
        }

    }
    void ResetAttacked()
    {
        attacked = false;
    }

}

