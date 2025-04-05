using UnityEngine;
using DG.Tweening;

public class DialogActorPopupEffect : MonoBehaviour
{
    public ActorStats actorStats;
    private Animator animator;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowActorPopupAnimation()
    {
        SetAnimatorIsOpen(true);
        DotTweenPopup(true);
    }

    public void HideActorPopupAnimation()
    {
        SetAnimatorIsOpen(false);
        DotTweenPopup(false);
    }

    public void DotTweenPopup(bool show)
    {
        if (show)
        {
            transform.localScale = Vector3.zero;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetEase(Ease.OutBack);
            transform.DOLocalMoveZ(-1, 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBack);
            transform.DOLocalMoveZ(0, 0.5f).SetEase(Ease.InBack);
        }
    }

    public void SetAnimatorIsOpen(bool isOpen)
    {
        // animator.SetBool("IsOpen", isOpen);
    }
}