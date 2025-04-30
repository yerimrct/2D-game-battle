using UnityEngine;
using UnityEngine.UI; // Added for Image

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    public EnemyAgent agent; // Only set this for the enemy!
    public Animator heroAnimator; // Reference to the Hero's Animator
    public GameManager gameManager; // Reference to the GameManager

    public Slider healthSlider; 
    public Image fillImage;

    public Color healthyColor = Color.green;
    public Color midColor = new Color(1f, 0.64f, 0f); 
    public Color lowColor = Color.red;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        UpdateHealthUI();

        if (agent != null)
        {
            agent.GotHit();
        }

        if (currentHealth <= 0)
        {
            if (agent != null)
            {
                agent.Died();
            }
            else
            {
                EnemyAgent enemyAgent = FindObjectOfType<EnemyAgent>();
                if (enemyAgent != null)
                {
                    enemyAgent.HeroDied();
                }

                if (heroAnimator != null)
                {
                    heroAnimator.SetTrigger("isDead");
                }
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (fillImage != null)
        {
            float healthPercent = currentHealth / maxHealth;

            if (healthPercent > 0.5f)
            {
                fillImage.color = healthyColor;
            }
            else if (healthPercent > 0.2f)
            {
                fillImage.color = midColor;
            }
            else
            {
                fillImage.color = lowColor;
            }
        }
    }

    public void OnHeroDeathAnimationEnd()
    {
        if (gameManager != null)
        {
            gameManager.HeroLoses();
        }
    }

    public void OnEnemyDeathAnimationEnd()
    {
        if (gameManager != null)
        {
            gameManager.HeroWins();
        }
    }
}
