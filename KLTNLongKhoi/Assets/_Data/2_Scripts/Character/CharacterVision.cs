using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using System.Collections;

/// <summary> Sử dụng collider để phát hiện va chạm </summary>
public class CharacterVision : MonoBehaviour
{
    private Transform playerPoint;
    public float distanceToPlayer;
    public float maxDistance = 30f;
    public bool isSeePlayer;
    public Transform rayStartPoint;

    private void Start()
    {
        playerPoint = FindFirstObjectByType<ThirdPersonController>().playerPoint;
        
        // If rayStartPoint is not assigned, use this object's transform
        if (rayStartPoint == null)
        {
            rayStartPoint = transform;
            Debug.LogWarning("rayStartPoint not assigned on " + gameObject.name + ". Using this object's transform instead.", this);
        }
    }

    private void Update()
    {
        if (playerPoint == null) return;
        distanceToPlayer = Vector3.Distance(transform.position, playerPoint.transform.position); 
        isSeePlayer = IsUnobstructed(playerPoint.transform);
    }

    public Transform GetPlayer()
    {
        if (isSeePlayer && distanceToPlayer <= maxDistance) return playerPoint;
        return null;
    }

    bool IsUnobstructed(Transform hit)
    {
        if (rayStartPoint == null || hit == null) return false;
        
        Vector3 direction = hit.position - rayStartPoint.position;
        Ray ray = new Ray(rayStartPoint.position, direction);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, direction.magnitude))
        {
            if (hitInfo.transform.GetComponentInParent<ThirdPersonController>())
            {
                Debug.DrawLine(rayStartPoint.position, hit.position, Color.green);
                return true;
            }
        }
        Debug.DrawLine(rayStartPoint.position, hitInfo.point, Color.red);
        return false;
    }
}


