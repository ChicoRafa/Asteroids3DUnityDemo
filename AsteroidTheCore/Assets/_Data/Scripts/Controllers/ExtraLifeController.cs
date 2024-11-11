using UnityEngine;
using UnityEngine.Serialization;

public class ExtraLifeController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]private AudioClip collectSound;
    private AudioSource audioSource; 
    private ExtraLifeSpawner extraLifeSpawner;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //In case the life gets to contact the ship and lives are less than the maximum amount, we get an extra life
        if (other.GetComponentInParent<PlayerController>() && gameManager.Lives < 3)
        {
            PlaySound();
            gameManager.Lives++;
            SpawnPool.Instance.Despawn(transform);
        }
    }

    private void PlaySound()
    {
        extraLifeSpawner = FindFirstObjectByType<ExtraLifeSpawner>();
        audioSource = extraLifeSpawner.GetComponent<AudioSource>();
        audioSource.PlayOneShot(collectSound, 0.5f);
    }
}