using System;

using UnityEngine;

[CreateAssetMenu(menuName = "Wealth/DollarsData")]
public class DollarsData : ScriptableObject {
    public event Action OnDollarsChanged;
    public event Action OnEarningsTimeElapsedChanged;
    public event Action OnEarningsChanged;

    [SerializeField] private int dollars = 1000;
    [SerializeField] private int earnings = 0;
    [SerializeField] private int earningsTimeElapsed;
    [field: SerializeField] public float EarningsTotalSeconds { get; private set; } = 700;
    [field: SerializeField] public float EarningsInterval { get; private set; } = 60;


    public float EarningsTimeElapsed {
        get => earningsTimeElapsed;
        set {
            earningsTimeElapsed = (int)value;
            OnEarningsTimeElapsedChanged?.Invoke();
        }
    }
    public float Dollars {
        get => dollars;
        set {
            if(dollars == value) return; // Prevent unnecessary updates
            dollars = (int)value;
            OnDollarsChanged?.Invoke();
        }
    }
    public float Earnings {
        get => earnings;
        set {
            earnings = (int)value;
            OnEarningsChanged?.Invoke();
        }
    }

    public void UpdateEarnings() {
        EarningsTimeElapsed++;
        if(EarningsTimeElapsed % EarningsInterval == 0) Dollars += Earnings;
        if(EarningsTimeElapsed >= EarningsTotalSeconds) EarningsTimeElapsed = 0;
    }
}