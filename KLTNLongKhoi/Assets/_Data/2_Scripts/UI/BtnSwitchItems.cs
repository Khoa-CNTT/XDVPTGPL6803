using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

// Di chuyển các slot sang trái/phải khi item quá nhiều
public class BtnSwitchItems : MonoBehaviour
{
    [SerializeField] private RectTransform _slotParent; // Slot Parent
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private float _moveDistance = 100f; // Khoảng cách di chuyển giữa các slot
    [SerializeField] private int _currentSlotIndex = 0; // Chỉ số slot hiện tại
    [SerializeField] private int _maxSlotIndex = 5; // Số lượng slot tối đa
    [SerializeField] private int _minSlotIndex = 0; // Số lượng slot tối thiểu
    [SerializeField] private float _animationDuration = 0.3f; // Thời gian hoàn thành animation
    [SerializeField] private Ease _easeType = Ease.OutQuad; // Kiểu ease cho animation
    
    private Vector2 _initialPosition; // Vị trí ban đầu của slot parent
    private Tweener _currentTween; // Tween hiện tại

    private void Start()
    {
        _leftButton.onClick.AddListener(MoveToPreviousSlot);
        _rightButton.onClick.AddListener(MoveToNextSlot);
        _initialPosition = _slotParent.anchoredPosition;
        UpdateButtonsInteractable();
    }

    public void MoveToNextSlot()
    {
        if (_currentSlotIndex >= _maxSlotIndex) return;
        
        _currentSlotIndex++;
        MoveToCurrentIndex();
        UpdateButtonsInteractable();
    }

    public void MoveToPreviousSlot()
    {
        if (_currentSlotIndex <= _minSlotIndex) return;
        
        _currentSlotIndex--;
        MoveToCurrentIndex();
        UpdateButtonsInteractable();
    }
    
    private void MoveToCurrentIndex()
    {
        // Hủy animation hiện tại nếu có
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
        }
        
        // Tính toán vị trí mới dựa trên index hiện tại
        Vector2 targetPosition = _initialPosition + new Vector2(-_moveDistance * _currentSlotIndex, 0f);
        
        // Tạo animation mới
        _currentTween = _slotParent.DOAnchorPos(targetPosition, _animationDuration)
            .SetEase(_easeType);
    }
    
    private void UpdateButtonsInteractable()
    {
        _leftButton.interactable = _currentSlotIndex > _minSlotIndex;
        _rightButton.interactable = _currentSlotIndex < _maxSlotIndex;
    }

    /// <summary>
    /// Reset lại vị trí ban đầu của các slot
    /// </summary>
    public void ResetPosition()
    {
        // Hủy animation hiện tại nếu có
        if (_currentTween != null && _currentTween.IsActive())
        {
            _currentTween.Kill();
        }
        
        // Reset index về 0
        _currentSlotIndex = 0;
        
        // Di chuyển về vị trí ban đầu
        _currentTween = _slotParent.DOAnchorPos(_initialPosition, _animationDuration)
            .SetEase(_easeType);
            
        // Cập nhật trạng thái các nút
        UpdateButtonsInteractable();
    }
}
