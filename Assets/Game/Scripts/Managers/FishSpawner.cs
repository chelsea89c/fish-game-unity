using UnityEngine;
using Game.Core;

namespace Game.Managers
{
    /// <summary>
    /// Spawns fish into the tank with rarity-based weighting.
    /// Common fish spawn more frequently than Epic fish.
    /// Manages the fish population in the scene.
    /// </summary>
    public class FishSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        [Tooltip("The fish prefab to spawn")]
        [SerializeField] private GameObject fishPrefab;

        [Header("Fish Types")]
        [Tooltip("All available fish data assets (drag all fish types here)")]
        [SerializeField] private FishData[] availableFishTypes;

        [Header("Spawn Settings")]
        [Tooltip("How many fish to spawn when game starts")]
        [SerializeField] private int initialFishCount = 10;

        [Tooltip("Spawn fish within this area")]
        [SerializeField] private Vector3 spawnAreaCenter = Vector3.zero;

        [Tooltip("Size of spawn area")]
        [SerializeField] private Vector3 spawnAreaSize = new Vector3(8f, 4f, 8f);

        [Header("Rarity Weights")]
        [Tooltip("Higher = more likely to spawn")]
        [SerializeField] private int commonWeight = 70;
        [SerializeField] private int rareWeight = 25;
        [SerializeField] private int epicWeight = 5;

        // Track spawned fish for future management
        private GameObject fishContainer;

        private void Start()
        {
            // Create container to keep Hierarchy clean
            fishContainer = new GameObject("=== Active Fish ===");

            // Spawn initial fish population
            SpawnInitialFish();
        }

        /// <summary>
        /// Spawn all fish when game starts.
        /// </summary>
        private void SpawnInitialFish()
        {
            if (fishPrefab == null)
            {
                Debug.LogError("FishSpawner: No fish prefab assigned!", this);
                return;
            }

            if (availableFishTypes == null || availableFishTypes.Length == 0)
            {
                Debug.LogError("FishSpawner: No fish types assigned!", this);
                return;
            }

            for (int i = 0; i < initialFishCount; i++)
            {
                SpawnRandomFish();
            }

            Debug.Log($"Spawned {initialFishCount} fish in the tank.");
        }

        /// <summary>
        /// Spawn a single fish with random position and type.
        /// Public so you can call it from UI buttons later.
        /// </summary>
        public GameObject SpawnRandomFish()
        {
            // Pick random position within spawn area
            Vector3 randomPosition = GetRandomSpawnPosition();

            // Instantiate fish
            GameObject newFish = Instantiate(fishPrefab, randomPosition, Quaternion.identity, fishContainer.transform);

            // Select random fish type based on rarity weights
            FishData selectedFishData = SelectRandomFishData();

            // Apply fish data
            Fish fishComponent = newFish.GetComponent<Fish>();
            if (fishComponent != null)
            {
                Debug.Log($"[SPAWN] Found Fish component, setting data...");
                fishComponent.SetFishData(selectedFishData);
                newFish.name = $"{selectedFishData.fishName} ({selectedFishData.rarity})";
                Debug.Log($"[SPAWN] Successfully set data for {newFish.name}");
            }
            else
            {
                Debug.LogError($"[SPAWN] Fish component NOT FOUND on {newFish.name}!");
            }
            return newFish;
        }

        /// <summary>
        /// Generate random position within spawn area.
        /// </summary>
        private Vector3 GetRandomSpawnPosition()
        {
            float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
            float randomY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
            float randomZ = Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f);

            return spawnAreaCenter + new Vector3(randomX, randomY, randomZ);
        }

        /// <summary>
        /// Select a random FishData based on rarity weights.
        /// Common fish are much more likely than Epic fish.
        /// </summary>
        private FishData SelectRandomFishData()
        {
            // First, try to select based on rarity weight
            int totalWeight = commonWeight + rareWeight + epicWeight;
            int randomValue = Random.Range(0, totalWeight);

            FishRarity selectedRarity;

            if (randomValue < commonWeight)
            {
                selectedRarity = FishRarity.Common;
            }
            else if (randomValue < commonWeight + rareWeight)
            {
                selectedRarity = FishRarity.Rare;
            }
            else
            {
                selectedRarity = FishRarity.Epic;
            }

            // Find a fish of the selected rarity
            FishData[] matchingFish = System.Array.FindAll(availableFishTypes,
                fish => fish.rarity == selectedRarity);

            if (matchingFish.Length > 0)
            {
                // Return random fish of this rarity
                return matchingFish[Random.Range(0, matchingFish.Length)];
            }
            else
            {
                // Fallback: return any random fish
                Debug.LogWarning($"No fish found for rarity {selectedRarity}, using random fish.");
                return availableFishTypes[Random.Range(0, availableFishTypes.Length)];
            }
        }

        /// <summary>
        /// Visualize spawn area in Scene view.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawCube(spawnAreaCenter, spawnAreaSize);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnAreaCenter, spawnAreaSize);
        }
    }
}