using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/ApartmentAreaData")]
public class ApartmentAreaData : ScriptableObject {
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public float LevelUpCost { get; private set; }
    [field: SerializeField] public float LevelUpCostMultiplier { get; private set; }
    [field: SerializeField] public float BaseEarningsRate { get; private set; }
    [field: SerializeField] public float FinalEarningsRate { get; private set; }
    [field: SerializeField] public FurnitureData EquippedFurniture { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public List<FurnitureData> FurnitureList { get; private set; }
    [field: SerializeField] public Constants.AreaType AreaType { get; private set; }
    [field: SerializeField] public Constants.AreaFurnishingType AreaFurnishingType { get; private set; }


    public void EquipFurniture(FurnitureData furniture) {
        EquippedFurniture = furniture;
        FinalEarningsRate = BaseEarningsRate * furniture.EarningsRateMultiplier;
        // Run Update Total Earnings in Wealth
    }
}