using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUnlockBehaviour : MonoBehaviour
{
    [SerializeField] GameStats gameStats;
    [SerializeField] ShopUnlock shopUnlock;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text unlockName;
    [SerializeField] TMP_Text unlockCostInGame, unlockInCostInGameFinal, reduceCost;
    [SerializeField] TMP_Text firstUnlockCost;
    [SerializeField] GameObject unlockBtn, reduceCostBtn, maxUpgrade;
    [SerializeField] TMP_Text coinAmount;

    void OnEnable()
    {
        SetUnlockStats();
    }
    public void SetUnlockStats()
    {
       // shopUnlock = s;
        icon.sprite = shopUnlock.Icon;
        icon.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(shopUnlock.IconScale, shopUnlock.IconScale);
        unlockName.text = shopUnlock.UnlockName;
        unlockBtn.GetComponent<Button>().onClick.AddListener(UnlockPrefab);
        reduceCostBtn.GetComponent<Button>().onClick.AddListener(ReduceCost);
        coinAmount = GameObject.Find("GameStats").transform.GetChild(1).GetComponent<TMP_Text>();
        RefreshBtn();
    }

    public void RefreshBtn()
    {
        coinAmount.text = gameStats.CCoinCount.ToString();
        reduceCost.text = shopUnlock.ReduceCost.ToString();
        unlockCostInGame.text = shopUnlock.BaseCostInGame.ToString();
        if (shopUnlock.IsUnlocked)
        {
            unlockBtn.SetActive(false);

            if (shopUnlock.ReduceCostLeft > 0)
                reduceCostBtn.SetActive(true);
            else
            {
                reduceCostBtn.SetActive(false);
                maxUpgrade.SetActive(true);
                unlockInCostInGameFinal.text = shopUnlock.BaseCostInGame.ToString();
            }


            return;
        }

        unlockBtn.SetActive(true);
        firstUnlockCost.text = shopUnlock.UnlockCost.ToString();
        reduceCostBtn.SetActive(false);
    }

    public void UnlockPrefab()
    {
        Debug.Log("0");
        if (gameStats.CCoinCount < shopUnlock.UnlockCost)
            return;

        Debug.Log("1");


        gameStats.ChangeBalanceCCoin(-shopUnlock.UnlockCost);
        shopUnlock.Unlock();

        RefreshBtn();
    }

    public void ReduceCost()
    {
        if (shopUnlock.ReduceCostLeft <= 0)
            return;

        if (shopUnlock.ReduceCost > gameStats.CCoinCount)
            return;

        gameStats.ChangeBalanceCCoin(-shopUnlock.ReduceCost);
        shopUnlock.ReduceCostHandle();
        RefreshBtn();
    }


}
