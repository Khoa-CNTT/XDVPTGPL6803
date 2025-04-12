using Parity.SFInventory2.Core;
using UnityEngine;

namespace Parity.SFInventory2.Testing
{
    //just an example of how you can open and close storage
    public class ChestOpener : MonoBehaviour
    {
        [SerializeField] private StorageController _storageController;

        [SerializeField] private Chest chest1;
        [SerializeField] private Chest chest2;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (_storageController.CurrentChest == chest1)
                {
                    _storageController.CloseStorage();
                }
                else
                {
                    chest1.ProceedStorage(_storageController);
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (_storageController.CurrentChest == chest2)
                {
                    _storageController.CloseStorage();
                }
                else
                {
                    chest2.ProceedStorage(_storageController);
                }
            }
        }
    }
}
