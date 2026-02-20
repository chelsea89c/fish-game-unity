using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Main fish component that applies FishData to a GameObject.
    /// Handles initialization and visual setup.
    /// Does NOT handle movement (that's FishMovement's job).
    /// </summary>
    [RequireComponent(typeof(FishMovement))]
    public class Fish : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("The data asset that defines this fish's properties")]
        [SerializeField] private FishData fishData;

        [Header("References")]
        [Tooltip("The renderer component (usually on child object)")]
        [SerializeField] private Renderer fishRenderer;

        // Cached components
        private FishMovement fishMovement;
        private MaterialPropertyBlock propertyBlock;

        // Public accessors for other systems
        public FishData Data => fishData;
        public string FishName => fishData != null ? fishData.fishName : "Unknown";
        public FishRarity Rarity => fishData != null ? fishData.rarity : FishRarity.Common;

        private void Awake()
        {
            // Cache component reference
            fishMovement = GetComponent<FishMovement>();

            // Auto-find renderer if not assigned in Inspector
            if (fishRenderer == null)
            {
                fishRenderer = GetComponentInChildren<Renderer>();
            }

            // MaterialPropertyBlock allows changing material properties
            // without creating new material instances (better performance)
            propertyBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            // Apply fish data when the game starts
            if (fishData != null)
            {
                InitializeFish();
            }
            else
            {
                Debug.LogWarning($"Fish '{gameObject.name}' has no FishData assigned!", this);
            }
        }

        /// <summary>
        /// Apply all properties from FishData to this GameObject.
        /// Call this after changing fishData at runtime.
        /// </summary>
        public void InitializeFish()
        {
            Debug.Log($"[FISH] InitializeFish called for {gameObject.name}");

            if (fishData == null)
            {
                Debug.LogError($"[FISH] Cannot initialize - fishData is NULL!", this);
                return;
            }

            Debug.Log($"[FISH] Initializing with data: {fishData.fishName}");

            ApplySize();
            ApplyColor();
            ApplySpeed();

            Debug.Log($"[FISH] Initialization complete!");
        }

        /// <summary>
        /// Set the data for this fish and reinitialize.
        /// Used by spawner to assign fish types dynamically.
        /// </summary>
        public void SetFishData(FishData data)
        {
            Debug.Log($"[FISH] SetFishData called with: {(data != null ? data.fishName : "NULL")}");
            fishData = data;

            if (Application.isPlaying)
            {
                InitializeFish();
            }
        }

        /// <summary>
        /// Scale the GameObject based on fish size.
        /// </summary>
        private void ApplySize()
        {
            transform.localScale = Vector3.one * fishData.size;
        }

        /// <summary>
        /// Apply fish color to the material.
        /// Uses MaterialPropertyBlock to avoid creating material copies.
        /// </summary>
        private void ApplyColor()
        {
            if (fishRenderer == null) return;

            // Get current property block, modify it, then set it back
            fishRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", fishData.fishColor); // URP Lit shader property
            fishRenderer.SetPropertyBlock(propertyBlock);
        }

        /// <summary>
        /// Pass swim speed to the movement component.
        /// </summary>
        private void ApplySpeed()
        {
            if (fishMovement != null)
            {
                fishMovement.SetSpeed(fishData.swimSpeed);
            }
        }

        // Optional: Visualize fish info in Scene view
        private void OnDrawGizmosSelected()
        {
            if (fishData != null)
            {
                // Draw a label showing fish name and rarity
#if UNITY_EDITOR
                UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f,
                    $"{fishData.fishName} ({fishData.rarity})");
#endif
            }
        }
    }
}