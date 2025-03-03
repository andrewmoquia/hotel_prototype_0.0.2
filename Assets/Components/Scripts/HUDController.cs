using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour {
    [SerializeField] private MasterData masterData;
    public TextMeshProUGUI dollarsText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI earningsText;
    public TextMeshProUGUI powerUsageText;
    public TextMeshProUGUI waterUsageText;
    public TextMeshProUGUI wasteUsageText;

    void OnEnable() {
        masterData.DollarsData.OnDollarsChanged += UpdateDollarsDisplay;
        masterData.DollarsData.OnEarningsTimeElapsedChanged += UpdateEarningsTimeElapsed;
        masterData.DollarsData.OnEarningsChanged += UpdateEarningsDisplay;
        masterData.OnUtilitiesStorageChanged += UpdateUtilitiesStorageDisplay;

        UpdateDollarsDisplay();
        UpdateEarningsTimeElapsed();
        UpdateEarningsDisplay();
        UpdateUtilitiesStorageDisplay();
    }
    void UpdateDollarsDisplay() {
        dollarsText.text = $"{masterData.DollarsData.Dollars:c}";
    }
    void UpdateEarningsTimeElapsed() {
        int totalSeconds = Mathf.FloorToInt(masterData.DollarsData.EarningsTimeElapsed);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        timeText.text = $"{minutes:D2}:{seconds:D2}";
    }
    void UpdateEarningsDisplay() {
        earningsText.text = $"{masterData.DollarsData.Earnings}";
    }

    void UpdateUtilitiesStorageDisplay() {
        powerUsageText.text = $"{masterData.PowerStorage}";
        waterUsageText.text = $"{masterData.WaterStorage}";
        wasteUsageText.text = $"{masterData.WasteStorage}";
    }
}