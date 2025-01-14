using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerScript : MonoBehaviour, IDeath, ISlowable
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public AudioClip[] audioClips;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    //private int numPuerta = 0;
    private float downSpeed = 0;
    private float slowDuration = 0;
    private bool slowed = false;
    private float slowCronometro = 0;


    CharacterController characterController;
    AudioSource audioSource;
    Animator animator;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    private HealthComponent myHealth;
    public BoxCollider[] puertas;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private PauseScript pauseScript;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        myHealth = GetComponent<HealthComponent>();
        audioSource = GetComponent<AudioSource>();

        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        AudioListener.pause = PlayerPrefs.GetInt("isMuted") == 1 ? true : false;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? ((isRunning ? runningSpeed : walkingSpeed) - downSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? ((isRunning ? runningSpeed : walkingSpeed) - downSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove && Time.timeScale != 0)
        {
            bool hasHorizontalInput = !Mathf.Approximately(Input.GetAxis("Horizontal"), 0);
            bool hasVerticalInput = !Mathf.Approximately(Input.GetAxis("Vertical"), 0);
            bool isWalking = hasHorizontalInput || hasVerticalInput;
            animator.SetBool("isWalking", isWalking);
            audioSource.clip = isRunning ? audioClips[1] : audioClips[0];
            if (isWalking)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                audioSource.Stop();
            }
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        if (slowed)
        {
            slowCronometro += Time.deltaTime;
            if (slowCronometro >= slowDuration)
            {
                downSpeed = 0;
                slowCronometro = 0;
                slowed = false;
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("GoblinSword"))
        {
            myHealth.TakeDamage(10);
        }
        else if (coll.gameObject.CompareTag("ControlPuerta"))
        {
            GameObject go = coll.gameObject;
            CombatManager manager = go.GetComponent<CombatManager>();
            if (manager != null)
            {
                manager.Lock();
            }
        }
    }

    public void die()
    {
        Time.timeScale = 0;
        deathUI.SetActive(true);
        pauseScript.yaNoSePuedePausar();
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = audioClips[2];
        audioSource.Play();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SlowDown(float downS, float duration)
    {
        downSpeed = downS;
        slowCronometro = 0;
        slowDuration = duration;
        slowed = true;
    }
}
