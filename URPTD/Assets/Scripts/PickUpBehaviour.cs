using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour : MonoBehaviour
{
    [SerializeField] GameObject cash, boost;
    [SerializeField] float boostChance;
    private void OnEnable() => MainSequence();

    void OnMouseDown()
    {
        if (boost.activeSelf)
        {
            GameManager.Instance.GameUIManager.GeneratePopUpValue(true);
            gameObject.SetActive(false);
            return;
        }


        GameManager.Instance.GameUIManager.GeneratePopUpValue(false);
        gameObject.SetActive(false);
    }

    async void SelfDestruction()
    {
        await Task.Delay(8000);
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }


    void MainSequence()
    {
        boost.SetActive(false);
        cash.SetActive(false);
        SelfDestruction();

        if (Random.Range(0f, 100f) < boostChance)
        {
            boost.SetActive(true);
            return;
        }


        cash.SetActive(true);
    }
}
