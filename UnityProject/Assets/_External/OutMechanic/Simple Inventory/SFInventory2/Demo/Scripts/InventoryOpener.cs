using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Parity.SFInventory2.Testing
{
    //smooth opening of inventory via CanvasGroup so that there is no flashbanging
    public class InventoryOpener : MonoBehaviour
    {
        [Range(0f, 1f)]
        [SerializeField] private float _openingTime = 0.1f;
        [SerializeField] private RectTransform _inventory;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _openAtStart;

        private bool _opened = false;
        private bool _isOpening = false;
        private void Start()
        {
            _opened = _openAtStart;
            _inventory.gameObject.SetActive(_openAtStart);
        }

        public UnityEvent<bool> onOpen;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OpenLogic();
            }
        }

        private void OpenLogic()
        {
            _opened = !_opened;
            onOpen?.Invoke(_opened);
            if (_openingTime > 0)
            {
                if (!_isOpening)
                    StartCoroutine(ProceedOpening());
            }
            else
            {
                _inventory.gameObject.SetActive(_opened);
            }
        }

        // coroutine that smoothly opens the inventory
        private IEnumerator ProceedOpening()
        {
            if (_opened)
                _inventory.gameObject.SetActive(true);

            _isOpening = true;
            while ((_canvasGroup.alpha > 0 && !_opened) || (_canvasGroup.alpha < 1 && _opened))
            {
                _canvasGroup.alpha += (_opened ? 1f : -1f) * (1f / _openingTime) * Time.deltaTime;
                yield return null;
            }
            _inventory.gameObject.SetActive(_opened);
            _isOpening = false;
        }
    }
}