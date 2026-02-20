using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Handles fish swimming movement within a bounded area.
    /// Uses simple waypoint navigation - fish swim to random points.
    /// No physics required, pure Transform-based movement.
    /// </summary>
    public class FishMovement : MonoBehaviour
    {
        [Header("Movement Bounds")]
        [Tooltip("Center point of the swimming area")]
        [SerializeField] private Vector3 tankCenter = Vector3.zero;

        [Tooltip("Size of the swimming area (width, height, depth)")]
        [SerializeField] private Vector3 tankSize = new Vector3(10f, 5f, 10f);

        [Header("Movement Behavior")]
        [Tooltip("How smoothly the fish rotates to face its destination")]
        [SerializeField] private float rotationSpeed = 3f;

        [Tooltip("How close fish must get to waypoint before picking a new one")]
        [SerializeField] private float waypointReachThreshold = 0.5f;

        [Tooltip("Minimum time before picking a new random waypoint")]
        [SerializeField] private float minWaypointTime = 2f;

        [Tooltip("Maximum time before picking a new random waypoint")]
        [SerializeField] private float maxWaypointTime = 5f;

        // Runtime state
        private Vector3 currentWaypoint;
        private float currentSpeed;
        private float nextWaypointChangeTime;

        private void Start()
        {
            // Pick initial waypoint when fish spawns
            PickNewWaypoint();
        }

        private void Update()
        {
            MoveTowardsWaypoint();
            RotateTowardsWaypoint();
            CheckWaypointReached();
        }

        /// <summary>
        /// Set movement speed (called by Fish component).
        /// </summary>
        public void SetSpeed(float speed)
        {
            currentSpeed = speed;
        }

        /// <summary>
        /// Override tank bounds (useful if you have multiple tanks).
        /// </summary>
        public void SetTankBounds(Vector3 center, Vector3 size)
        {
            tankCenter = center;
            tankSize = size;
        }

        /// <summary>
        /// Move fish toward current waypoint at current speed.
        /// </summary>
        private void MoveTowardsWaypoint()
        {
            Vector3 direction = (currentWaypoint - transform.position).normalized;
            transform.position += direction * currentSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Smoothly rotate fish to face movement direction.
        /// Creates natural swimming animation.
        /// </summary>
        private void RotateTowardsWaypoint()
        {
            Vector3 direction = (currentWaypoint - transform.position).normalized;

            // Only rotate if there's a direction to face
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        /// <summary>
        /// Check if fish reached waypoint or if it's time to pick a new one.
        /// </summary>
        private void CheckWaypointReached()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, currentWaypoint);

            // Pick new waypoint if:
            // 1. Fish reached current waypoint, OR
            // 2. Timer expired (prevents fish getting stuck)
            if (distanceToWaypoint < waypointReachThreshold || Time.time >= nextWaypointChangeTime)
            {
                PickNewWaypoint();
            }
        }

        /// <summary>
        /// Generate a random point within the tank bounds.
        /// </summary>
        private void PickNewWaypoint()
        {
            // Generate random position within tank bounds
            float randomX = Random.Range(-tankSize.x / 2f, tankSize.x / 2f);
            float randomY = Random.Range(-tankSize.y / 2f, tankSize.y / 2f);
            float randomZ = Random.Range(-tankSize.z / 2f, tankSize.z / 2f);

            currentWaypoint = tankCenter + new Vector3(randomX, randomY, randomZ);

            // Schedule next forced waypoint change
            nextWaypointChangeTime = Time.time + Random.Range(minWaypointTime, maxWaypointTime);
        }

        /// <summary>
        /// Draw tank bounds in Scene view for easy visualization.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Draw tank bounds as a wire cube
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(tankCenter, tankSize);

            // Draw current waypoint during play mode
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(currentWaypoint, 0.2f);

                // Draw line from fish to waypoint
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, currentWaypoint);
            }
        }
    }
}