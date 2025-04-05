using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    [RequireComponent(typeof(Button))]
    public class BtnSwitch : MonoBehaviour
    {
        [Header("Active Object")]
        [SerializeField] bool isOpen = false;
        [SerializeField] List<GameObject> ListPanelTrigger;
        [SerializeField] bool usePopupScale;

        Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SwitchPanel();
        }

        private void SwitchPanel()
        {
            isOpen = !isOpen;

            foreach (GameObject panel in ListPanelTrigger)
            {
                if (panel != null)
                {
                    if (usePopupScale)
                    {
                        PopupScale(panel, isOpen);
                    }
                    else
                    {
                        panel.SetActive(isOpen); // Bật đối tượng
                    }
                }
            }
        }

        private void PopupScale(GameObject panel, bool isOn)
        {
            PopupScale popupScale = panel.GetComponent<PopupScale>();
            if (popupScale)
            {
                if (isOn) popupScale.ScaleUp();
                else popupScale.ScaleDown();
            }
        }
    }

}
