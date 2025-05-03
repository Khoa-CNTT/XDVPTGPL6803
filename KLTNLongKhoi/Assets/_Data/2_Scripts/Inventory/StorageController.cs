using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    //just an example of storage 
    public class StorageController : ContainerBase
    {
        [Header("StorageController")]
        public UnityEvent openPanel;
        public UnityEvent closePanel;
        private Chest currentChest;
        public Chest CurrentChest => currentChest;
        PauseManager pauseManager;
        protected override void Start()
        {
            base.Start();
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        public void OpenStorage(Chest chest)
        {
            openPanel.Invoke();
            pauseManager.PauseGame();
            Debug.Log("Open storage");
            SaveToChest();
            currentChest = chest;
            var cells = currentChest.GetCells();
            if (cells != null)
            {
                if (cells.Count > 0)
                {
                    for (int i = 0; i < inventoryCells.Count; i++)
                    {
                        inventoryCells[i].MigrateCell(cells[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < inventoryCells.Count; i++)
                    {
                        inventoryCells[i].SetInventoryItem(null);
                    }
                }
            }
            for (int i = 0; i < inventoryCells.Count; i++)
            {
                inventoryCells[i].UpdateCellUI();
            }
        }

        public void CloseStorage()
        {
            SaveToChest();
            currentChest = null;
            closePanel.Invoke();
            pauseManager.ResumeGame();
        }

        public void SaveToChest()
        {
            if (currentChest != null)
            {
                if (inventoryCells.Count > 0)
                {
                    currentChest.SaveItems(inventoryCells.Select(s => s.ItemDataSO).ToList());
                }
                else
                {
                    currentChest.SaveItems(new List<ItemDataSO>());
                }
            }
        }
    }
}
