using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Managers;

namespace Game.UI
{
    /// <summary>
    /// Quản lý giao diện người dùng.
    /// Hiển thị số cá, nút spawn, và thông tin cá được chọn.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FishSpawner fishSpawner;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI fishCountText;
        [SerializeField] private Button spawnFishButton;
        [SerializeField] private TextMeshProUGUI selectedFishInfoText;

        private int currentFishCount = 0;

        private void Start()
        {
            // Gắn sự kiện cho nút spawn
            if (spawnFishButton != null)
            {
                spawnFishButton.onClick.AddListener(OnSpawnButtonClicked);
            }

            UpdateFishCount();
        }

        private void Update()
        {
            // Đếm số cá hiện tại trong scene
            int fishInScene = GameObject.FindGameObjectsWithTag("Fish").Length;
            if (fishInScene != currentFishCount)
            {
                currentFishCount = fishInScene;
                UpdateFishCount();
            }
        }

        /// <summary>
        /// Cập nhật text hiển thị số cá
        /// </summary>
        private void UpdateFishCount()
        {
            if (fishCountText != null)
            {
                fishCountText.text = $"Số cá: {currentFishCount}";
            }
        }

        /// <summary>
        /// Xử lý khi người chơi nhấn nút Spawn
        /// </summary>
        private void OnSpawnButtonClicked()
        {
            if (fishSpawner != null)
            {
                fishSpawner.SpawnRandomFish();
                Debug.Log("Đã spawn thêm 1 con cá!");
            }
        }

        /// <summary>
        /// Hiển thị thông tin cá khi người chơi click
        /// </summary>
        public void ShowFishInfo(string fishName, string rarity, float size, float speed)
        {
            if (selectedFishInfoText != null)
            {
                selectedFishInfoText.text = $"<b>{fishName}</b>\n" +
                                           $"Độ hiếm: {rarity}\n" +
                                           $"Kích thước: {size:F1}\n" +
                                           $"Tốc độ: {speed:F1}";
            }
        }
    }
}