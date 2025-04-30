using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAgent : Agent
{
    public Transform hero;
    public Animator animator;
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public Health myHealth;
    public Health heroHealth;
    public float attackCooldown = 1f;
    private float lastAttackTime = 2f;
    private Rigidbody2D rb;
    
    public GameObject attackCollider; // Assign in inspector


    private SpriteRenderer spriteRenderer; // For flipping the sprite

    [SerializeField] private float flipCooldown = 0.5f;

    private float lastFlipTime = 1f;
    private bool recentlyFlipped = false;

    private float walkingTime = 0f;
    private float maxContinuousWalkTime = 1f;

    private bool attackLanded = false;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (attackCollider != null)
            attackCollider.SetActive(false);
    }

    public override void OnEpisodeBegin()
    {
        myHealth.ResetHealth();
        lastAttackTime = Time.time; // Reset attack cooldown
        
    }
    public void SetAttackLanded(bool value)
        {
            attackLanded = value;
        }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector2 relativePos = hero.position - transform.position;
    sensor.AddObservation(relativePos.normalized);
    sensor.AddObservation(Vector2.Distance(transform.position, hero.position));
    
    // Observe health status
    sensor.AddObservation(myHealth.currentHealth / myHealth.maxHealth);
    sensor.AddObservation(heroHealth.currentHealth / heroHealth.maxHealth);
    
    // Observe movement
    sensor.AddObservation(rb.linearVelocity);
    sensor.AddObservation(hero.GetComponent<Rigidbody2D>().linearVelocity); // Hero's velocity

    // Optionally: Is hero attacking or jumping?
    sensor.AddObservation(hero.GetComponent<Animator>().GetBool("Attack") ? 1f : 0f);
    sensor.AddObservation(hero.GetComponent<Animator>().GetBool("isJumping") ? 1f : 0f);
    }

  public override void OnActionReceived(ActionBuffers actions)
{
    int moveAction = actions.DiscreteActions[0];
    int attackAction = actions.DiscreteActions[1];

    Vector2 moveDir = Vector2.zero;
    float distanceToHero = Vector2.Distance(transform.position, hero.position);
    Vector2 directionToHero = (hero.position - transform.position).normalized;

    // Detect direction flip
    bool shouldFlip = (directionToHero.x > 0 && !spriteRenderer.flipX) || (directionToHero.x < 0 && spriteRenderer.flipX);
    if (shouldFlip)
    {
        spriteRenderer.flipX = directionToHero.x > 0;
        recentlyFlipped = true;
        lastFlipTime = Time.time;
        FlipAttackCollider();
    }

    bool inFlipCooldown = recentlyFlipped && Time.time - lastFlipTime < flipCooldown;

    // Handle idle cooldown after flip
    if (inFlipCooldown)
    {
        moveDir = Vector2.zero;
        animator.SetBool("isWalking", false);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        AddReward(-0.001f); // tiny penalty to discourage too much flipping
    }
    else
    {
        recentlyFlipped = false;
        moveDir = directionToHero;

        if (moveDir != Vector2.zero)
        {
            rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, rb.linearVelocity.y);
            animator.SetBool("isWalking", true);

            walkingTime += Time.deltaTime;
            if (walkingTime > maxContinuousWalkTime)
            {
                AddReward(-0.002f); // walking too much
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            walkingTime = 0f;
        }
    }

    // Handle attack
    if (attackAction == 1 && Time.time - lastAttackTime >= attackCooldown && distanceToHero <= attackRange)
    {
        animator.SetTrigger("Attack");
        // EnableattackCollider();
        lastAttackTime = Time.time;
        attackLanded = false;
        
    }

    AddReward(-0.001f); // small time penalty
}



    private void EnableattackCollider()
{
    if (attackCollider != null && Vector2.Distance(transform.position, hero.position) <= attackRange)
    {
        


        attackCollider.SetActive(true);
        Debug.Log("Attack Collider Enabled!");
    }
}


   private void DisableattackCollider()
{
    if (attackCollider != null)
    {
        attackCollider.SetActive(false);

        if (!attackLanded)
        {
            AddReward(-0.1f); // penalty for missing the hero
            Debug.Log("Missed attack! Penalty applied.");
        }
    }
}
    private void FlipAttackCollider()
{
    if (attackCollider != null)
    {
        Vector3 localPos = attackCollider.transform.localPosition;
        localPos.x *= -1; // Mirror the x position
        attackCollider.transform.localPosition = localPos;
    }
}


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 0;
        discreteActions[1] = 0;

        if (Input.GetKey(KeyCode.A)) discreteActions[0] = 1;
        else if (Input.GetKey(KeyCode.D)) discreteActions[0] = 2;

        if (Input.GetKey(KeyCode.Space)) discreteActions[1] = 1;
    }

    public void GotHit()
    {
        animator.SetTrigger("Hit");
        AddReward(-1.0f);
    }

    public void Died()
    {
        animator.SetTrigger("Die");
        AddReward(-2.0f);
        // EndEpisode();
    }

    public void HeroDied()
    {
        AddReward(+2.0f);
        EndEpisode();
    }
}
