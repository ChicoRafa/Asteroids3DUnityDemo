using System;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IDamageable
{
    private int health = 1;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void MakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameManager.RemainingAsteroids--;
            SpawnPool.Instance.Despawn(transform);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            damageable.MakeDamage(1);
            SpawnPool.Instance.Despawn(transform);
        }
    }
}
