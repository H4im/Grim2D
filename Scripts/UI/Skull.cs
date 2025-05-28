using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Skull : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform ProjectileSpawnPoint;
     
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        if (weaponInfo == null)
        {
            weaponInfo = GetComponent<WeaponInfo>();
        }
    }

    public void Attack()
    {
        Vector2 movementDirection = PlayerController.Instance.GetLastMovementDirection();
        if (movementDirection == Vector2.zero)
        {
            movementDirection = Vector2.down;
        }

        myAnimator.SetTrigger("Attack");
        mySpriteRenderer.enabled = true;

        Vector2 spawnOffset = Vector2.zero;

        if (movementDirection.x > 0) // Right
        {
            spawnOffset = new Vector2(0.3f, 0);
            myAnimator.SetTrigger("Right");
            mySpriteRenderer.sortingOrder = 1;
        }
        else if (movementDirection.x < 0) // Left
        {
            spawnOffset = new Vector2(-0.3f, 0);
            myAnimator.SetTrigger("Left");
            mySpriteRenderer.sortingOrder = 1;
        }
        else if (movementDirection.y > 0) // Up
        {
            spawnOffset = new Vector2(0, 0.3f);
            myAnimator.SetTrigger("Up");
            mySpriteRenderer.sortingOrder = 1;
        }
        else // Down
        {
            spawnOffset = new Vector2(0, -0.3f);
            myAnimator.SetTrigger("Down");
            mySpriteRenderer.sortingOrder = 4;
        }

        Vector3 spawnPosition = PlayerController.Instance.transform.position + (Vector3)spawnOffset;

        GameObject newProjectile = Instantiate(ProjectilePrefab, spawnPosition, Quaternion.identity);
        newProjectile.GetComponent<Projectile>().SetDirection(movementDirection);
        newProjectile.GetComponent<Projectile>().UpdateProjectileRange(weaponInfo.weaponRange);
    }



    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
    private void SetInvisible()
    {
        mySpriteRenderer.enabled = false;
    }

}
