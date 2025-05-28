using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead {  get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    private Rigidbody2D rb;


    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_TEXT = "Scene1";
    readonly int DEATH_HASH = Animator.StringToHash("Death");


    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();

    }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        Enemy_AI enemy = other.gameObject.GetComponent<Enemy_AI>();

        if (enemy)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        Debug.Log($"Trying to heal: {currentHealth}/{maxHealth}");

        if (currentHealth < maxHealth && !isDead)
        {
            currentHealth += 1;
            UpdateHealthSlider();
            Debug.Log("Player healed!");
        }
    }


    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage || isDead) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        flash.TriggerFlash();
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

private void CheckIfPlayerDeath()
{
    if (currentHealth <= 0 && !isDead)
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; 
        Destroy(ActiveWeapon.Instance.gameObject);
        currentHealth = 0;
        GetComponent<Animator>().SetTrigger(DEATH_HASH);
        canTakeDamage = false;

        Debug.Log("Player has died.");
        Debug.Log("GameOverUIController.Instance (from PlayerHealth) = " + GameOverUIController.Instance);

        if (GameOverUIController.Instance != null)
        {
            Debug.Log("Triggering game over UI...");
            GameOverUIController.Instance.TriggerGameOver();
        }
        else
        {
            Debug.LogWarning("GameOverUIController.Instance is null!");
        }
    }
}


    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
        canTakeDamage = true;
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if(healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
