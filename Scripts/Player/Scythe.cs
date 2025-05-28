using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Scythe : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    private Transform weaponCollider;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;


    private GameObject slashAnim;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;
    }

    private void Update()
    {
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }


    public void Attack()
    {
        Vector2 movementDirection = PlayerController.Instance.GetLastMovementDirection();

            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);

            mySpriteRenderer.enabled = true;
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);

            slashAnim.transform.parent = this.transform.parent;
            if (slashAnim != null && slashAnim.activeInHierarchy)
            {
                Vector3 spawnPosition = slashAnim.transform.position;
                spawnPosition.y += 4f;
                slashAnim.transform.position = spawnPosition;
            }
            if (movementDirection.x > 0) // Right
            {
                myAnimator.SetTrigger("Right");
                mySpriteRenderer.sortingOrder = 1;
                slashAnim.transform.position = slashAnimSpawnPoint.position;
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
                weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (movementDirection.x < 0) // Left
            {
                myAnimator.SetTrigger("Left");
                mySpriteRenderer.sortingOrder = 1;
                slashAnim.transform.position = slashAnimSpawnPoint.position;
                slashAnim.transform.rotation = Quaternion.Euler(0, 180, 0);
                weaponCollider.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (movementDirection.y > 0) // Up
            {
                myAnimator.SetTrigger("Up");
                mySpriteRenderer.sortingOrder = 1;
                slashAnim.transform.position = slashAnimSpawnPoint.position;
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, 90);
                weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else // Down
            {
                myAnimator.SetTrigger("Down");
                mySpriteRenderer.sortingOrder = 4;
                slashAnim.transform.position = slashAnimSpawnPoint.position;
                slashAnim.transform.rotation = Quaternion.Euler(0, 0, -90);
                weaponCollider.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
    }

    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    private void SetInvisible()
    {
        mySpriteRenderer.enabled = false;
    }
}
