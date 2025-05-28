using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject laser;
    [SerializeField] private SpriteRenderer gemSpriteRenderer;

    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        gemSpriteRenderer = transform.Find("Gem")?.GetComponent<SpriteRenderer>();
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        // Always spawn laser at player position
        Vector2 spawnPosition = PlayerController.Instance.transform.position;

        GameObject newLaser = Instantiate(laser, spawnPosition, Quaternion.identity);

        Vector2 attackDirection = PlayerController.Instance.GetLastMovementDirection();
        newLaser.GetComponent<Laser>().SetLaserDirection(attackDirection);
        newLaser.GetComponent<Laser>().UpdateLaserRange(weaponInfo.weaponRange);
    }




    public void Attack()
    {
        Vector2 movementDirection = PlayerController.Instance.GetLastMovementDirection();

        myAnimator.SetTrigger("Attack");
        mySpriteRenderer.enabled = true;
        gemSpriteRenderer.enabled = true; 

        if (movementDirection.x > 0) // Right
        {
            myAnimator.SetTrigger("Right");
            mySpriteRenderer.sortingOrder = 1;
            if (gemSpriteRenderer != null)
                gemSpriteRenderer.sortingOrder = 1;
        }
        else if (movementDirection.x < 0) // Left
        {
            myAnimator.SetTrigger("Left");
            mySpriteRenderer.sortingOrder = 1;
            if (gemSpriteRenderer != null)
                gemSpriteRenderer.sortingOrder = 1;
        }
        else if (movementDirection.y > 0) // Up
        {
            myAnimator.SetTrigger("Up");
            mySpriteRenderer.sortingOrder = 1;
            if (gemSpriteRenderer != null)
                gemSpriteRenderer.sortingOrder = 1;
        }
        else // Down
        {
            myAnimator.SetTrigger("Down");
            mySpriteRenderer.sortingOrder = 4;
            if (gemSpriteRenderer != null)
                gemSpriteRenderer.sortingOrder = 4;
        }
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
    private void SetInvisible()
    {
        mySpriteRenderer.enabled = false;
        gemSpriteRenderer.enabled = false;
    }
}
