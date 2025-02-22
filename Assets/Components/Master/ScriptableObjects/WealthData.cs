using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "WealthData", menuName = "Game/Wealth Data")]
public class WealthData : ScriptableObject {
    public event Action OnDollarsChanged;
    public event Action OnEarningsTimeElapsedChanged;
    public event Action OnEarningsRateChanged;

    [field: SerializeField] public float Dollars { get; private set; } = 1000;
    [field: SerializeField] public float Gems { get; private set; } = 0;
    [field: SerializeField] public float EarningsPerInterval { get; private set; } = 120;
    [field: SerializeField] public int EarningsTotalTime { get; private set; } = 720;
    [field: SerializeField] public int EarningsInterval { get; private set; } = 5;
    [field: SerializeField] private float earningsTimeElapsed;
    [field: SerializeField] public Dictionary<int, float> FinalEarningsRates { get; private set; } = new Dictionary<int, float>();

    public float EarningsTimeElapsed {
        get => earningsTimeElapsed;
        set {
            earningsTimeElapsed = value;
            OnEarningsTimeElapsedChanged?.Invoke();
        }
    }
    public bool SpendMoney(int amount) {
        if(Dollars >= amount) {
            Dollars -= amount;
            OnDollarsChanged?.Invoke();
            return true;
        }
        return false;
    }
    public void AddMoney(float amount) {
        Dollars += amount;
        OnDollarsChanged?.Invoke();
    }
    public void SetEarning() {
        AddMoney(EarningsPerInterval);
    }
    public void UpdateEarnings(int apartmentId, float newFinalEarningsRate) {
        if(FinalEarningsRates.TryGetValue(apartmentId, out float oldFinalEarningsRate)) {
            EarningsPerInterval += newFinalEarningsRate - oldFinalEarningsRate;
            FinalEarningsRates[apartmentId] = newFinalEarningsRate;
        }
        else {
            FinalEarningsRates.Add(apartmentId, newFinalEarningsRate);
            EarningsPerInterval += newFinalEarningsRate;
        }
        OnEarningsRateChanged?.Invoke();
    }
}
