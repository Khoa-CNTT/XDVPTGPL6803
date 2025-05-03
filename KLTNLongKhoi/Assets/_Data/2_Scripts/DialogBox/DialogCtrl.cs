using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KLTNLongKhoi
{
    public class DialogCtrl : MonoBehaviour
    {
        [SerializeField] DialogBox dialogBox;

        private PauseManager pauseManager;
        private NPC npc;

        private void Start()
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        /// <summary> Phương thức để mở hộp thoại với dialogLines </summary>
        public void OpenDialogBox(NPC npc)
        {
            this.npc = npc;
            dialogBox.gameObject.SetActive(true);
            dialogBox.SetDialogLines(npc.DialogContent);
            dialogBox.CheckPoint = npc.CheckPoint; 
            pauseManager.PauseGame();
        }

        public void CloseDialogBox()
        {   
            Debug.Log("Close Dialog Box");
            pauseManager.ResumeGame();
            npc.OnCloseDialog();
        }
    }
}