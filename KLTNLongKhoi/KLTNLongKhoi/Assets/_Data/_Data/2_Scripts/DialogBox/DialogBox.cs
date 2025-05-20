using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;
using StarterAssets;
using Unity.VisualScripting;

namespace KLTNLongKhoi
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] bool autoPlayMode;
        [SerializeField] TextMeshProUGUI characterNameText;
        [SerializeField] TextMeshProUGUI dialogContentText;
        [SerializeField] List<AudioClip> typingSounds; // Danh sách các âm thanh để phát khi hiển thị từng ký tự
        [SerializeField] DialogContent dialogContent; // Tham chiếu đến nội dung hội thoại
        [SerializeField] float textSpeed = 0.05f; // Tốc độ hiển thị từng ký tự
        private Coroutine typingCoroutine;
        private bool isTyping = false;
        private int currentLineIndex = 0;
        private AudioSource audioSource;
        private DialogCtrl dialogCtrl;
        private Checkpoint checkPoint;
        private StarterAssetsInputs starterAssetsInputs;

        public bool AutoPlayMode { get => autoPlayMode; set => autoPlayMode = value; }
        public Checkpoint CheckPoint { get => checkPoint; set => checkPoint = value; }

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            dialogCtrl = FindFirstObjectByType<DialogCtrl>();
            starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
        } 

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShowNextLine();
            }
        }

        public void ToggleAutoPlayMode()
        {
            autoPlayMode = !autoPlayMode;
        }

        public void SetDialogLines(DialogContent dialogContent)
        {
            gameObject.SetActive(true);
            this.dialogContent = dialogContent; 
            ShowNextLine();
        }

        // Phương thức để hiển thị dòng hội thoại tiếp theo
        public void ShowNextLine()
        { 
            List<DialogLine> dialogLines = dialogContent.dialogLines;
            if (currentLineIndex < dialogLines.Count)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }

                characterNameText.text = dialogLines[currentLineIndex].actorName;
                typingCoroutine = StartCoroutine(TypeSentence(dialogLines[currentLineIndex].dialogText));
                gameObject.SetActive(true);
                currentLineIndex++;
            }
            else // Kết thúc hội thoại
            {
                CloseDialog();
            }
        }

        // Phương thức để bỏ qua hội thoại
        public void CloseDialog()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
            isTyping = false;
            currentLineIndex = 0;
            dialogCtrl.CloseDialogBox();
            CheckPoint?.SaveCheckPoint();
        }

        // Coroutine để hiển thị từng ký tự của nội dung hội thoại
        private IEnumerator TypeSentence(string sentence)
        {
            dialogContentText.text = "";
            isTyping = true;
            for (int i = 0; i < sentence.Length; i++)
            {
                dialogContentText.text += sentence[i];

                // Phát âm thanh ngẫu nhiên khi hiển thị từng ký tự
                if (typingSounds.Count > 0)
                {
                    AudioClip randomClip = typingSounds[Random.Range(0, typingSounds.Count)];
                    audioSource.PlayOneShot(randomClip);
                }

                yield return new WaitForSeconds(textSpeed);

                if (AutoPlayMode && i == sentence.Length - 1)
                {
                    yield return new WaitForSeconds(0.5f);
                    ShowNextLine();
                }
            }
            isTyping = false;
        }
    }
}