using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class BulletController : MonoBehaviour
{
    private SphereCollider sphereCollider;
    [SerializeField]private AudioClip explodeSound;
    private AudioSource audioSource; 
    private AsteroidsSpawner asteroidsSpawner;
    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {            
        PlaySound();
        SpawnPool.Instance.Despawn(transform);
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.MakeDamage(100);
        }
    }

    private void PlaySound()
    {
        asteroidsSpawner = FindFirstObjectByType<AsteroidsSpawner>();
        audioSource = asteroidsSpawner.GetComponent<AudioSource>();
        audioSource.PlayOneShot(explodeSound, 0.5f);
    }
}