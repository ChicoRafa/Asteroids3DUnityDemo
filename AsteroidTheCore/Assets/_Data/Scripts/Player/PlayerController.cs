using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//asociates the script with the player object
// Ensures that the GameObject this script is attached to has a Rigidbody component
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour, IDamageable
{
    //Ship Game parameters
    [Header("Ship Game parameters")] [Tooltip("The health of the ship")]
    private bool isAlive = true;
    private bool canMove = true;

    private GameManager gameManager;

    //Ship Movement parameters
    [Header("Ship Movement parameters")] [Tooltip("The velocity of the ship")]
    private Rigidbody playerRigidbody;

    private Vector3 shipMovementDirection = Vector3.zero;
    [SerializeField] private float shipSpeed = 3f * 100;
    [SerializeField] private float shipRotationSpeed = 200f;
    private float originalShipSpeed;

    private Quaternion shipRotation;

    //Fire parameters
    [Header("Fire parameters")] [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField] private Transform bulletSpawnPoint;

    //Sprint parameters
    [Header("Sprint parameters")] [SerializeField]
    private float sprintEnergy = 100f;
    private float originalSprintEnergy;
    private ClassicProgressBar sprintBar;

    //Camera parameters
    [Header("Camera parameters")] [Tooltip("The camera settings")]
    private Camera mainCamera;
    
    //Sound effects
    [Header("Sound effects")] [Tooltip("The sound effects")]
    [SerializeField]private AudioClip explodeSound;
    AudioSource audioSource;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        audioSource = GetComponent<AudioSource>();
        //gets the rigidbody component from the player object
        playerRigidbody = GetComponent<Rigidbody>();
        //disables gravity and rotation on the rigidbody
        playerRigidbody.useGravity = false;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX |
                                      RigidbodyConstraints.FreezePositionY;

        //gets the main camera
        mainCamera = Camera.main;
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //this is a slower way to get the main camera
        originalShipSpeed = shipSpeed;
        originalSprintEnergy = sprintEnergy;
        sprintBar = FindFirstObjectByType<ClassicProgressBar>();
        sprintBar.FillAmout = 1f;
    }

    //Physhics
    private void FixedUpdate()
    {
        AllowMovement();
        //rotates the ship to face the mouse
        playerRigidbody.rotation = Quaternion.RotateTowards(playerRigidbody.rotation, shipRotation,
            shipRotationSpeed * Time.fixedDeltaTime);
    }

    public void OnMove(InputValue inputValue)
    {
        //recieves an input in 2 dimensions
        Vector2 vectorInput = inputValue.Get<Vector2>();
        //as the ship moves in 3 dimensions, we only want to move in the x and z axis (y is up and down)
        shipMovementDirection = new Vector3(vectorInput.x, 0f, vectorInput.y);
        //normalizes the vector to ensure that the ship moves at the same speed in all directions
        shipMovementDirection = shipMovementDirection.normalized;
    }

    public void OnFire(InputValue inputValue)
    {
        //creates a bullet at the bullet spawn point with the same rotation as the ship
        SpawnPool.Instance.Spawn(bulletPrefab.transform, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    public void OnLook(InputValue inputValue)
    {
        Vector2 mousePosition = inputValue.Get<Vector2>();
        //converts the mouse position to a world position based on the camera position and the distance from the camera to the ship
        float zCoord = mainCamera.transform.position.y - playerRigidbody.position.y;
        Vector3 worldMousePosition =
            mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zCoord));
        //gets the direction the ship should face
        Vector3 shipForward = worldMousePosition - transform.position;
        //sets the y value to 0 to ensure the ship doesn't rotate up or down
        shipForward.y = 0f;
        //sets the rotation of the ship to face the mouse   
        shipRotation = Quaternion.LookRotation(shipForward);
    }

    public void OnSprint(InputValue inputValue)
    {
        //cancels restoring the energy when the player is sprinting
        CancelInvoke(nameof(RestoreShipEnergy));
        if (shipSpeed.Equals(originalShipSpeed))
        {
            shipSpeed += shipSpeed * 0.3f;
            InvokeRepeating(nameof(DecreaseSprintEnergy), 0f, 1f);
        }
        else
        {
            CancelInvoke(nameof(DecreaseSprintEnergy));
            RestoreShipSpeed();
            //if the ship is not sprinting, it will restore the energy after 1 second
            InvokeRepeating(nameof(RestoreShipEnergy), 1f, 1f);
        }
    }

    private void RestoreShipSpeed()
    {
        shipSpeed = originalShipSpeed;
    }

    private void DecreaseSprintEnergy()
    {
        if (sprintEnergy > 0)
        {
            sprintEnergy -= 10f;
            sprintBar.FillAmout -= 0.1f;
            if (sprintEnergy < 0)
            {
                sprintEnergy = 0;
                InvokeRepeating(nameof(RestoreShipEnergy), 1f, 1f);
            }
        }
        else
        {
            // Stops the repeated call when the energy is depleted
            CancelInvoke(nameof(DecreaseSprintEnergy));
            RestoreShipSpeed();
            StartCoroutine(DisableMovementForSeconds(1f));
            InvokeRepeating(nameof(RestoreShipEnergy), 0f, 1f);
        }
    }

    private void RestoreShipEnergy()
    {
        if (sprintEnergy < originalSprintEnergy)
        {
            sprintEnergy += 5f;
            sprintBar.FillAmout += 0.05f;
        }
        else
        {
            // Stops the repeated call when the energy is fully restored
            CancelInvoke(nameof(RestoreShipEnergy));
        }
    }

    private void AllowMovement()
    {
        if (canMove)
        {
            //moves the ship in the direction of the input at a constant speed
            playerRigidbody.linearVelocity = shipMovementDirection * (shipSpeed * Time.fixedDeltaTime);
        }
        else
        {
            playerRigidbody.linearVelocity = Vector3.zero;
        }
    }
    
    private IEnumerator DisableMovementForSeconds(float seconds)
    {
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

    /*
     * This method is called when the player is hit by an asteroid.
     */
    public void MakeDamage(int damage)
    {
        audioSource.PlayOneShot(explodeSound, 0.5f);
        if (gameManager.Lives > 1)
        {
            gameManager.Lives -= damage;
        }
        else
        {
            gameManager.Lives -= damage;
            isAlive = false;
            gameManager.SetState(GameState.GameOver);
        }
    }
}