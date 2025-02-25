using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainScreen
{
    [RequireComponent(typeof(ScreenVisabilityHandler))]
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private Settings _settings;
        [SerializeField] private Button _balloon, _colorSwitch, _reflex, _tracker, _settingsButton;
        [SerializeField] private Onboarding _onboarding;

        private ScreenVisabilityHandler _screenVisabilityHandler;

        private void Awake()
        {
            _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
        }

        private void OnEnable()
        {
            _balloon.onClick.AddListener(OnBalloonClicked);
            _colorSwitch.onClick.AddListener(OnColorSwitchClicked);
            _reflex.onClick.AddListener(OnReflexClicked);
            _tracker.onClick.AddListener(OnTrackerClicked);
            _settingsButton.onClick.AddListener(OnSettingsClicked);
            _onboarding.Shown += _screenVisabilityHandler.EnableScreen;
        }

        private void OnDisable()
        {
            _balloon.onClick.RemoveListener(OnBalloonClicked);
            _colorSwitch.onClick.RemoveListener(OnColorSwitchClicked);
            _reflex.onClick.RemoveListener(OnReflexClicked);
            _tracker.onClick.RemoveListener(OnTrackerClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsClicked);
            _onboarding.Shown -= _screenVisabilityHandler.EnableScreen;
        }

        private void OnBalloonClicked()
        {
            SceneManager.LoadScene("TapBalloonScene");
        }

        private void OnColorSwitchClicked()
        {
            SceneManager.LoadScene("ColorSwitchScene");
        }

        private void OnReflexClicked()
        {
            SceneManager.LoadScene("LightningReflexScene");
        }

        private void OnTrackerClicked()
        {
            SceneManager.LoadScene("PathTrackerScene");
        }

        private void OnSettingsClicked()
        {
            _settings.ShowSettings();
        }
    }
}
