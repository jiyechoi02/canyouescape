using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    [SerializeField] private List<GameObject> points; // Points for AI to travel to
    [SerializeField] private NavMeshAgent agent; // to get the AI
    [SerializeField] private GameObject player; // to get the Player
    [SerializeField] private GameObject Flight; // to get the flashlight for better mechanics
    [SerializeField] private GhostAI[] fellowAI; // Store all AI

    private int currPoint; // Current location
    public bool patrolling; // Check to see if AIs are patrolling
    private Vector3 temphold; // temporary hold the player's last known position
    public bool showText;
    private bool takingDamage;
    public float timer;
    public Healthbar healthBar;
    AudioSource theAudio;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        theAudio = GetComponent<AudioSource>();
        showText = false;
        takingDamage = false;
        patrolling = true;
        agent.autoBraking = false; // true or false is up to me
        currPoint = 0; // = the first location from the points list
        agent.destination = points[currPoint].transform.position; // Move to this location
    }

    // Update is called once per frame
    void Update()
    {
        // At the start or when patrolling is true, go normally
        if (patrolling)
        {
            // If distance between AI and player, is it true? This is for chasing player
            if (Vector3.Distance(this.transform.position, player.transform.position) <= 38f)
            {
                theAudio.Play(0);
                showText = true;
                if (Flight.GetComponent<Flashlight>().isOn == true)
                {
                    agent.speed = 0f;
                    if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
                    {
                        // Taking damage from AI
                        takingDamage = true;
                        TakeDamage(1);
                    }
                }
                else
                {
                    AttackPlayer(); // If one of the AI is in range it knows where the player is and called others for help
                    agent.speed = 15f;
                    agent.destination = player.transform.position; // AI then go to player's location
                    if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
                    {
                        // Taking damage from AI
                        takingDamage = true;
                        TakeDamage(1);
                    }
                }
            }
            // If distance between AI and player, is it true? This is for loosing player's sight so back to patrol
            if (Vector3.Distance(this.transform.position, player.transform.position) > 38f)
            {
                showText = false;
                agent.speed = 10f;
                agent.destination = points[currPoint].transform.position; // Move to the current way point location which is the last time you saw the player 
            }
            // If distance between AI and player, is it true? This is for loosing player's sight so back to patrol
            if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
            {
                // Taking damage from AI
                takingDamage = true;
                TakeDamage(1);

            }
            // If distance between AI and waypoint, is it true? This is for patroling between pre-determined waypoints
            if (Vector3.Distance(this.transform.position, points[currPoint].transform.position) < 8f)
            {
                Iterate(); // Iterate through the next location
            }
        }
        // If not patrolling (meaning attacking), check distance to player, if so keep attacking, if not set patrolling to true
        if (!patrolling)
        {
            // At least one of the AI is within range
            if (CheckDistance())
            {
                if (Flight.GetComponent<Flashlight>().isOn == true)
                {
                    agent.speed = 0f;
                    if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
                    {
                        // Taking damage from AI
                        takingDamage = true;
                        TakeDamage(1);
                    }
                }
                else
                {
                    agent.speed = 15f;
                    AttackPlayer(); // If one of the AI is in range it knows where the player is and called others for help
                    if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
                    {
                        // Taking damage from AI
                        takingDamage = true;
                        TakeDamage(1);
                    }
                }
            }
            else
            {
                temphold = this.GetComponent<NavMeshAgent>().destination; // Store the last known position of the player into temphold
                AddWayPoints(); // After get the last known position we then add a new way point to the scene
                patrolling = true;
            }
        }
    }

    // Iterate through the next location
    void Iterate()
    {
        // Allow us to add as many
        // 1234 in our list 0123 hence why - 1
        // 0 < 3, 1 < 3, 2 < 3
        if (currPoint < points.Count - 1)
        {
            currPoint++;
        }
        // Once it reaches the last one it will loop back to 0
        // This prevents out of bounds
        else
        {
            currPoint = 0;
        }
        agent.destination = points[currPoint].transform.position; // Move to this location
    }

    // Call other AIs to help attacking player
    void AttackPlayer()
    {
        foreach (GhostAI n in fellowAI)
        {
            n.patrolling = false; // Then we set AI to stop patrolling and attack player instead
            n.agent.destination = player.transform.position; // Set all AI to go to player's position
        }
    }

    // Check to see if AI is within the range of player
    // Whatever AI either the first one or the last one, what is your distance to player?
    public bool CheckDistance()
    {
        for (int i = 0; i < fellowAI.Length; i++)
        {
            // If least one of the AI is within range then we know it's within the player's distance so return true
            if (Vector3.Distance(fellowAI[i].transform.position, player.transform.position) <= 38f)
            {
                return true;
            }
        }
        return false;
    }

    // Add empty game object into our scenes
    void AddWayPoints()
    {
        points.Add(new GameObject()); // Add new game object to the scene
        points[points.Count - 1].transform.position = temphold; // Create a new game object at the player last known position through temphold
    }

    // Testing
    void TakeDamage(int damage)
    {
        if (timer >= 0)
        {
            if (takingDamage)
            {
                timer -= Time.deltaTime;
            }
        }

        if (timer <= 0)
        {
            timer = 1;
            if (takingDamage)
            {
                player.GetComponent<FirstPersonController>().currentHealth -= damage;
                healthBar.SetHealth(player.GetComponent<FirstPersonController>().currentHealth);
            }
        }
    }

    // Display if AI is nearby
    void OnGUI()
    {
        if (showText)
        {
            GUI.Box(new Rect(Screen.width / 3f, Screen.height / 2f, Screen.width / 4f, Screen.height / 20f), "A GHOST IS NEARBY LOOK AROUND AND RUN!!!");
        }
    }
}
