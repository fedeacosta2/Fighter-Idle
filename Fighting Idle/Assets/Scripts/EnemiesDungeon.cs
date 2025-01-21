using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class EnemiesDungeon : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius within which the enemy detects the player
    public float moveSpeed = 3f; // Speed at which the enemy moves towards the player
    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private PlayerController _player;
    [SerializeField] private  AnimationPlayer animationplayer;
    [SerializeField] private GameObject enemy1;
    [SerializeField] private GameObject enemy2;
    [SerializeField] private GameObject victory;
    private Animator animatorOfTheEnemies; // Reference to the animator component
    private bool isAttacking = false; // Flag to check if the enemy is 
    public HealthBar HealthBarEnemy;
    public bool damagePlayerNow = true;

    void Start()
    {
        
        animatorOfTheEnemies = GetComponent<Animator>(); // Get the animator component
    }

    void Update()
    {
        // Check if the player is within detection radius
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            
            // Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Play running animation
            animatorOfTheEnemies.SetBool("Enemy_Animations", true);
            animatorOfTheEnemies.SetBool("Enemy_Animations", false);

            // Rotate to face the player
            transform.LookAt(player.position);
        } 
       
        else
        {
            // Play idle animation
            animatorOfTheEnemies.SetBool("Enemy_Animations", false);
            animatorOfTheEnemies.SetBool("Enemy_Animations", true);
        }
        
        // Check if the enemy's health is depleted
            if (HealthBarEnemy.sliderValue <= 0)
            {
                // Deactivate only this enemy
                gameObject.SetActive(false);
            }

            if (!enemy1.activeSelf && !enemy2.activeSelf)
            {
                victory.SetActive(true);
                StartCoroutine(ChangeSceneAfterWinning(3f));
            }
    }

    IEnumerator ChangeSceneAfterWinning(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && damagePlayerNow)
        {
            Debug.Log("cagaste wachin");
            _player.PlayerDamaged();
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        // Check if the collider is the player
        if (other.CompareTag("EnemyAttackRange"))
        {
            // Play attack animation
            animatorOfTheEnemies.Play("Sword_Slash");
            isAttacking = true; // Set attacking flag
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the collider is the player
        if (other.CompareTag("EnemyAttackRange"))
        {
            isAttacking = false; // Reset attacking flag when player exits the trigger
        }
    }

    // Draw the detection radius in the editor for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    
    public void EnemyDamaged()
    {
        HealthBarEnemy.sliderValue -= 0.20f;
    }
    
    public void DamageEnemy()
    {
        damagePlayerNow = true;
    }
    
    public void DamageEnemyfalse()
    {
        damagePlayerNow = false;
    }
    
    
}
