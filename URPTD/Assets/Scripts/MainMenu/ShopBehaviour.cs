using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBehaviour : MonoBehaviour
{
    [SerializeField] GameStats gameStats;
    [SerializeField] GameObject shopUnlockPrefab;
    [SerializeField] GameObject shopList;

    // void OnEnable()
    // {
    //     foreach(Transform child in shopList.transform)
    //         Destroy(child.gameObject);

    //     foreach (ShopUnlock shopUnlock in gameStats.ListOfUnlocks)
    //     {
    //         GameObject x = Instantiate(shopUnlockPrefab, transform.position, transform.rotation);
    //         x.transform.SetParent(shopList.transform);
    //         x.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
    //         x.GetComponent<ShopUnlockBehaviour>().SetUnlockStats(shopUnlock);
    //     }
    // }
}
