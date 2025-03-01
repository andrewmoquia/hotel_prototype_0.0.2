using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/ApartmentFurnishingData")]
public class ApartmentFurnishingData : ScriptableObject {
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public int MaxLevel { get; private set; }
    [field: SerializeField] public float LevelUpCost { get; private set; }
    [field: SerializeField] public float LevelUpCostMultiplier { get; private set; }
    [field: SerializeField] public float BaseEarningsRate { get; private set; }
    [field: SerializeField] public float BaseEarningsRateMultiplier { get; private set; }
    [field: SerializeField] public float FinalEarningsRate { get; private set; }
    [field: SerializeField] public Sprite FurnishingTabIcon { get; private set; }
    [field: SerializeField] public Constants.AreaFurnishingType AreaFurnishingType { get; private set; }
    [field: SerializeField] public FurnitureData EquippedFurniture { get; private set; }
    [field: SerializeField] public List<FurnitureData> FurnitureList { get; private set; }

    void OnEnable() {
        FinalEarningsRate = BaseEarningsRate + (BaseEarningsRate * EquippedFurniture.EarningsRateMultiplier);
    }

    public void EquipFurniture(FurnitureData furniture) {
        EquippedFurniture = furniture;
        FinalEarningsRate = BaseEarningsRate + (BaseEarningsRate * furniture.EarningsRateMultiplier);
    }

    public void LevelUp() {
        if(MaxLevel == Level) return;
        Level++;
        LevelUpCost += LevelUpCost * LevelUpCostMultiplier;
        BaseEarningsRate += BaseEarningsRate * BaseEarningsRateMultiplier;
    }
}