using UnityEngine;
using DG.Tweening;

public class StartMenuAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform header;
    [SerializeField] private Transform menuPanel;
    
    [SerializeField] private float headerFinalPosY = -256;
    private CanvasGroup _menuCanvasGroup;

    private const float AnimDelay = 1f;
    private const float HeaderMoveDuration = 1.5f;
    private const float MenuFadeDuration = 2f;
    private const float IntervalBetweenAnim = 0.5f;
    
    private void Start()
    {
        _menuCanvasGroup = menuPanel.GetComponent<CanvasGroup>();
        _menuCanvasGroup.alpha = 0;
        
        MenuAnim();
    }
    
    private void MenuAnim()
    {
        var sequence = DOTween.Sequence();

        sequence.AppendInterval(AnimDelay);
        sequence.Append(header.DOAnchorPosY(headerFinalPosY, HeaderMoveDuration).SetEase(Ease.OutCubic));
        sequence.AppendInterval(IntervalBetweenAnim);
        sequence.Append(_menuCanvasGroup.DOFade(1, MenuFadeDuration));
    }
}
