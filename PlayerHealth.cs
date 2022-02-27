using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public float invincibilityFlashDelay = 0.15f;
    public bool isInvicible = false;

    public float invincibilityTimeAfterHit = 3f;
    public SpriteRenderer graphics;
    public HealthBar healthBar;

    public static PlayerHealth instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Playerhealth dans la scène");
            return;
        }

        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(50);
        }
    }

    public void HealPlayer(int amount)
    {
        if ((currentHealth + amount) < maxHealth)
        {
            currentHealth += amount;
        }
        else
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (!isInvicible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            // vérifier si le joueur est toujours vivant 
            if(currentHealth <= 0)
            {
                Die();
                return;
            }

            isInvicible = true;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(HandlerInvincibilityDelay());
        }
    }

    public void Die()
    {
        // Debug.Log("Le joueur est éliminé");
        // bloquer les mouvements du personnage
        PlayerMovement.instance.enabled = false;
        PlayerMovement.instance.rb.velocity = Vector3.zero;
        // jouer l'animation d'élimination
        PlayerMovement.instance.animator.SetTrigger("Die");
        // empêcher les interactions physique avec autres éléments de la scène
        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Kinematic;
        PlayerMovement.instance.playerCollider.enabled = false;
        // Affichage Game Over
        GameOverManager.instance.OnPlayerDeath();
    }

    public void Respawn()
    {
        // Réactiver les mouvements du personnage
        PlayerMovement.instance.enabled = true;
        // Réactiver les animations
        PlayerMovement.instance.animator.SetTrigger("Respawn");
        // Réactiver les interactions physique avec autres éléments de la scène
        PlayerMovement.instance.rb.bodyType = RigidbodyType2D.Dynamic;
        PlayerMovement.instance.playerCollider.enabled = true;
        // Rendre de la vie au joueur
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }

    public IEnumerator InvincibilityFlash()
    {
        while (isInvicible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    public IEnumerator HandlerInvincibilityDelay()
    {
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isInvicible = false;
    }
}
