using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : MonoBehaviour
{
    private float walking_velocity = 1.0f;
    public GameObject player;
    private float max_distance = 80f;
    private float min_distance = 10f;
    private float shooting_range = 50f;
    private bool attacked = false;

    private Transform player_transform;
    private UnityEngine.AI.NavMeshAgent agent;

    public GameObject flashLight;
    public FlashlightUI flashlightUI;

    //Shooting
    private GameObject projectile_rock;

    public AudioClip sound;
    private AudioSource source;
    private bool playedSound;
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        player =  GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if(player != null)
        {
            player_transform = player.transform;

        }else
        {
            Debug.Log("Player is null");
        }
        playedSound = false;
        projectile_rock = Resources.Load("Prefabs/Pumkin/Prefab/rock",typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player_transform.position, transform.position);        
        if(distance <= max_distance && !flashLight.GetComponent<Flashlight>().enemyIsIn){

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
                    Attack();
                }
            }

        }else{
            agent.SetDestination(transform.position);
            playedSound = false;
        }
    }

    private void Attack()
    {
        transform.LookAt(player_transform);
        agent.SetDestination(transform.position);

        if(!attacked)
        {
            attacked = true;
            GameObject rock_obj = Instantiate(projectile_rock, new Vector3(transform.position.x, transform.position.y*2, transform.position.z), Quaternion.identity) as GameObject;
            rock_obj.transform.localScale = new Vector3(3.0f,3.0f,3.0f);
            Rigidbody rd = rock_obj.GetComponent<Rigidbody>();
            rd.AddForce(transform.forward * 1500f);
            rd.AddForce(transform.up * 90f);
            Invoke("ResetAttacked", 3.0f);
        }

    }
    void ResetAttacked()
    {
        attacked = false;
    }
}
