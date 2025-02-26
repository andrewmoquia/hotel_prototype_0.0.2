using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class BuildPanelController : MonoBehaviour {

    [Header("UI Panels")]
    public GameObject buildPanel;

    public Sprite furnitureImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI powerUsageText;
    public TextMeshProUGUI waterUsageText;
    public TextMeshProUGUI wasteUsageText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI levelUpCostText;

    private Transform targetTransform;
    private ApartmentData s_ApartmentData;
    private readonly FurnitureData selectedFurnitureData;
    private readonly List<Toggle> areaToggles = new();
    public GameObject areaButtonPrefab;
    public Transform areaSelectionPanel;
    private readonly List<Toggle> furnishingToggles = new();
    public GameObject furnishingButtonPrefab;
    public Transform furnishingSelectionPanel;
    public GameObject furnitureButtonPrefab;
    public Transform furnitureSelectionPanel;
    private readonly List<Toggle> furnitureToggles = new();

    public void OpenPanel(Transform target) {
        targetTransform = target;
        s_ApartmentData = target.GetComponent<ApartmentManager>().ApartmentData;
        UpdateUIDisplay();
        buildPanel.SetActive(true);

    }

    public void ClosePanel() {
        buildPanel.SetActive(false);
    }

    private void UpdateUIDisplay() {
        ApartmentAreaData firstAreaData = null;
        for(int i = 0; i < s_ApartmentData.ApartmentAreas.Count; i++) {
            Toggle areaToggle = Helper.CreateToggle(areaButtonPrefab, areaSelectionPanel, s_ApartmentData.ApartmentAreas[i].Icon);
            // areaToggle.onValueChanged.AddListener((value) => OnToggleChange(value, areaToggle, s_ApartmentData.ApartmentAreas[i]));
            areaToggles.Add(areaToggle);
            if(i == 0) {
                areaToggle.isOn = true;
                firstAreaData = s_ApartmentData.ApartmentAreas[i];
                UpdateBuildInfoPanel(s_ApartmentData.ApartmentAreas[i]);
            }
            if(s_ApartmentData.ApartmentAreas[i].AreaFurnishingType == firstAreaData.AreaFurnishingType) {
                Toggle furnishingToggle = Helper.CreateToggle(furnishingButtonPrefab, furnishingSelectionPanel);
                furnishingToggles.Add(furnishingToggle);
                if(i == 0) {
                    furnishingToggle.isOn = true;
                }
            }
        }
    }

    private void UpdateBuildInfoPanel(ApartmentAreaData apartmentAreaData) {
        apartmentAreaData.FurnitureList.ForEach((furniture) => {
            Toggle furnitureToggle = Helper.CreateToggle(furnitureButtonPrefab, furnitureSelectionPanel);
            furnitureToggles.Add(furnitureToggle);

            if(furniture.Name == apartmentAreaData.EquippedFurniture.Name) {
                furnitureToggle.isOn = true;
            }
        });
    }

    void OnToggleChange(bool value, Toggle toggle, ApartmentAreaData apartmentAreaData) {
        //
    }
}