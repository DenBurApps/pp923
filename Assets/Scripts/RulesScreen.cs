using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class RulesScreen : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _homeButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action StartClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartClicked);
        _homeButton.onClick.AddListener(OnHomeClicked);
    }

    private void OnDisable()
    {
        _startButton.onClick.AddListener(OnStartClicked);
        _homeButton.onClick.RemoveListener(OnHomeClicked);
    }

    private void Start()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    private void OnStartClicked()
    {
        StartClicked?.Invoke();
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnHomeClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}
