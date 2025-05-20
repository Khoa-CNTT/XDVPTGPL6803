using System;
using StarterAssets;
using UnityEngine;

namespace KLTNLongKhoi
{
    public class PlayerDrinkBottle : MonoBehaviour
    {
        [SerializeField] private QuickSlotsController quickSlotsController;
        [SerializeField] private float drinkDuration = 1.5f; // Thời gian để uống nước
        private StarterAssetsInputs starterAssetsInputs;
        private Animator animator;
        private bool isDrinking = false;
        private PlayerStatus playerStatus;
        private ThirdPersonController thirdPersonController;
        private CharacterAnimationEvents characterAnimationEvents;

        private void Awake()
        {
            quickSlotsController = FindFirstObjectByType<QuickSlotsController>();
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
            animator = GetComponentInChildren<Animator>();
            playerStatus = GetComponent<PlayerStatus>();
            thirdPersonController = GetComponent<ThirdPersonController>();
            characterAnimationEvents = GetComponentInChildren<CharacterAnimationEvents>();
        }

        void OnEnable()
        {
            characterAnimationEvents.onDrinkBottle += DrinkBottle;
            starterAssetsInputs.DrinkBottle += OnDrinkBottleInput;
        }

        void OnDisable()
        {
            characterAnimationEvents.onDrinkBottle -= DrinkBottle;
            starterAssetsInputs.DrinkBottle -= OnDrinkBottleInput;
        }

        /// <summary> khi event DrinkBottle được kích hoạt thì gọi phương thức này </summary>
        private void DrinkBottle()
        {
            InventoryCell inventoryCellSelected = quickSlotsController.InventoryCellSelected;
            if (inventoryCellSelected != null)
            {
                if (inventoryCellSelected.ItemDataSO != null)
                {
                    if (inventoryCellSelected.ItemsCount > 0 && inventoryCellSelected.ItemDataSO.ItemData.itemType == ItemType.Consumable)
                    {
                        float healthRecovery = inventoryCellSelected.ItemDataSO.ItemData.healthRecovery;
                        playerStatus.RestoreHealth(healthRecovery);
                        quickSlotsController.UseItem();
                    }
                }
            }
        }

        private void OnDrinkBottleInput()
        { 
            if (isDrinking || thirdPersonController.canMove == false) return; 
            InventoryCell inventoryCellSelected = quickSlotsController.InventoryCellSelected;
            if (inventoryCellSelected != null)
            { 
                if (inventoryCellSelected.ItemDataSO != null)
                {
                    Debug.Log("Drink bottle input received" + inventoryCellSelected.ItemDataSO.ItemData.itemType + inventoryCellSelected.ItemsCount);
                    if (inventoryCellSelected.ItemsCount > 0 && inventoryCellSelected.ItemDataSO.ItemData.itemType == ItemType.Consumable)
                    {
                        Debug.Log("Drink bottle input received");
                        if (isDrinking) return;
                        isDrinking = true;
                        thirdPersonController.CanMove = false;
                        animator.SetBool("DrinkBottle", true);
                        Invoke("EndDrink", drinkDuration);
                    }
                }
            }
        }

        private void EndDrink()
        {
            isDrinking = false;
            animator.SetBool("DrinkBottle", false);
            thirdPersonController.CanMove = true;
        }
    }
}
