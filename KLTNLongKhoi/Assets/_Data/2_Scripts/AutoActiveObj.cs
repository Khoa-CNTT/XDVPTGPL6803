using StarterAssets;
using UnityEngine;

namespace KLTNLongKhoi
{
    [ExecuteInEditMode]
    public class AutoActiveObj : MonoBehaviour
    {
        [SerializeField] private GameObject[] childObjects;
        [SerializeField] private float activationDistance = 30f;
        
        private Transform playerTransform;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Find all child objects if not set manually
            if (childObjects == null || childObjects.Length == 0)
            {
                RefreshChildObjects();
            }
            
            // Find player in game mode
            if (Application.isPlaying)
            {
                FindPlayer();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Application.isPlaying && playerTransform != null)
            {
                UpdateObjectsState();
            }
        }
        
        private void FindPlayer()
        {
            var playerController = FindFirstObjectByType<ThirdPersonController>();
            if (playerController != null)
            {
                playerTransform = playerController.transform;
            }
        }
        
        private void UpdateObjectsState()
        {
            if (childObjects == null) return;
            
            foreach (var childObj in childObjects)
            {
                if (childObj == null) continue;
                
                float distance = Vector3.Distance(childObj.transform.position, playerTransform.position);
                childObj.SetActive(distance <= activationDistance);
            }
        }
        
        public void RefreshChildObjects()
        {
            // Get all child objects
            int childCount = transform.childCount;
            childObjects = new GameObject[childCount];
            
            for (int i = 0; i < childCount; i++)
            {
                childObjects[i] = transform.GetChild(i).gameObject;
            }
        }
    }
}
