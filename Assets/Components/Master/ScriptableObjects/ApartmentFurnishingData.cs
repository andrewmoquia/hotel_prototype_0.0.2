using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/ApartmentFurnishingData")]
public class ApartmentFurnishingData : ScriptableObject {
    [SerializeField] private MasterData masterData;

    [Header("Basic Info")]
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public int MaxLevel { get; private set; }
    [field: SerializeField] public int LevelUpCost { get; private set; }
    [field: SerializeField] public float LevelUpCostMultiplier { get; private set; }
    [field: SerializeField] public int BaseEarningsRate { get; private set; }
    [field: SerializeField] public float BaseEarningsRateMultiplier { get; private set; }
    [field: SerializeField] public int FinalEarningsRate { get; private set; }
    [field: SerializeField] public Sprite FurnishingTabIcon { get; private set; }
    [field: SerializeField] public Constants.AreaFurnishingType AreaFurnishingType { get; private set; }
    [field: SerializeField] public FurnitureData EquippedFurniture { get; private set; }
    [field: SerializeField] public List<FurnitureData> FurnitureList { get; private set; }

    [Header("Position Info")]
    [SerializeField] private Transform instantiateLocation;
    [field: SerializeField] public float XPositionOffset { get; private set; }
    [field: SerializeField] public float YPositionOffset { get; private set; }
    [field: SerializeField] public float ZPositionOffset { get; private set; }

    public Transform InstantiateLocation {
        get => instantiateLocation;
        set => instantiateLocation = value;
    }

    void OnEnable() {
        FinalEarningsRate = (int)(BaseEarningsRate + (BaseEarningsRate * EquippedFurniture.EarningsRateMultiplier));
    }

    public void EquipFurniture(FurnitureData furniture) {
        masterData.PowerStorage -= EquippedFurniture.PowerUsage;
        masterData.WaterStorage -= EquippedFurniture.WaterUsage;
        masterData.WasteStorage -= EquippedFurniture.WasteUsage;

        masterData.PowerStorage += furniture.PowerUsage;
        masterData.WaterStorage += furniture.WaterUsage;
        masterData.WasteStorage += furniture.WasteUsage;

        masterData.DollarsData.Earnings -= FinalEarningsRate;
        EquippedFurniture = furniture;
        FinalEarningsRate = (int)(BaseEarningsRate + (BaseEarningsRate * furniture.EarningsRateMultiplier));
        masterData.DollarsData.Earnings += FinalEarningsRate;

        Helper.InstantiateObject(InstantiateLocation, furniture, XPositionOffset, YPositionOffset, ZPositionOffset);
    }

    public void LevelUp() {
        if(MaxLevel == Level) return;
        if(masterData.DollarsData.Dollars < LevelUpCost) return;

        Level++;
        LevelUpCost += (int)Math.Round(LevelUpCost * LevelUpCostMultiplier);
        BaseEarningsRate += (int)Math.Round(BaseEarningsRate * BaseEarningsRateMultiplier);
        masterData.DollarsData.Dollars -= LevelUpCost;

        FinalEarningsRate = (int)Math.Round(BaseEarningsRate + (BaseEarningsRate * EquippedFurniture.EarningsRateMultiplier));
    }

    public void PurchasedFurniture(FurnitureData furnitureData) {
        if(furnitureData.IsPurchased) return;
        if(furnitureData.FurnitureCost > masterData.DollarsData.Dollars) return;

        masterData.DollarsData.Dollars -= furnitureData.FurnitureCost;
        furnitureData.IsPurchased = true;
    }
}