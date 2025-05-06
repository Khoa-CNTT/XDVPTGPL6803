using UnityEngine;
using StarterAssets;
using System;
using System.Collections;

namespace KLTNLongKhoi
{
    public class PlayerUseHeal : MonoBehaviour
    {
        [Header("Sound Effects")] 
        [SerializeField] private AudioClip useItemStartSound; // Sound when starting to use the item
        [SerializeField] private AudioClip useItemEndSound;   // Sound when the item effect is applied
        [Header("Item Usage Settings")]
        [SerializeField] private bool isUsingHealItem;
        [SerializeField] private float useItemCooldown = 0.5f;
        [SerializeField] private float useItemDuration = 2f; // How long the item usage animation/effect lasts
        [SerializeField] private string useItemAnimationName = "UseItem"; // Name of the animation trigger
        [SerializeField] private int healAmount = 20; // Amount to heal the player
        ThirdPersonController thirdPersonController;
        StarterAssetsInputs starterAssetsInputs;
        QuickSlotsController quickSlotsController;
        InventoryController inventoryController;
        Animator animator;
        PlayerStatus playerStatus; // Reference to the PlayerStatus

        private bool canUseItem = true;

        private void Awake()
        {
            thirdPersonController = FindFirstObjectByType<ThirdPersonController>();
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
            quickSlotsController = FindFirstObjectByType<QuickSlotsController>();
            inventoryController = FindFirstObjectByType<InventoryController>();
            animator = GetComponent<Animator>();
            playerStatus = GetComponent<PlayerStatus>();
        }

        private void Start()
        {
            starterAssetsInputs.UseItem += OnUseItemInput;
        }

        private void OnDestroy()
        {
            starterAssetsInputs.UseItem -= OnUseItemInput;
        }

        private void OnUseItemInput()
        {
            UseItem();
        }

        private void UseItem()
        {
            if (!canUseItem) return;

            if (quickSlotsController.InventoryCellSelected != null)
            {
                if (quickSlotsController.InventoryCellSelected.ItemDataSO != null)
                {
                    if (quickSlotsController.InventoryCellSelected.ItemsCount > 0 && quickSlotsController.InventoryCellSelected.ItemDataSO.itemData.itemType == ItemType.Consumable)
                    {
                        StartCoroutine(UseItemCoroutine());
                    }
                }
            }
            else
            {
                Debug.Log("No item selected");
            }
        }

        private IEnumerator UseItemCoroutine()
        {
            canUseItem = false;
            thirdPersonController.CanMove = false;

            Debug.Log("Use Item: " + quickSlotsController.InventoryCellSelected.ItemDataSO.itemData.name);
            //ItemDataSO itemToRemove = quickSlotsController.InventoryCellSelected.ItemDataSO;
            inventoryController.RemoveInventoryCell(quickSlotsController.InventoryCellSelected.ItemDataSO, 1, out int countLeft);

            // Trigger the animation
            animator.SetTrigger(useItemAnimationName);

            // Play the start sound effect
            if (useItemStartSound != null)
            {
                AudioSource.PlayClipAtPoint(useItemStartSound, transform.position);
            }

            // Simulate item use duration (like a roll animation)
            yield return new WaitForSeconds(useItemDuration);

            // Apply the heal effect
            if (playerStatus != null)
            {
                playerStatus.RestoreHealth(healAmount);

                // Play the end sound effect
                if (useItemEndSound != null)
                {
                    AudioSource.PlayClipAtPoint(useItemEndSound, transform.position);
                }
            }
            else
            {
                Debug.LogError("PlayerStatus not found on player!");
            }

            thirdPersonController.CanMove = true;
            yield return new WaitForSeconds(useItemCooldown);
            canUseItem = true;
        }
    }
}