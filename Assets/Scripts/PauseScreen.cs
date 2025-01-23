using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private TMP_Text _scoreText;

    private string _text = "Score: ";
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action RestartClicked;
    public event Action ContinueClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartClicked);
        _homeButton.onClick.AddListener(OnHomeClicked);
        _continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _homeButton.onClick.RemoveListener(OnHomeClicked);
        _continueButton.onClick.RemoveListener(OnContinueClicked);
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void EnableScreen(int score)
    {
        _screenVisabilityHandler.EnableScreen();
        _scoreText.text = _text + score;
    }

    private void OnHomeClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnRestartClicked()
    {
        RestartClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
    
    private void OnContinueClicked()
    {
        ContinueClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }
}
