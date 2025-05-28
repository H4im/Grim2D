using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed = 10f;
    [SerializeField] private int burstCount = 3;
    [SerializeField] private int projectilesPerBurst = 8;
    [SerializeField][Range(0, 359)] private float angleSpread = 360f;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts = 0.3f;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool stagger = false;
    [SerializeField] private bool oscillate = false;

    private bool isShooting = false;

    private void OnValidate()
    {
        if (oscillate) stagger = true;
        if (!oscillate) stagger = false;

        if (projectilesPerBurst < 1) projectilesPerBurst = 1;
        if (burstCount < 1) burstCount = 1;
        if (timeBetweenBursts < 0.1f) timeBetweenBursts = 0.1f;
        if (restTime < 0.1f) restTime = 0.1f;
        if (startingDistance < 0.1f) startingDistance = 0.1f;
        if (angleSpread == 0) projectilesPerBurst = 1;
        if (bulletMoveSpeed <= 0) bulletMoveSpeed = 0.1f;
    }

    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;
        float timeBetweenProjectiles = stagger ? timeBetweenBursts / projectilesPerBurst : 0f;

        for (int i = 0; i < burstCount; i++)
        {
            // Oscillating pattern
            TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle);

            if (oscillate && i % 2 == 1)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            for (int j = 0; j < projectilesPerBurst; j++)
            {
                float rad = currentAngle * Mathf.Deg2Rad;
                Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
                Vector2 spawnPos = (Vector2)transform.position + direction * startingDistance;

                GameObject newBullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.SetDirection(direction);
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;

                if (stagger) yield return new WaitForSeconds(timeBetweenProjectiles);
            }

            if (!stagger) yield return new WaitForSeconds(timeBetweenBursts);
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        if (projectilesPerBurst <= 1 || angleSpread == 0)
        {
            startAngle = currentAngle = endAngle = targetAngle;
            angleStep = 0f;
            return;
        }

        float halfSpread = angleSpread / 2f;
        startAngle = targetAngle - halfSpread;
        endAngle = targetAngle + halfSpread;
        currentAngle = startAngle;
        angleStep = angleSpread / (projectilesPerBurst - 1);
    }
}
