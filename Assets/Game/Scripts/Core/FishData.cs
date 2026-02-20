using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// ScriptableObject that holds all data for a specific fish type.
    /// Create different assets for different fish (Common, Rare, Epic).
    /// This keeps fish properties separate from code = easier to balance and expand.
    /// </summary>
    [CreateAssetMenu(fileName = "NewFishData", menuName = "Betta Fish/Fish Data", order = 1)]
    public class FishData : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Display name of this fish type")]
        public string fishName = "Betta Fish";

        [Tooltip("Rarity tier affects spawn rate and value")]
        public FishRarity rarity = FishRarity.Common;

        [Header("Physical Properties")]
        [Tooltip("Size multiplier (1 = normal, 0.5 = half size, 2 = double size)")]
        [Range(0.3f, 3f)]
        public float size = 1f;

        [Tooltip("Movement speed in units per second")]
        [Range(0.5f, 5f)]
        public float swimSpeed = 1.5f;

        [Header("Visual")]
        [Tooltip("Primary color of the fish body")]
        public Color fishColor = Color.white;

        // Future expansion fields (commented out for now):
        // public Sprite fishIcon;
        // public int collectionValue;
        // public GameObject customMeshPrefab;
    }
}