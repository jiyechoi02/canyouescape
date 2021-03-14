using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    // Public values
    public CharacterController controller;
    // Movement values
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
    public float speed = 20f;
    public float gravity = -19.62f;
    // Private values
    Vector3 velocity;
    bool isGrounded;
    // Player's health
    public int maxHealth = 10;
    public int currentHealth;
    public Healthbar healthBar;
    public GameObject pausedbuttons; // Paused buttons
    public GameObject diedButtons; // Died buttons
    public GameObject wonButtons; // Won buttons
    public bool pickedItemUp;
    // Checking for leftshift
    private bool isShiftKeyDown;
    // Animation
    // [SerializeField] private Animator animation_controller;

    // trap variables
    public bool gotTrapped;
    private float slowTrapTimer;

    public bool gotHit;

    // End 
    public bool onEndZone;
    public AudioClip hitting_sound;
    public AudioClip pickingup_sound;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        pickedItemUp = false;
        healthBar.SetMaxHealth(maxHealth);
        // animation_controller = GetComponent<Animator>();
        gotTrapped = false;
        slowTrapTimer = 5f;
        gotHit = false;
        source = GetComponent<AudioSource>();
        onEndZone = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is on the end zone, the game is over
        if(onEndZone)
        {
            Cursor.lockState = CursorLockMode.None; // Cursor unlock
            Cursor.visible = true; // Mouse show
            wonButtons.SetActive(true); // Buttons show
            controller.enabled = false; // Turn off player's controller
            GameObject varGameObject = GameObject.FindWithTag("MainCamera"); // Turn off camera
            varGameObject.GetComponent<MouseLook>().enabled = false; // Turn off Camera
            Time.timeScale = 0f;
        }
        // if the player gets slower by the trap
        slowTrapTimer -= Time.deltaTime;
        if(slowTrapTimer <= 0.0f)
        {
            HealTrap();
        }
        if(gotTrapped)
        {
            speed = 5f;
        }


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Either true or false

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        // Get input from wasd
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // Move using those inputs
        Vector3 move = transform.right * x + transform.forward * z;
        if (isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // animation_controller.SetBool("Running", isShiftKeyDown);
            controller.Move(move * (2f * speed) * Time.deltaTime);
        }

        controller.Move(move * speed * Time.deltaTime);
        // Jump mechanics
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // Making sure velocity does not build up before jump
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // If player reaches 0 health game over
        if (currentHealth == 0)
        {
            Cursor.lockState = CursorLockMode.None; // Cursor unlock
            Cursor.visible = true; // Mouse show
            diedButtons.SetActive(true); // Buttons show
            controller.enabled = false; // Turn off player's controller
            GameObject varGameObject = GameObject.FindWithTag("MainCamera"); // Turn off camera
            varGameObject.GetComponent<MouseLook>().enabled = false; // Turn off Camera 
        }

        // pause and shows the menu when the user presses ESC
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedbuttons.active)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pausedbuttons.SetActive(false);
                controller.enabled = true;
                GameObject varGameObject = GameObject.FindWithTag("MainCamera"); // Turn off camera
                varGameObject.GetComponent<MouseLook>().enabled = true; // Turn off Camera 
                Time.timeScale = 1f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pausedbuttons.SetActive(true);
                controller.enabled = false;
                GameObject varGameObject = GameObject.FindWithTag("MainCamera"); // Turn off camera
                varGameObject.GetComponent<MouseLook>().enabled = false; // Turn off Camera 
                Time.timeScale = 0f;
            }
        }

        if(gotHit)
        {
            source.PlayOneShot(hitting_sound);
            gotHit = false;
        }

        if(pickedItemUp)
        {
            source.PlayOneShot(pickingup_sound);
            pickedItemUp = false;
        }

    }
    void HealTrap()
    {
        gotTrapped = false;
        speed = 20f;
        slowTrapTimer = 5f;
    }
}
