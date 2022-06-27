using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BoostBehaviour : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text str, dur;
    int boostId, boostStrength, boostDuration;

    public void SetStats(int id, int str, int dur)
    {
        boostId = id;
        boostDuration = dur;
        boostStrength = str;
        this.str.text = $"{str}%";
        this.dur.text = $"{dur}s";
        this.icon.sprite = GameManager.Instance.GameUIManager.BoostIcons[id].Icon;
    }

    public void GetStatsToManager()
    {
        if (GameManager.Instance.GameUIManager.ActivateBoost(boostId, boostStrength, boostDuration))
            Destroy(gameObject);
    }
}
