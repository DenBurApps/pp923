using TMPro;
using UnityEngine;
using DG.Tweening;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsCanvas;
    [SerializeField] private GameObject _privacyCanvas;
    [SerializeField] private GameObject _termsCanvas;
    [SerializeField] private GameObject _contactCanvas;
    [SerializeField] private GameObject _versionCanvas;
    [SerializeField] private TMP_Text _versionText;
    private string _version = "Application version:\n";

    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private Ease _openEase = Ease.OutBack;
    [SerializeField] private Ease _closeEase = Ease.InBack;

    private void Awake()
    {
        _settingsCanvas.SetActive(false);
        _privacyCanvas.SetActive(false);
        _termsCanvas.SetActive(false);
        _contactCanvas.SetActive(false);
        _versionCanvas.SetActive(false);
        SetVersion();
    }

    private void SetVersion()
    {
        _versionText.text = _version + Application.version;
    }

    public void ShowSettings()
    {
        OpenWindow(_settingsCanvas);
    }

    public void HideSettings()
    {
        CloseWindow(_settingsCanvas);
    }

    public void ShowPrivacy()
    {
        OpenWindow(_privacyCanvas);
    }

    public void HidePrivacy()
    {
        CloseWindow(_privacyCanvas);
    }

    public void ShowTerms()
    {
        OpenWindow(_termsCanvas);
    }

    public void HideTerms()
    {
        CloseWindow(_termsCanvas);
    }

    public void ShowContact()
    {
        OpenWindow(_contactCanvas);
    }

    public void HideContact()
    {
        CloseWindow(_contactCanvas);
    }

    public void ShowVersion()
    {
        OpenWindow(_versionCanvas);
    }

    public void HideVersion()
    {
        CloseWindow(_versionCanvas);
    }

    private void OpenWindow(GameObject window)
    {
        window.SetActive(true);
        RectTransform rect = window.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
        {
            canvasGroup = window.AddComponent<CanvasGroup>();
        }
        
        rect.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        
        Sequence openSequence = DOTween.Sequence();
        openSequence.Append(rect.DOScale(1f, _animationDuration).SetEase(_openEase));
        openSequence.Join(canvasGroup.DOFade(1f, _animationDuration * 0.7f));
    }

    private void CloseWindow(GameObject window)
    {
        RectTransform rect = window.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
        {
            canvasGroup = window.AddComponent<CanvasGroup>();
        }
        
        Sequence closeSequence = DOTween.Sequence();
        closeSequence.Append(rect.DOScale(0f, _animationDuration).SetEase(_closeEase));
        closeSequence.Join(canvasGroup.DOFade(0f, _animationDuration * 0.7f));
        
        closeSequence.OnComplete(() => window.SetActive(false));
    }

    public void RateUs()
    {
#if UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}