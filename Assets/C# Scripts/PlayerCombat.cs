using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Public Variables

    public float attackRange = 2.05f;
    public LayerMask enemyLayers;

    #endregion

    #region Private Variables

    private Animator _anim;
    private Transform _attackPoint;
    private int _attackDamage = 40;
    private float _attackRate = 2.0f;
    private float _nextAttackTime = 0f;
    [SerializeField] private AudioSource attackSound;

    #endregion
    
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _attackPoint = GetComponent<Transform>();
    }

    void Update()
    {
        if (Time.time >= _nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                _nextAttackTime = Time.time + 1f / _attackRate;
            }
        }
    }
    
    void Attack()
    {
        _anim.SetTrigger("Attack");
        attackSound.Play();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.takeDamage(_attackDamage);
            }
            else
            {
                Debug.LogWarning("Enemy object is missing the Enemy component.");
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(_attackPoint.position, attackRange);
    }
}