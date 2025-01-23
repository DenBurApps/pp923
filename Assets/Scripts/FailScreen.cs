using System;
using System.Collections;
using System.Collections.Generic;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class FailScreen : MonoBehaviour
{
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _bestScoreText;

    private readonly string _text = "Score: ";
    private readonly string _bestScoretext = "Best: ";
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action RestartClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartClicked);
        _homeButton.onClick.AddListener(OnHomeClicked);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _homeButton.onClick.RemoveListener(OnHomeClicked);
    }

    private void Start()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void EnableScreen(int score, GameType gameType)
    {
        _screenVisabilityHandler.EnableScreen();
        _scoreText.text = _text + score;
        _bestScoreText.text = _bestScoretext + RecordHolder.GetRecordByType(gameType);
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
}
