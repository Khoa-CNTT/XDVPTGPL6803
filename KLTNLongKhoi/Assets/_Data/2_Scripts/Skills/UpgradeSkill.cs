using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KLTNLongKhoi
{
    public class UpgradeSkill : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private OnTriggerThis panelSkill;
        private StarterAssetsInputs starterAssetsInputs;
        private PauseManager pauseManager;
        private bool isOpen = false;

        void Awake()
        {
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        void OnEnable()
        {
            starterAssetsInputs.SkillPanel += OnClick;
        }

        void OnDisable()
        {
            starterAssetsInputs.SkillPanel -= OnClick;
        }

        public void OnClick()
        {
            if (pauseManager.IsPaused && isOpen == false)
            {
                return;
            }

            Debug.Log("Open Skill Panel");
            isOpen = !isOpen;

            if (isOpen)
            {
                OpenStorage();
            }
            else
            {
                CloseStorage();
            }
        }

        public void OpenStorage()
        {
            panelSkill.ActiveObjects();
            pauseManager.PauseGame();
            isOpen = true;
        }

        public void CloseStorage()
        {
            panelSkill.UnActiveObjects();
            pauseManager.ResumeGame();
            isOpen = false;
        }
    }
}