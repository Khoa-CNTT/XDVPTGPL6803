using UnityEngine;
using UnityEngine.Events;
using KLTNLongKhoi;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool attack; // Thêm biến attack
		public UnityEvent Escape;
		public UnityEvent openInventory;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true; // ẩn con trỏ chuột
		public bool cursorInputForLook = true; // không cho phép xoay cam theo trỏ chuột

		PauseManager pauseManager;

		void Awake()
		{
			pauseManager = FindFirstObjectByType<PauseManager>();
			pauseManager.onGamePaused.AddListener(OnPauseGame);
		}

		void Start()
		{
			SetCursorState(cursorLocked);
		}

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			SprintInput(value.isPressed);
		}

		public void OnAttack(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			AttackInput(value.isPressed);
		}

		public void OnEscape(InputValue value)
		{
			Escape.Invoke();
		}

		public void OnOpenInventory(InputValue value)
		{
			openInventory.Invoke();
		}
#endif

#region UI Input
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AttackInput(bool newAttackState)
		{
			attack = newAttackState;
		}
#endregion

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void OnPauseGame(bool value)
		{
			bool activeMouse = !value;

			cursorLocked = activeMouse;
			cursorInputForLook = activeMouse;
			SetCursorState(cursorLocked);
			look = Vector2.zero;
			move = Vector2.zero;
		}
	}
}
