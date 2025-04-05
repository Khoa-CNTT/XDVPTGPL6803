using DG.Tweening;
using UnityEngine;

public class PopupScale : MonoBehaviour
{
    public void ScaleUp()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        // Thực hiện hiệu ứng phóng to đối tượng từ kích thước ban đầu (Vector3.zero) đến kích thước đầy đủ (Vector3.one) trong 0.5 giây
        // Sử dụng easing OutBack để tạo hiệu ứng nảy nhẹ khi phóng to
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    [ContextMenu("ScaleDown")]
    public void ScaleDown()
    {
        // Thực hiện hiệu ứng thu nhỏ đối tượng từ kích thước đầy đủ (Vector3.one) đến kích thước ban đầu (Vector3.zero) trong 0.5 giây
        // Sử dụng easing InBack để tạo hiệu ứng nảy nhẹ khi thu nhỏ
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }
}

