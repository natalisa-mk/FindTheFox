using UnityEngine;
using DG.Tweening;

public class StartMenuAnimation : MonoBehaviour
{
    [SerializeField] private CanvasGroup menuCanvasGroup;
    [SerializeField] private RectTransform headingRectTransform;

    [SerializeField] private float headerFinalPosY = -256;

    private const float AnimDelay = 1f;
    private const float HeadingMoveDuration = 1.5f;
    private const float MenuFadeDuration = 2f;
    private const float IntervalBetweenAnim = 0.5f;
    
    private void Start()
    {
        menuCanvasGroup.alpha = 0;
        
        PlayMenuAnim();
    }
    
    private void PlayMenuAnim()
    {
        var sequence = DOTween.Sequence();

        sequence.AppendInterval(AnimDelay);
        sequence.Append(headingRectTransform.DOAnchorPosY(headerFinalPosY, HeadingMoveDuration).SetEase(Ease.OutCubic));
        sequence.AppendInterval(IntervalBetweenAnim);
        sequence.Append(menuCanvasGroup.DOFade(1, MenuFadeDuration));
    }
}
