using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;

/// <summary> Sử dụng collider để phát hiện va chạm </summary>
public class CharacterVision : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] Transform rayStartPoint; // Biến công khai cho điểm bắt đầu của tia
    [SerializeField] LayerMask allowedLayers; // Biến cho phép cấu hình layer
    [SerializeField] List<string> allowedTags; // Danh sách các tag được phép
    [SerializeField] List<Transform> unobstructedHits; // Danh sách các đối tượng không bị cản trở
    [SerializeField] List<Transform> detectedObjects; // Danh sách các đối tượng được phát hiện theo layer và tag
    [SerializeField] List<Transform> allCollidingObjects; // Danh sách tất cả các đối tượng va chạm

    void Start()
    {
        unobstructedHits = new List<Transform>();
        detectedObjects = new List<Transform>();
        allCollidingObjects = new List<Transform>();
    }

    void Update()
    {
        UpdateUnobstructedHits();
    }

    void OnTriggerStay(Collider other)
    {
        if (!allCollidingObjects.Contains(other.transform))
        {
            allCollidingObjects.Add(other.transform);
        }

        if ((allowedLayers.value & (1 << other.gameObject.layer)) != 0 && allowedTags.Contains(other.tag))
        {
            if (!detectedObjects.Contains(other.transform))
            {
                detectedObjects.Add(other.transform);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (detectedObjects.Contains(other.transform))
        {
            detectedObjects.Remove(other.transform);
        }

        if (allCollidingObjects.Contains(other.transform))
        {
            allCollidingObjects.Remove(other.transform);
        }
    }

    void UpdateUnobstructedHits()
    {
        unobstructedHits.Clear(); // Xóa danh sách trước khi cập nhật

        foreach (var hit in detectedObjects)
        {
            if (hit != null)
            {
                if (IsUnobstructed(hit))
                {
                    unobstructedHits.Add(hit);
                }
            }
        }
    }

    bool IsUnobstructed(Transform hit)
    {
        Vector3 direction = hit.position - rayStartPoint.position;
        Ray ray = new Ray(rayStartPoint.position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, direction.magnitude))
        {
            if (hitInfo.transform.GetComponentInParent<ThirdPersonController>())
            {
                Debug.DrawLine(rayStartPoint.position, hit.position, Color.green);
                playerTransform = hitInfo.transform;
                return true;
            }
        }
        playerTransform = null;
        Debug.DrawLine(rayStartPoint.position, hitInfo.point, Color.red);
        return false;
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    public bool CanSeePlayer()
    {
        return playerTransform != null;
    }

    public Vector3 GetPlayerPosition()
    {
        return playerTransform.position;
    }
}

