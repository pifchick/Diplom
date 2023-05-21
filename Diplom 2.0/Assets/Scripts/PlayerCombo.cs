using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerCombo : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Animator animator;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _enemyLayers;

    [SerializeField] private HealthSustem _healthSystem;
    // Update is called once per frame
    public int _attackDamage = 1;


    private bool _canTakeDamage = true;
    public int maxHealth = 4;
    int currentHelth;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    [SerializeField] public GameObject RestartGame;

    public LevelChanger levelChanger;

    void Start()
    {
        currentHelth = maxHealth;
        RestartGame.SetActive(false);

    }

    void Update()
    {
       
        if (currentHelth <= 0)
        {
            RestartGame.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            RestartGame.SetActive(false);
            Time.timeScale = 1;
        }
        //Если игрок опустить нижу по у, то умрет
        if (transform.position.y < -7)
        {
            Debug.Log("Player died");
            RestartGame.SetActive(true);
            /*MainScript.GameOver();*/    // MainScript ваш скрипт управления игрой (называться может по разному),
                                          // в функции GameOver (назвать можно как угодно) можно делать подсчет очков, выводить окна и кнопки для начала новой игры
        }

        if (transform.position.x >= 131.6554)
        {
            Debug.Log("Next Level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            levelChanger.FadeToLevel();
        }

    }

    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
                animator.SetTrigger("attack");
                nextAttackTime = Time.time + 1f / attackRate;

        }
    }

    public void OnAttack()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayers);


        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(_attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    //Получение урона
    

    public void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            currentHelth -= damage;
            _healthSystem.SetHealth(currentHelth); //для передачи сердец
            animator.SetTrigger("hurt");
            //playerMovement.speed = 0f;

            if (currentHelth <= 0)
            {
                _canTakeDamage = false;
                Die();

            }

        }

    }
    void EndAttack()
    {
        playerMovement.speed = 10f;
    }

    void Die()
    {
        Debug.Log("Player died");
        animator.SetBool("isDead", true);

        //GetComponent<Collider2D>().enabled = false;
        //this.enabled = false;
    }

    void Destroy()
    {
        GameObject.Destroy(gameObject);
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
}
