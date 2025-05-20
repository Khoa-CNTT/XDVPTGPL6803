using UnityEngine;

namespace KLTNLongKhoi
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private ParticleSystem activateEffect; // Hiệu ứng khi kích hoạt
        [SerializeField] private AudioClip activateSound; // Âm thanh khi kích hoạt
        private bool hasBeenActivated = false;

        PlayerStatsManager playerStatsManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
                playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
                if (playerStatus != null && !hasBeenActivated)
                {
                    SaveCheckPoint();
                }
            }
        }

        public void SaveCheckPoint()
        {
            playerStatsManager = FindFirstObjectByType<PlayerStatsManager>();
            // Lưu vị trí checkpoint
            playerStatsManager.SetCheckpoint(transform.position);

            // Phát hiệu ứng
            if (activateEffect != null)
            {
                activateEffect.Play();
            }

            // Phát âm thanh
            if (activateSound != null)
            {
                AudioSource.PlayClipAtPoint(activateSound, transform.position);
            }

            hasBeenActivated = true;
        }
    }
}
