using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private GameObject goldCoin;
    [SerializeField] private int minCoins = 1;
    [SerializeField] private int maxCoins = 4;

    [Header("Orb Settings")]
    [SerializeField] private GameObject healthGlobe;
    [SerializeField] private GameObject staminaGlobe;

    public void DropItems()
    {
        if (goldCoin != null && healthGlobe == null && staminaGlobe == null)
        {
            int coinCount = Random.Range(minCoins, maxCoins + 1); // inclusive max
            for (int i = 0; i < coinCount; i++)
            {
                Vector3 offset = Random.insideUnitCircle * 0.2f;
                Instantiate(goldCoin, transform.position + offset, Quaternion.identity);
            }
        }
        else if (goldCoin == null && healthGlobe != null && staminaGlobe != null)
        {
            GameObject chosenGlobe = Random.value < 0.5f ? healthGlobe : staminaGlobe;
            Instantiate(chosenGlobe, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has an incomplete or mixed PickupSpawner setup.");
        }
    }
}
