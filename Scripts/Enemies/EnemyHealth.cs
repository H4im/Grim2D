using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private float damageCooldownTime = 2f;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15;

    private float lastDamageTime = 0f;

    private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time - lastDamageTime >= damageCooldownTime)
        {
            currentHealth -= damage;
            lastDamageTime = Time.time;
            knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
            flash.TriggerFlash();
            StartCoroutine(checkDetectDeathRoutine());
        }
    }

    private IEnumerator checkDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }
    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickupSpawner>().DropItems();
            Destroy(gameObject);
        }
    }
}