using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private AudioSource deathSoundEffect;

    public int maxHealth = 100;
    private int currentHealth;

    // Time in seconds before health reset
    public float resetTime = 5f;
    private float timeSinceLastDamage = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Update the timer
        timeSinceLastDamage += Time.deltaTime;

        // Check if it's time to reset health
        if (timeSinceLastDamage >= resetTime)
        {
            ResetHealth();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("mori"))
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("hurt");
            // Reset the timer when the player takes damage
            timeSinceLastDamage = 0f;
        }
    }

    private void Die()
    {
        deathSoundEffect.Play();
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        Debug.Log("Player died!");

        Invoke("RestartLevel", 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetHealth()
    {
        // Reset the health to max
        currentHealth = maxHealth;
        Debug.Log("Player health reset to " + maxHealth);
    }
}