using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
  {
    public Animator animator;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private CharacterController2D player;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 20;
    public float attackRate = 2f;
     float nextAttackTime = 0f;

    private float cooldownTimer = Mathf.Infinity;

    
    private EnemyPatrol enemyPatrol;
    public int maxHealth = 100;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindObjectOfType<CharacterController2D>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    public void TakeDamage(int damage)
    {
      currentHealth -= damage;

      animator.SetTrigger("Hurt");

      if(currentHealth <= 0)
      {
        Die();
      }
    }
    // Update is called once per frame

    private void Update()
    {
        if (PlayerInSight())
        {
          if(Time.time >= nextAttackTime)
        {
          Attack();
          
        }
        }
       if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }
    bool PlayerInSight()
    {
      
      RaycastHit2D hitPlayer = Physics2D.CircleCast(transform.position, attackRange, attackPoint.position);
      
      return hitPlayer.collider != null;      
    }

     void Attack() 
    {
     animator.SetTrigger("Attack");
    
    //  Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

      player.GetComponent<CharacterController2D>().TakeDamage(attackDamage);
      Debug.Log("Player taking damage");
      nextAttackTime = Time.time + 1f / attackRate;
      
    }


    void OnDrawGizmosSelected()
    {
    if (attackPoint == null)
      return;
    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    

    void Die()
    {
      Debug.Log("Enemy died!");  

      animator.SetBool("IsDead", true);

      GetComponent<Collider2D>().enabled = false;
      this.enabled = false;
    }
}
