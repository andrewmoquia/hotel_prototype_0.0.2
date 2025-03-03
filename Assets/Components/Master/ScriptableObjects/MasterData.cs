using System;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/MasterData")]
public class MasterData : ScriptableObject {
    public Action OnUtilitiesStorageChanged;
    [field: SerializeField] public List<ApartmentData> Apartments { get; private set; }
    [field: SerializeField] public List<UtilitiesData> Utilities { get; private set; }
    [field: SerializeField] public DollarsData DollarsData { get; private set; }
    [SerializeField] private int powerStorage = 0;
    [SerializeField] private int waterStorage = 0;
    [SerializeField] private int wasteStorage = 0;

    public int PowerStorage {
        get => powerStorage;
        set {
            powerStorage = value;
            OnUtilitiesStorageChanged?.Invoke();
        }
    }
    public int WaterStorage {
        get => waterStorage;
        set {
            waterStorage = value;
            OnUtilitiesStorageChanged?.Invoke();
        }
    }
    public int WasteStorage {
        get => wasteStorage;
        set {
            wasteStorage = value;
            OnUtilitiesStorageChanged?.Invoke();
        }
    }

    void OnEnable() {
        GetEarnings();
    }

    void GetEarnings() {
        DollarsData.Earnings = 0;
        PowerStorage = 0;
        WaterStorage = 0;
        WasteStorage = 0;

        Apartments.ForEach((apartment) => {
            apartment.ApartmentAreas.ForEach((area) => {
                area.FurnishingList.ForEach((furnishing) => {
                    DollarsData.Earnings += furnishing.FinalEarningsRate;
                    furnishing.FurnitureList.ForEach((furniture) => {
                        if(furniture == furnishing.EquippedFurniture) {
                            PowerStorage += furniture.PowerUsage;
                            WaterStorage += furniture.WaterUsage;
                            WasteStorage += furniture.WasteUsage;
                        }
                    });
                });
            });
        });
    }
}