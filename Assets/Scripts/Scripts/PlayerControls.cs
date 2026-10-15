using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveSpeed1 = 10f;
    public float noFuelSpeed = 2f;
    public float mouseSen1 = 2f;

    [Header("Turn&Tilt Settings")]
    public float turnSpeed = 200f;
    public float tiltAmount = 20f;
    public float tiltSpeed = 5f;

    [Header("Rotation Setting")]
    public float rot;
    private float rotx = 0f;

    [Header("Shooting")]
    public Camera playerCamera;
    public float shootingDistance;


    [Header("Fuel Settings")]
    public float fuelUse = 0.01f;

    [Header("Other Settings")]
    private GameObject target;
    public string targetTag = "Asteroid1";

    private float Health;

    private bool isDead = false;



    public PlayerStats PS;
    private Rigidbody rb;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);


        PS = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {

        if(PS != null)
        {
           Health = PS.Health;
        }


        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        MouseMovement();

        if (Health <= 0 && !isDead)
        {
            isDead = true;

            GameManager gameManager = FindFirstObjectByType<GameManager>();

            if (gameManager != null)
            {
                Debug.Log("Game Manager Found");
                gameManager.GameOver();
            }
            else
            {
                Debug.LogError("Game Manager Not Found");
            }
        }

    }


    private void FixedUpdate()
    {
        float currentSpeed = moveSpeed;

        if (PS != null && PS.Fuel <= 0)
        {
            currentSpeed = noFuelSpeed;
        }

        float turnInput = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical") * Time.deltaTime * currentSpeed;

        float rotation = -turnInput * turnSpeed * Time.fixedDeltaTime;

        rb.MoveRotation(
            rb.rotation * Quaternion.Euler(0f, rotation, 0f)
        );


        if (Input.GetKey(KeyCode.Space))
        {


            if (PS != null && PS.Fuel <= 0 )
            {
                currentSpeed = noFuelSpeed;
            }

            rb.linearVelocity = transform.forward * currentSpeed;

            if (PS != null && PS.Fuel > 0)
            {
                PS.losefuel(fuelUse);
            }
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }


        TiltShip(turnInput);
        transform.Translate(0, y, 0);
    }

    private void TiltShip(float turnInput)
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * rot;
        float targetTilt = -turnInput * tiltAmount;

        Quaternion targetRotation = Quaternion.Euler(
            0f,
            transform.eulerAngles.y,
            targetTilt
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            tiltSpeed * Time.fixedDeltaTime
        );

        transform.Translate(x, 0, 0);

    }


    private void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSen1;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSen1;


        transform.Rotate(Vector3.up * mouseX);

        rotx -= mouseY;
        rotx = Mathf.Clamp(rotx, -80f, 80f);

        if (Camera.main != null)
        {
            playerCamera.transform.localRotation =
    Quaternion.Euler(rotx, 0f, 0f);
        }
    }


    private void Shoot()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;



        if (Physics.Raycast(ray, out hit, shootingDistance))
        {
            Debug.Log("Shot hit: " + hit.collider.name);

            AsteroidTarget asteroid = hit.collider.GetComponentInParent<AsteroidTarget>();

            if (asteroid != null)
            {
                asteroid.DestroyAsteroid();
            }
        }
        else
        {
            Debug.Log("Shot fired but hit nothing.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Player hit by asteroid.");

            PS.hurt(10);
        }

        if (collision.gameObject.CompareTag("Money"))
        {
            Debug.Log("Player Obtained Money.");

            PS.addMoney(20);

            Destroy(collision.gameObject);
 
        }

        if (collision.gameObject.CompareTag("Scrap"))
        {
            Debug.Log("Player Obtained Scrap.");

            PS.addScore(10);

            Destroy(collision.gameObject);
 
        }

        if (collision.gameObject.CompareTag("Health"))
        {
            Debug.Log("Player Obtained Health.");

            PS.heal(10);

            Destroy(collision.gameObject);
 
        }
        if (collision.gameObject.CompareTag("EnemyDrone"))
        {
            Debug.Log("Player has collided with enemy Drone and has been destroyed");

            PS.hurt(100);

            Destroy(collision.gameObject);

 
        }
        if (collision.gameObject.CompareTag("MovingAsteroid"))
        {
            Debug.Log("Player has collided with a Moving asteroid. PLayer Got hurt");

            PS.hurt(50);

            Destroy(collision.gameObject);
 
        }
    }

}
