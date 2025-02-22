using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/ApartmentAreaData")]
public class ApartmentAreaData : ScriptableObject {
    [SerializeField] public int Id { get; private set; }
    [SerializeField] public int Level { get; private set; }
    [SerializeField] public float LevelUpCost { get; private set; }
    [SerializeField] public float LevelUpCostMultiplier { get; private set; }
    [SerializeField] public float BaseEarningsRate { get; private set; }
    [SerializeField] public float FinalEarningsRate { get; private set; }
    [SerializeField] public FurnitureData EquippedFurniture { get; private set; }
    [SerializeField] public List<FurnitureData> FurnitureList { get; private set; }
    [SerializeField] public Constants.AreaType AreaType { get; private set; }
    [SerializeField] public Constants.AreaFurnishingType AreaFurnishingType { get; private set; }


    public void EquipFurniture(FurnitureData furniture) {
        EquippedFurniture = furniture;
        FinalEarningsRate = BaseEarningsRate * furniture.EarningsRateMultiplier;
        // Run Update Total Earnings in Wealth
    }
}