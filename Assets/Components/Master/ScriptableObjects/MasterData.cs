using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/MasterData")]
public class MasterData : ScriptableObject {
    [field: SerializeField] public List<ApartmentData> Apartments { get; private set; }
    [field: SerializeField] public List<UtilitiesData> Utilities { get; private set; }
    [field: SerializeField] public WealthData Wealth { get; private set; }
}