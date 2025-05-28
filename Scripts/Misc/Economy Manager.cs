using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;
    private TMP_Text skullText;
    private int currentSkulls = 0;
    private TMP_Text soulText;
    private int currentSouls = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";
    const string SKULL_AMOUNT_TEXT = "Skull Amount Text";
    const string SOUL_AMOUNT_TEXT = "Soul Amount Text";

    public void UpdateCurrentGold()
    {
        currentGold += 1;

        if (goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        goldText.text = currentGold.ToString("D3");
    }
    public void UpdateCurrentSkulls()
    {
        currentSkulls += 1;

        if (skullText == null)
        {
            skullText = GameObject.Find(SKULL_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        skullText.text = currentSkulls.ToString("D3");
    }
    public void UpdateCurrentSouls()
    {
        currentSouls += 1;

        if (soulText == null)
        {
            soulText = GameObject.Find(SOUL_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }
        soulText.text = currentSouls.ToString("D3");
    }
    public static void ResetStatics()
    {
        if (Instance != null)
        {
            Instance.currentGold = 0;
            Instance.currentSkulls = 0;
            Instance.currentSouls = 0;

            Instance.UpdateGoldText();
            Instance.UpdateSkullText();
            Instance.UpdateSoulText();
        }
    }
    private void UpdateGoldText()
    {
        if (goldText == null)
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMPro.TMP_Text>();

        goldText.text = currentGold.ToString("D3");
    }

    private void UpdateSkullText()
    {
        if (skullText == null)
            skullText = GameObject.Find(SKULL_AMOUNT_TEXT).GetComponent<TMPro.TMP_Text>();

        skullText.text = currentSkulls.ToString("D3");
    }

    private void UpdateSoulText()
    {
        if (soulText == null)
            soulText = GameObject.Find(SOUL_AMOUNT_TEXT).GetComponent<TMPro.TMP_Text>();

        soulText.text = currentSouls.ToString("D3");
    }


}
