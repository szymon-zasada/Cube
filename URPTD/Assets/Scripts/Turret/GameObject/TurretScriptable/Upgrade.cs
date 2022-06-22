using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade", order = 1)]
public class Upgrade : ScriptableObject
{
    //v________________________________________________________________________________________________________________________________________________________
    [SerializeField] GameObject prefab;
    [SerializeField] int cost;
    [SerializeField] string turretName;

    //V________________________________________________________________________________________________________________________________________________________

    public GameObject Prefab => prefab;
    public int Cost => cost;
    public string TurretName => turretName;

    //F________________________________________________________________________________________________________________________________________________________



}
