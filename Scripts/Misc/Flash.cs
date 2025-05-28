using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material flashMat;        // Your solid Flash shader material
    [SerializeField] private float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Material originalMat;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMat = spriteRenderer.material; // Store the material at start (non-shared instance)
    }

    public void TriggerFlash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMat;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMat;
    }

    public float GetRestoreMatTime()
    {
        return flashDuration;
    }
}
