using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class EnemyUI : MonoBehaviour
{

    //v________________________________________________________________________________________________________________________________________________________
    [SerializeField] GameObject damageValuePrefab;
    [SerializeField] GameObject damageValueCritPrefab;
    List<GameObject> damageValues = new List<GameObject>();

    //V________________________________________________________________________________________________________________________________________________________


    //F________________________________________________________________________________________________________________________________________________________

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void DamagePopUp(float value, bool isCritical)
    {
        if(isCritical)
            DamagePopHandle(Instantiate(damageValueCritPrefab, transform.position, Quaternion.identity) as GameObject, value);

        else
            DamagePopHandle(Instantiate(damageValuePrefab, transform.position, Quaternion.identity) as GameObject, value);
    }



    public async void DamagePopHandle(GameObject prefab, float value)
    {
        TMP_Text damageValue = prefab.GetComponent<TMP_Text>();
        prefab.transform.SetParent(gameObject.transform);

        damageValues.Add(prefab);

        if (damageValues.Count > 6)
        {
            GameObject x = damageValues[0];
            damageValues.Remove(x);
            Destroy(x);
        }

        float timer = 2f;
        damageValue.color = new Color32(255, 255, 255, 255);
        Vector3 pos = damageValue.transform.position;
        damageValue.text = GameManager.Instance.FormatNumber((long)(value*10f));

        while (timer > 0)
        {
            if(prefab == null)  return;

            damageValue.transform.position += new Vector3(0, timer / 500, -timer / 300);
            damageValue.color = new Color32(255, 255, 255, (byte)(255 - (127 * timer)));
            timer -= Time.deltaTime;
            await Task.Yield();
        }
        Destroy(prefab);
    }

}
