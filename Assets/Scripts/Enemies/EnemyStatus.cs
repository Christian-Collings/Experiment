using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Subtracts from the enemy's total health.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            TakeDamage(50);
        }
    }
}
