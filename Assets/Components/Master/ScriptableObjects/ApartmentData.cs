using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/ApartmentData")]
public class ApartmentData : ScriptableObject {
    [field: SerializeField] public bool IsPurchased { get; private set; }
    [field: SerializeField] public float ApartmentCost { get; private set; }
    [field: SerializeField] public Constants.ApartmentType ApartmentType { get; private set; }
    [field: SerializeField] public List<ApartmentAreaData> ApartmentAreas { get; private set; }
}