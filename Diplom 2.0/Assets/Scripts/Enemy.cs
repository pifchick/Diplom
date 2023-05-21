using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    //public float attackRate = 2f;
    //float nextAttackTime = 0f;

    public Animator animator;
    public int maxHealth = 4;
    int currentHelth;
    public Rigidbody rb;
    private float stopTime;
    

    //Враг преследует 
    public float speed = 3f;
    public Transform player;
    private bool _canTakeDamage = true;
    private Vector3 _defaultLocalScale;

    //Атака врага
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private float _attackDistance = 0.9f;
    [SerializeField] private float _viewDistance = 5.0f;
    [SerializeField] private LayerMask _playerLayers;
    public int _attackDamage = 1;
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    public float timeDeath = 5f;



    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHelth = maxHealth;
        _defaultLocalScale = transform.localScale;

    }


    void Update()
    {
        if (Time.time >= nextAttackTime)
        {

            if ((transform.position - player.transform.position).magnitude <= _attackDistance)
            {
                OnAttack();
                nextAttackTime = Time.time + 4f / attackRate;
            }

        }
        if (transform.position.y < -7)
        {
            animator.SetBool("isDead", true);
            Debug.Log("Enemy died");
            Destroy();
            /*MainScript.GameOver();*/    // MainScript ваш скрипт управления игрой (называться может по разному),
                                          // в функции GameOver (назвать можно как угодно) можно делать подсчет очков, выводить окна и кнопки для начала новой игры
        }
    }

   

    void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    void OnAttack()
    {
         animator.SetTrigger("attack1");
         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayers);


         foreach (Collider2D enemy in hitEnemies)
         {
            enemy.GetComponent<PlayerCombo>().TakeDamage(_attackDamage); 
         }
    }

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            currentHelth -= damage;
            nextAttackTime = Time.time + 4f / attackRate;
            animator.SetTrigger("hurt");
            speed = 0f;


            if (currentHelth <= 0)
            {
                _canTakeDamage = false;
                Die();
            }

        }

    }

    void EndAttack()
    {
        if(currentHelth <= 0)
        {
            speed = 0f;
        }
        else
        {
            speed = 3f;
        }
    }

    void FixedUpdate()
    {
        if ((transform.position - player.transform.position).magnitude <= _viewDistance)
        {

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.fixedDeltaTime);

            float scaleX = transform.position.x > player.position.x
            ? _defaultLocalScale.x
            : -_defaultLocalScale.x;
            transform.localScale = new Vector3(scaleX, transform.localScale.y, 1f);
            if (speed != null)
            {
                animator.SetBool("run", true);
            }
            else
            {
                animator.SetBool("run", false);
            }
        }
        else
        {
            animator.SetBool("run", false);
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
       
        animator.SetBool("isDead", true);
        //GetComponent<Collider2D>().enabled = false;
        //this.enabled = false;
    }

    void Destroy()
    {
        GameObject.Destroy(gameObject);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
   
   
}
