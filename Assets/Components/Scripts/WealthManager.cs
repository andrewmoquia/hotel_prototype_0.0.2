using UnityEngine;

public class WealthManager : MonoBehaviour {

    [SerializeField] private MasterData masterData;

    void Start() {
        InvokeRepeating(nameof(UpdateWealth), 1f, 1f);
    }

    void UpdateWealth() {
        masterData.DollarsData.UpdateEarnings();
    }
}