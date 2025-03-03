using UnityEngine;

public class FurnitureLocationReference : MonoBehaviour {
    [SerializeField] private ApartmentFurnishingData apartmentFurnishingData;
    [SerializeField] private Transform instantiateLocation;

    void Awake() {
        apartmentFurnishingData.InstantiateLocation = instantiateLocation;
    }
}