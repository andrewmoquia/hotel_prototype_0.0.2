using UnityEngine;

[CreateAssetMenu(menuName = "Game/FurnitureData")]
public class FurnitureData : ScriptableObject {
    [Header("Furniture Info")]
    [SerializeField] private bool isPurchased;
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public float EarningsRateMultiplier { get; private set; }
    [field: SerializeField] public int PowerUsage { get; private set; }
    [field: SerializeField] public int WaterUsage { get; private set; }
    [field: SerializeField] public int WasteUsage { get; private set; }
    [field: SerializeField] public int FurnitureCost { get; private set; }
    [field: SerializeField] public Constants.RarityType RarityType { get; private set; }
    [field: SerializeField] public Constants.FurnitureType FurnitureType { get; private set; }
    [field: SerializeField] public GameObject FurniturePrefab { get; private set; }
    [field: SerializeField] public Sprite FurnitureIcon { get; private set; }
    [field: SerializeField] public GameObject FurnitureImage { get; private set; }

    public bool IsPurchased {
        get => isPurchased;
        set {
            isPurchased = value;
        }
    }
}