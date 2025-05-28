using UnityEngine;

public static class GameReset
{
    public static void ResetAll()
    {
        if (PlayerHealth.Instance != null)
            Object.Destroy(PlayerHealth.Instance.gameObject);

        if (Stamina.Instance != null)
            Object.Destroy(Stamina.Instance.gameObject);

        if (EconomyManager.Instance != null)
            Object.Destroy(EconomyManager.Instance.gameObject);

        if (GameOverUIController.Instance != null)
            Object.Destroy(GameOverUIController.Instance.gameObject);

        EconomyManager.ResetStatics();

    }
}

