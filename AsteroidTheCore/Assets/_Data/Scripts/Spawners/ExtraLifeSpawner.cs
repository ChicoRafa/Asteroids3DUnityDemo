using System.Collections;
using UnityEngine;

public class ExtraLifeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject extraLifePrefab;
    private Camera mainCamera;
    
    [SerializeField] private int maxExtraLivesAmount;
    private int currentExtraLivesAmount;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(5f);
        SpawnExtraLife();
        currentExtraLivesAmount++;
        if (currentExtraLivesAmount < maxExtraLivesAmount)
        {
            StartCoroutine(Spawn());
        }
    }

    private void SpawnExtraLife()
    {
        int randomScreenSide = Random.Range(0, 4); // Randomly chooses a screen side (0 to 3)
        Vector3 spawnPosition = Vector3.zero; // Initializes the spawn position
        float randomPoint = Random.Range(0f, 1f); // Gets a random position along the chosen edge
        Quaternion randomRotation = Quaternion.identity; // Initializes the life's rotation
        
        
        // Assigns spawn position based on the selected screen side
        switch (randomScreenSide)
        {
            case 0: // Top
                spawnPosition = new Vector3(randomPoint, 1f, mainCamera.nearClipPlane + 15f);
                randomRotation = Quaternion.LookRotation(-Vector3.forward); // Facing downwards
                break;
            case 1: // Bottom
                spawnPosition = new Vector3(randomPoint, 0f, mainCamera.nearClipPlane + 15f);
                randomRotation = Quaternion.LookRotation(Vector3.forward); // Facing upwards
                break;
            case 2: // Right
                spawnPosition = new Vector3(1f, randomPoint, mainCamera.nearClipPlane + 15f);
                randomRotation = Quaternion.LookRotation(-Vector3.right); // Facing left
                break;
            case 3: // Left
                spawnPosition = new Vector3(0f, randomPoint, mainCamera.nearClipPlane + 15f);
                randomRotation = Quaternion.LookRotation(Vector3.right); // Facing right
                break;
        }

        // Converts the spawn position from viewport to world coordinates
        Vector3 spawnWorldPosition = mainCamera.ViewportToWorldPoint(spawnPosition);

        // Instantiates the extra life at the specified position
        SpawnPool.Instance.Spawn(extraLifePrefab.transform, spawnWorldPosition, randomRotation);
    }
}