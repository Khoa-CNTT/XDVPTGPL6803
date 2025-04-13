using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KLTNLongKhoi
{
    //here are the callbacks  
    public class CellsCallbacksController : MonoBehaviour
    {
        public Action<InventoryCell, PointerEventData> onBeginDrag;
        public Action<InventoryCell, PointerEventData> onDrag;
        public Action<InventoryCell, PointerEventData> onEndDrag;
        public Action<InventoryCell, PointerEventData> onDrop;
        public Action<InventoryCell, PointerEventData> onClick;
    }
}