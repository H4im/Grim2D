using UnityEngine;

public class Walker : MonoBehaviour, IEnemy
{
    [SerializeField] private int contactDamage = 1;

    private float lastDamageTime;

    public void Attack()
    {
        // Prevent multiple hits within the cooldown handled by Enemy_AI
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.75f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(contactDamage, transform);
                    Debug.Log($"{gameObject.name} dealt {contactDamage} damage to Player.");
                }
            }
        }
    }
}
