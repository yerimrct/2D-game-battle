using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    public EnemyAgent enemyAgent;      
    public Health heroHealth;           
    public heromove heroMoveScript;     

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject != this.gameObject)
        {
            Debug.Log("Enemy hit the player!");
            if (heroHealth != null)
            {
                heroHealth.TakeDamage(10f);
            }

            // Trigger hero's hit animation
            if (heroMoveScript != null)
            {
                heroMoveScript.TakeHit();
            }

            // Reward the enemy for a successful attack
            if (enemyAgent != null)
            {
                enemyAgent.AddReward(1.0f);
                enemyAgent.SetAttackLanded(true);
            }
        }
    }
}
