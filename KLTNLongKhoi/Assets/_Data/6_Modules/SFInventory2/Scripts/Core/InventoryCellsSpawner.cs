using UnityEngine;

namespace Parity.SFInventory2.Core
{
    //just a cell spawner in any container.
    public class InventoryCellsSpawner : MonoBehaviour
    {
        [SerializeField] private InventoryCell _inventoryCellPrefab;
        [SerializeField] private RectTransform _spawnParent;
        [SerializeField] private ContainerBase _cellsContainer;
        [SerializeField] private int spawnCount = 6;

        private void Start()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var cell = Instantiate(_inventoryCellPrefab, _spawnParent);
                cell.Init(_cellsContainer);
                _cellsContainer.AddInventoryCell(cell);
            }
        }
    }
}