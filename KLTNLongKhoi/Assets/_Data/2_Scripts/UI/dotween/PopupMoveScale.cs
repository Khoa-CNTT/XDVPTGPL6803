using UnityEngine;
using DG.Tweening;

public class PopupMoveScale : MonoBehaviour
{
    [SerializeField] private Transform hiddenPoint;
    [SerializeField] private Transform shownPoint;

    public void MoveAndScaleUp()
    {
        transform.localScale = Vector3.zero;
        transform.position = hiddenPoint.position; // Sử dụng điểm ẩn
        transform.DOMove(shownPoint.position, 0.5f).SetEase(Ease.OutBack); // Sử dụng điểm hiện
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    [ContextMenu("MoveAndScaleDown")]
    public void MoveAndScaleDown()
    {
        transform.DOMove(hiddenPoint.position, 0.5f).SetEase(Ease.InBack);
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }

}