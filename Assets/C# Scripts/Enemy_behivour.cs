using UnityEngine;

public class Enemy_behivour : MonoBehaviour
{
    #region Public Variables

    public Transform rayCast;
    public LayerMask reycastMask;
    public float rayCastLenght;
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public Transform enemyAttackPoint; 
    public ScreenFlash screenFlash;

    #endregion

    #region Private Variables

    private RaycastHit2D _hit;
    private GameObject _target;
    private Animator _anim;
    private float _distance;
    private bool _attackMode;
    private bool _inRange;
    private bool _cooling;
    private float _intTimer;
    private bool _canAttack = true;
    private bool _isUnderPlayerAttack = false;
    [SerializeField] private AudioSource attackSound;

    #endregion
    
    void Awake()
    {
        _intTimer = timer;
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        screenFlash = GetComponent<ScreenFlash>();
    }
    
    void Update()
    {
        if (_inRange)
        {
            _hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLenght, reycastMask);
            RaycastDebugger();
        }

        if (_hit.collider != null)
        {
            EnemyLogic();
        }
        else if (_hit.collider == null)
        {
            _inRange = false;
        }

        if (_inRange == false)
        {
            _anim.SetBool("canWalk", false);
            StopAttack();
        }
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            _target = trig.gameObject;
            _inRange = true;
        }
    }

    void EnemyLogic()
    {
        _distance = Vector2.Distance(transform.position, _target.transform.position);

        if (_distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (attackDistance >= _distance && _cooling == false)
        {
            Attack();
        }

        if (_cooling)
        {
            Cooldown();
            _anim.SetBool("Attack", false);
        }
    }

    void Move()
    {
        _anim.SetBool("canWalk", true);

        if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_attack"))
        {
            Vector2 targetPosition = new Vector2(_target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        timer = _intTimer;
        _attackMode = true;

        _anim.SetBool("canWalk", false);
        _anim.SetBool("Attack", true);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && _cooling && _attackMode)
        {
            _cooling = false;
            timer = _intTimer;
        }
    }

    void StopAttack()
    {
        _cooling = false;
        _attackMode = false;
        _anim.SetBool("Attack", false);
    }

    void RaycastDebugger()
    {
        if (_distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLenght, Color.green);
        }
    }

    public void TriggerCooling()
    {
        _cooling = true;
    }
    
    public void TriggerAttack()
    {
        if (_inRange)
        {
            if (enemyAttackPoint != null && _distance <= attackDistance)
            {
                PlayerLife playerLife = _target.GetComponent<PlayerLife>();
                if (playerLife != null)
                {
                    // Take damage from enemy attacks
                    attackSound.Play();
                    screenFlash.FlashRed();
                    playerLife.TakeDamage(20);
                }
            }
        }
    }
}