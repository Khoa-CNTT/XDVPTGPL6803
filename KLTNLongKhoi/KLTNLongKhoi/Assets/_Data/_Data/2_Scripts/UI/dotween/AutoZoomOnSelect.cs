using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class AutoZoomOnSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float zoomScale = 1.1f;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Ease easeType = Ease.OutBack;
    
    private Vector3 originalScale;
    private Tweener currentTween;
    
    void Start()
    {
        originalScale = transform.localScale;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hủy tween hiện tại nếu có
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        
        // Phóng to khi rê chuột vào
        currentTween = transform.DOScale(originalScale * zoomScale, animationDuration)
            .SetEase(easeType);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        // Hủy tween hiện tại nếu có
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        
        // Trở về kích thước ban đầu khi rê chuột ra
        currentTween = transform.DOScale(originalScale, animationDuration)
            .SetEase(Ease.OutQuad);
    }
}