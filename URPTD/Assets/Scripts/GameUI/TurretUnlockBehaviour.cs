using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretUnlockBehaviour : MonoBehaviour
{

    //v________________________________________________________________________________________________________________________________________________________
    [SerializeField] ShopUnlock shopUnlock;
    [SerializeField] Image icon;
    [SerializeField] TMP_Text unlockName;
    [SerializeField] TMP_Text unlockCost;
    [SerializeField] int realCost;

    //V________________________________________________________________________________________________________________________________________________________

    //F________________________________________________________________________________________________________________________________________________________

    public int GetRealCost()
    {
        if (GameManager.Instance.Turrets.Count == 0)
            realCost = shopUnlock.BaseCostInGame;

        else
            realCost = shopUnlock.BaseCostInGame * (GameManager.Instance.Turrets.Count);

        return realCost;
    }

    public void SetUnlockStats(ShopUnlock s)
    {
        shopUnlock = s;
        icon.sprite = shopUnlock.Icon;
        GetRealCost();
        icon.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(shopUnlock.IconScale, shopUnlock.IconScale);
        unlockName.text = shopUnlock.UnlockName;
        gameObject.GetComponent<Button>().onClick.AddListener(SetPrefab);
    }

    void Update()
    {
        if (realCost == 0)
            return;

        GetRealCost();
        unlockCost.text = realCost.ToString();
    }

    public void SetPrefab()
    {
        if (GameManager.Instance.CrystalAmount < realCost)
            return;

        GameManager.Instance.TurretSpawnArea.SetActive(true);
        GameManager.Instance.TurretSpawnArea.GetComponent<SpawnAreaBehaviour>().SetPrefab(shopUnlock.TurretPrefab, realCost);
    }


}
