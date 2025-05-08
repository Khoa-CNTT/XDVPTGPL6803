using UnityEngine;
using StarterAssets;
using System.Collections;

namespace KLTNLongKhoi
{
    public class PlayerRoll : MonoBehaviour
    {
        [SerializeField] private float rollSpeed = 6f;
        [SerializeField] private float rollDuration = 1f;
        [SerializeField] private float rollCooldown = 0.2f;
        [SerializeField] private AnimationCurve rollSpeedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        [SerializeField] private AudioClip rollSoundEffect;
        [SerializeField] private float rollSoundVolume = 0.7f;
        
        private bool isRolling = false;
        private float rollTimer = 0f;
        private float cooldownTimer = 0f;
        private Vector3 rollDirection;
        private float initialRollSpeed;
        private bool rollRequested = false;
        private float verticalVelocity = 0f;
        private float gravity = -9.81f;
        
        private Animator animator;
        private CharacterAnimationEvents playerAnimationEvents;
        private StarterAssetsInputs starterAssetsInputs;
        private ThirdPersonController thirdPersonController;
        private CharacterController characterController;
        private Transform cameraTransform;
        private AudioSource audioSource;
        private PauseManager pauseManager;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            playerAnimationEvents = GetComponentInChildren<CharacterAnimationEvents>();
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            thirdPersonController = GetComponent<ThirdPersonController>();
            characterController = GetComponent<CharacterController>();
            
            // Lấy AudioSource
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Lấy PauseManager
            pauseManager = FindFirstObjectByType<PauseManager>();
            
            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Start()
        {
            if (playerAnimationEvents != null)
                playerAnimationEvents.onEndAnimation += OnEndAnimation;
                
            if (starterAssetsInputs != null)
                starterAssetsInputs.Roll += OnRollInput;
            
            // Lưu tốc độ ban đầu để sử dụng sau khi roll
            initialRollSpeed = rollSpeed;
            
            // Lấy giá trị gravity từ ThirdPersonController nếu có
            if (thirdPersonController != null)
            {
                gravity = thirdPersonController.Gravity;
            }
        }

        private void Update()
        {
            // Kiểm tra nếu game đang pause thì không xử lý
            if (pauseManager != null && pauseManager.IsPaused)
                return;
                
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Time.deltaTime;
            }
            
            // Xử lý yêu cầu roll nếu có
            if (rollRequested)
            {
                // In ra log để debug
                // Debug.Log("Roll requested. isRolling: " + isRolling + ", Grounded: " + thirdPersonController.Grounded + ", cooldownTimer: " + cooldownTimer);
                
                if (!isRolling && cooldownTimer <= 0 && thirdPersonController.Grounded)
                {
                    StartCoroutine(ExecuteRollCoroutine());
                }
                rollRequested = false;
            }
            
            if (isRolling)
            {
                // Tính toán tỉ lệ thời gian đã trôi qua
                float normalizedTime = rollTimer / rollDuration;
                
                // Áp dụng curve để làm mượt chuyển động
                float speedMultiplier = rollSpeedCurve.Evaluate(normalizedTime);
                
                // Xử lý trọng lực
                if (characterController.isGrounded && verticalVelocity < 0)
                {
                    verticalVelocity = -2f; // Reset vertical velocity khi chạm đất
                }
                else
                {
                    verticalVelocity += gravity * Time.deltaTime;
                }
                
                // Tạo vector di chuyển kết hợp cả hướng roll và trọng lực
                Vector3 moveDirection = rollDirection * (rollSpeed * speedMultiplier);
                moveDirection.y = verticalVelocity;
                
                // Di chuyển nhân vật
                characterController.Move(moveDirection * Time.deltaTime);
                
                rollTimer += Time.deltaTime;
                if (rollTimer >= rollDuration)
                {
                    EndRoll();
                }
            }
        }

        private void OnDestroy()
        {
            if (playerAnimationEvents != null)
                playerAnimationEvents.onEndAnimation -= OnEndAnimation;
                
            if (starterAssetsInputs != null)
                starterAssetsInputs.Roll -= OnRollInput;
        }

        private void OnEndAnimation()
        {
            if (isRolling)
            {
                EndRoll();
            }
        }
        
        private void EndRoll()
        {
            // Debug.Log("End roll animation");
            isRolling = false;
            rollTimer = 0f;
            cooldownTimer = rollCooldown;
            animator.SetBool("Roll", false);
            thirdPersonController.CanMove = true;
        }

        // Phương thức được gọi khi nhận input roll
        private void OnRollInput()
        {
            // Kiểm tra nếu game đang pause thì không xử lý
            if (pauseManager != null && pauseManager.IsPaused)
                return;
                
            // Debug.Log("Roll input received");
            
            // Đánh dấu yêu cầu roll để xử lý trong Update
            rollRequested = true;
        }

        // Sử dụng Coroutine để đảm bảo roll được thực hiện sau khi frame hiện tại hoàn thành
        private IEnumerator ExecuteRollCoroutine()
        {
            // Đợi đến frame tiếp theo
            yield return null;
            
            // Debug.Log("Roll executed. Grounded: " + thirdPersonController.Grounded);
            
            isRolling = true;
            rollTimer = 0f;
            animator.SetBool("Roll", true);
            
            // Tạm dừng khả năng di chuyển của ThirdPersonController
            thirdPersonController.CanMove = false;
            
            // Lấy giá trị verticalVelocity hiện tại từ ThirdPersonController nếu có thể
            if (thirdPersonController != null)
            {
                // Nếu ThirdPersonController có thuộc tính VerticalVelocity, sử dụng nó
                // Nếu không, sử dụng giá trị mặc định
                verticalVelocity = -2f;
            }

            // Phát âm thanh roll
            PlayRollSound();

            // Lấy input di chuyển
            Vector2 input = starterAssetsInputs.move;
            
            // Xác định hướng roll dựa trên input
            if (input.magnitude > 0.1f)
            {
                // Tính toán hướng dựa trên input và camera
                float targetRotation = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                Quaternion targetRotationQuat = Quaternion.Euler(0, targetRotation, 0);
                rollDirection = targetRotationQuat * Vector3.forward;
                
                // Xoay nhân vật theo hướng roll
                transform.rotation = Quaternion.Euler(0, targetRotation, 0);
            }
            else
            {
                // Nếu không có input, roll theo hướng nhìn hiện tại
                rollDirection = transform.forward;
            }
            
            rollDirection.Normalize();
            
            // Điều chỉnh tốc độ roll dựa trên tốc độ di chuyển hiện tại
            float currentSpeed = thirdPersonController.MoveSpeed;
            rollSpeed = Mathf.Max(initialRollSpeed, currentSpeed * 1.5f);
        }

        // Phương thức phát âm thanh roll
        private void PlayRollSound()
        {
            if (rollSoundEffect != null && audioSource != null)
            {
                audioSource.PlayOneShot(rollSoundEffect, rollSoundVolume);
            }
        }
    }
}
