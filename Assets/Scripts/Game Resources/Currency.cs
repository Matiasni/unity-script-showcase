using System;
using UnityEngine;

[Serializable]
public enum CurrencyType
{
    cur_gold,
    cur_gems
}

[Serializable]
[CreateAssetMenu(fileName = "New Currency", menuName = "ScriptableObjects/New Currency", order = 0)]
public class Currency : GameResource
{
    [SerializeField] public CurrencyType currencyType;

    public CurrencyType CurrencyType => currencyType;
}