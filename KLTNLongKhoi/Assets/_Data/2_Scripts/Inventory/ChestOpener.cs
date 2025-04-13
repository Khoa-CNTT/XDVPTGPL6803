using Parity.SFInventory2.Core;
using UnityEngine;

namespace KLTNLongKhoi
{
    public class ChestOpener : MonoBehaviour
    {
        [SerializeField] StorageController storageController;

        public void OpenChest(Chest chest)
        {
            if (storageController.CurrentChest == chest)
            {
                storageController.CloseStorage();
            }
            else
            {
                chest.ProceedStorage(storageController);
            }
        }
    }
}
