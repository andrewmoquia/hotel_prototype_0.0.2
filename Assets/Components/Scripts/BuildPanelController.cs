using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;
using Unity.Collections;
using System;

public class BuildPanelController : MonoBehaviour {

    [Header("UI Panels")]
    public GameObject buildPanel;

    public Sprite furnitureImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI powerUsageText;
    public TextMeshProUGUI waterUsageText;
    public TextMeshProUGUI wasteUsageText;
    public TextMeshProUGUI dollarRateText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Button levelUpButton;
    public Button selectDesignButton;

    private Transform targetTransform;
    private ApartmentData s_ApartmentData;
    private readonly List<Toggle> areaToggles = new();
    public GameObject areaButtonPrefab;
    public Transform areaSelectionPanel;
    private Toggle selectedArea;
    private readonly List<Toggle> furnishingToggles = new();
    public GameObject furnishingButtonPrefab;
    public Transform furnishingSelectionPanel;
    private Toggle selectedFurnishing;
    public GameObject furnitureButtonPrefab;
    public Transform furnitureSelectionPanel;
    private readonly List<Toggle> furnitureToggles = new();
    private Toggle selectedFurniture;

    public void OpenPanel(Transform target) {
        targetTransform = target;
        s_ApartmentData = target.GetComponent<ApartmentManager>().ApartmentData;
        UpdateUIDisplay();
        buildPanel.SetActive(true);

    }
    public void ClosePanel() {
        buildPanel.SetActive(false);
        ClearTogglesAndList();
    }
    private void UpdateUIDisplay() {
        ClearTogglesAndList();

        for(int i = 0; i < s_ApartmentData.ApartmentAreas.Count; i++) {
            ApartmentAreaData areaData = s_ApartmentData.ApartmentAreas[i];
            Toggle areaToggle = Helper.CreateToggle(areaButtonPrefab, areaSelectionPanel, areaData.AreaTabIcon);
            areaToggle.name = areaData.AreaType.ToSafeString();

            if(i == 0) {
                areaToggle.isOn = true;
                selectedArea = areaToggle;
                CreateFurnishingToggles(areaData);
            }

            areaToggles.Add(areaToggle);
            areaToggle.onValueChanged.AddListener((value) => {
                OnToggleChange(value, areaToggle, areaToggles, ref selectedArea);
                if(value) CreateFurnishingToggles(areaData);
            });
        }
    }
    void CreateFurnishingToggles(ApartmentAreaData areaData) {
        foreach(var toggle in furnishingToggles) Destroy(toggle.gameObject);
        furnishingToggles.Clear();

        for(int i = 0; i < areaData.FurnishingList.Count; i++) {
            ApartmentFurnishingData furnishingData = areaData.FurnishingList[i];
            Toggle furnishingToggle = Helper.CreateToggle(furnishingButtonPrefab, furnishingSelectionPanel, furnishingData.FurnishingTabIcon);
            furnishingToggle.name = furnishingData.AreaFurnishingType.ToSafeString();

            if(i == 0) {
                furnishingToggle.isOn = true;
                selectedFurnishing = furnishingToggle;
                CreateFurnitureToggles(furnishingData);
            }

            furnishingToggles.Add(furnishingToggle);
            furnishingToggle.onValueChanged.AddListener((value) => {
                OnToggleChange(value, furnishingToggle, furnishingToggles, ref selectedFurnishing);
                if(value) CreateFurnitureToggles(furnishingData);
            });
        }
    }
    void CreateFurnitureToggles(ApartmentFurnishingData apartmentFurnishingData) {
        foreach(var toggle in furnitureToggles) Destroy(toggle.gameObject);
        furnitureToggles.Clear();

        for(int i = 0; i < apartmentFurnishingData.FurnitureList.Count; i++) {
            FurnitureData furnitureData = apartmentFurnishingData.FurnitureList[i];
            Toggle furnitureToggle = Helper.CreateToggle(furnitureButtonPrefab, furnitureSelectionPanel, furnitureData.FurnitureIcon);
            furnitureToggle.name = furnitureData.Name;

            if(apartmentFurnishingData.EquippedFurniture == furnitureData) {
                furnitureToggle.isOn = true;
                selectedFurniture = furnitureToggle;
                UpdateInfoDisplay(furnitureData, apartmentFurnishingData);
            }

            furnitureToggles.Add(furnitureToggle);
            furnitureToggle.onValueChanged.AddListener((value) => {
                OnToggleChange(value, furnitureToggle, furnitureToggles, ref selectedFurniture);
                if(value) UpdateInfoDisplay(furnitureData, apartmentFurnishingData);
            });
        }

        void UpdateInfoDisplay(FurnitureData furnitureData, ApartmentFurnishingData apartmentFurnishingData) {
            levelText.text = $"Level: {apartmentFurnishingData.Level.ToSafeString()}";
            rarityText.text = furnitureData.RarityType.ToString();
            powerUsageText.text = furnitureData.PowerUsage.ToString();
            waterUsageText.text = furnitureData.WaterUsage.ToString();
            wasteUsageText.text = furnitureData.WasteUsage.ToString();
            nameText.text = furnitureData.Name.ToString();
            descriptionText.text = furnitureData.Description.ToString();

            if(furnitureData == apartmentFurnishingData.EquippedFurniture) {
                dollarRateText.text = $"{apartmentFurnishingData.FinalEarningsRate}";
            }
            else {
                int calculatedFinalRate = (int)Math.Round(apartmentFurnishingData.BaseEarningsRate + (apartmentFurnishingData.BaseEarningsRate * furnitureData.EarningsRateMultiplier));
                dollarRateText.text = $"{calculatedFinalRate}";
            }

            Button l_Button = levelUpButton.GetComponent<Button>();
            TextMeshProUGUI l_ButtonText = l_Button.GetComponentInChildren<TextMeshProUGUI>();
            if(apartmentFurnishingData.MaxLevel == apartmentFurnishingData.Level) {
                l_ButtonText.text = $"Max Level";
            }
            else {
                l_ButtonText.text = $"Level Up: {apartmentFurnishingData.LevelUpCost}";
                l_Button.onClick.RemoveAllListeners();
                l_Button.onClick.AddListener(() => {
                    apartmentFurnishingData.LevelUp();
                    UpdateInfoDisplay(furnitureData, apartmentFurnishingData);
                });
            }

            Button d_Button = selectDesignButton.GetComponent<Button>();
            Image d_ButtonImage = selectDesignButton.GetComponent<Image>();
            TextMeshProUGUI d_ButtonText = selectDesignButton.GetComponentInChildren<TextMeshProUGUI>();
            d_Button.onClick.RemoveAllListeners();
            if(apartmentFurnishingData.EquippedFurniture == furnitureData) {
                d_Button.interactable = false;
                d_ButtonImage.color = new Color32(210, 210, 210, 255);
                d_ButtonText.color = new Color32(121, 121, 121, 255);
                d_ButtonText.text = "Equipped Furniture";
            }
            else {
                d_Button.interactable = true;
                d_ButtonImage.color = Color.white;
                d_ButtonText.color = Color.black;


                if(furnitureData.IsPurchased) {
                    d_ButtonText.text = "Select Furniture";
                    d_Button.onClick.AddListener(() => {
                        apartmentFurnishingData.EquipFurniture(furnitureData);
                        UpdateInfoDisplay(furnitureData, apartmentFurnishingData);
                    });
                }
                else {
                    d_ButtonText.text = $"Buy Furniture: {furnitureData.FurnitureCost}";
                    d_Button.onClick.AddListener(() => {
                        apartmentFurnishingData.PurchasedFurniture(furnitureData);
                        UpdateInfoDisplay(furnitureData, apartmentFurnishingData);
                    });
                }

            }
        }
    }
    void OnToggleChange(bool value, Toggle targetToggle, List<Toggle> targetToggles, ref Toggle selectedToggle) {
        if(value) {
            selectedToggle = targetToggle;
            foreach(var t_Toggle in targetToggles) {
                if(t_Toggle != targetToggle) t_Toggle.isOn = false;
            }
        }
        else {
            if(selectedToggle == targetToggle) targetToggle.isOn = true;
        }
    }
    void ClearTogglesAndList() {
        foreach(var toggle in areaToggles) Destroy(toggle.gameObject);
        foreach(var toggle in furnishingToggles) Destroy(toggle.gameObject);
        foreach(var toggle in furnitureToggles) Destroy(toggle.gameObject);
        areaToggles.Clear();
        furnishingToggles.Clear();
        furnitureToggles.Clear();
        selectedArea = null;
        selectedFurnishing = null;
        selectedFurniture = null;
    }
}