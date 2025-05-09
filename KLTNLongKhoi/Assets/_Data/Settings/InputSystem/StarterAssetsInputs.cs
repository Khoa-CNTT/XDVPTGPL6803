using UnityEngine;
using UnityEngine.Events;
using KLTNLongKhoi;
using System;

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
		public event Action UseItem;
		public event Action Interact;
		public event Action Attack;
		public event Action Escape;
		public event Action openInventory;
		public event Action SkillQ;
		public event Action SkillE;
		public event Action SkillC;
		public event Action Roll;
		public event Action SkillPanel;
		public event Action QuestPanel;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true; // ẩn con trỏ chuột
		public bool cursorInputForLook = true; // không cho phép xoay cam theo trỏ chuột

		private PauseManager pauseManager;

		void Awake()
		{
			pauseManager = FindFirstObjectByType<PauseManager>();
			pauseManager.onGamePaused.AddListener(OnPauseGame);
			pauseManager.onGameResumed.AddListener(OnResumeGame);
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

			if (value.isPressed)
			{
				Attack?.Invoke();
			}
		}

		public void OnEscape(InputValue value)
		{
			Escape.Invoke();
		}

		public void OnOpenInventory(InputValue value)
		{
			openInventory?.Invoke();
		}

		public void OnSkillQ(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			if (value.isPressed)
			{
				SkillQ.Invoke();
			}
		}

		public void OnSkillE(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			if (value.isPressed)
			{
				SkillE?.Invoke();
			}
		}

		public void OnSkillC(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			if (value.isPressed)
			{
				SkillC.Invoke();
			}
		}
		public void OnRoll(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			if (value.isPressed)
			{
				Roll?.Invoke();
			}
		}

		public void OnInteract(InputValue value)
		{
			if (pauseManager.IsPaused) return;

			if (value.isPressed)
			{
				Interact?.Invoke();
			}
		}

		public void OnSkillPanel(InputValue value)
		{
			if (value.isPressed)
			{
				SkillPanel?.Invoke();
			}
		}

		public void OnUseItem(InputValue value)
		{
			if (value.isPressed)
			{
				UseItem?.Invoke();
			}
		}

		public void OnQuestPanel(InputValue value)
		{
			if (value.isPressed)
			{
				QuestPanel?.Invoke();
			}
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
		#endregion

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		private void OnPauseGame() => SetActivePlayerInput(true);
		private void OnResumeGame() => SetActivePlayerInput(false);
		private void SetActivePlayerInput(bool value)
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
