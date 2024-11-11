using System.Collections;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    // The asteroid prefab to instantiate
    [SerializeField] private GameObject asteroidPrefab;

    // Maximum number of asteroids allowed on screen
    [SerializeField] private int maxAsteroidsAmount;
    private int currentAsteroidsAmount; // Current number of asteroids on screen
    private Camera mainCamera; // Reference to the main camera
    GameManager gameManager;

    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
        // Start the coroutine to spawn asteroids
        StartCoroutine(Spawn());
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.RemainingAsteroids = maxAsteroidsAmount;
    }

    // Coroutine to control the delay between asteroid spawns
    IEnumerator Spawn()
    {
        // Waits 1.5 seconds before spawning the first asteroid
        yield return new WaitForSeconds(1.5f);

        // Calls the method to spawn an asteroid
        SpawnAsteroid();
        currentAsteroidsAmount++; // Increases the asteroid counter

        // If max number of asteroids hasn't been reached, restart coroutine
        if (currentAsteroidsAmount < maxAsteroidsAmount)
        {
            StartCoroutine(Spawn());
        }
    }

    // Spawns an asteroid at a random edge of the screen
    private void SpawnAsteroid()
    {
        
        int randomScreenSide = Random.Range(0, 4); // Randomly chooses a screen side (0 to 3)
        Quaternion randomRotation = Quaternion.identity; // Initializes the asteroid's rotation
        Vector3 spawnPosition = Vector3.zero; // Initializes the spawn position
        float randomPoint = Random.Range(0f, 1f); // Gets a random position along the chosen edge

        // Assigns spawn position and rotation based on the selected screen side
        switch (randomScreenSide)
        {
            case 0: // Top
                spawnPosition = new Vector3(randomPoint, 1.1f, mainCamera.nearClipPlane + 15f);
                randomRotation = Quaternion.LookRotation(-Vector3.forward); // Facing downwards
                break;
            case 1: // Bottom
                spawnPosition = new Vector3(randomPoint, -0.1f, mainCamera.nearClipPlane + 15f);
                randomRotation = Quaternion.LookRotation(Vector3.forward); // Facing upwards
                break;
            case 2: // Right
                spawnPosition = new Vector3(1.1f, randomPoint, mainCamera.nearClipPlane + 15f);
                randomRotation = Quaternion.LookRotation(-Vector3.right); // Facing left
                break;
            case 3: // Left
                spawnPosition = new Vector3(-0.1f, randomPoint, mainCamera.nearClipPlane + 15f);
                randomRotation = Quaternion.LookRotation(Vector3.right); // Facing right
                break;
        }

        // Converts the spawn position from viewport to world coordinates
        Vector3 spawnWorldPosition = mainCamera.ViewportToWorldPoint(spawnPosition);
        
        // Randomizes the asteroid's rotation around the Y axis
        randomRotation *= Quaternion.Euler(0, Random.Range(0f, 45f), 0);
        
        // Instantiates the asteroid at the specified position and rotation
        SpawnPool.Instance.Spawn(asteroidPrefab.transform, spawnWorldPosition, randomRotation);
    }
}