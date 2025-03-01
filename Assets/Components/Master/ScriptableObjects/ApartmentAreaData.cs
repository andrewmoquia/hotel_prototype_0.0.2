using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Game/ApartmentAreaData")]
public class ApartmentAreaData : ScriptableObject {
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public Sprite AreaTabIcon { get; private set; }
    [field: SerializeField] public Constants.AreaType AreaType { get; private set; }
    [field: SerializeField] public List<ApartmentFurnishingData> FurnishingList { get; private set; }
}