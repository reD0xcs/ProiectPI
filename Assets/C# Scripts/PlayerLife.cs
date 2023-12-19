using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    #region Public Variables

    public int maxHealth = 100;
    public ScreenFlash screenFlash;
    public float resetTime = 5f;

    #endregion

    #region Private Variables

    private Rigidbody2D _rb;
    private Animator _anim;
    [SerializeField] private AudioSource deathSoundEffect;
    private int _currentHealth;
    private float _timeSinceLastDamage;

    #endregion
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        screenFlash = GetComponent<ScreenFlash>();
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;
        
        if (_timeSinceLastDamage >= resetTime && _currentHealth != 100)
        {
            screenFlash.FlashGreen();
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
        _currentHealth -= damage;
        _anim.SetTrigger("hurt");

        if (_currentHealth <= 0)
        {
            Die();
        }
        else
        {
            _anim.SetTrigger("hurt");
            _timeSinceLastDamage = 0f;
        }
    }

    private void Die()
    {
        deathSoundEffect.Play();
        _rb.bodyType = RigidbodyType2D.Static;
        _anim.SetTrigger("death");
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
        _currentHealth = maxHealth;
        Debug.Log("Player health reset to " + maxHealth);
    }
}