using UnityEngine;

public class HeroAttackCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
         Debug.Log("Something entered AttackCollider: " + other.name);
        // Check if the collider belongs to an enemy
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hero hit the enemy!");

            // Apply damage to the enemy
            var enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10); // Adjust damage as necessary
            }

            // Trigger the enemy's "GotHit" animation
            var enemyAgent = other.GetComponent<EnemyAgent>();
            if (enemyAgent != null)
            {
                enemyAgent.GotHit();  // Trigger the "Hit" animation on the enemy
            }
        }
    }
}
