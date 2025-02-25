using System;
using System.Collections;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LightingReflex
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Image[] _livesImages;
        [SerializeField] private ScreenVisabilityHandler _gameScreen;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private LightDataHolder _lightDataHolder;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private RulesScreen _rulesScreen;
        [SerializeField] private FailScreen _failScreen;
        [SerializeField] private GameType _gameType;

        private int _score;
        private int _lives;
        private IEnumerator _disablingCoroutine;
        private const int _maxTimerValue = 3;
        private float _currentTimer;

        private void OnEnable()
        {
            _rulesScreen.StartClicked += StartNewGame;

            _pauseButton.onClick.AddListener(PauseGame);

            _pauseScreen.RestartClicked += RestartGame;
            _pauseScreen.ContinueClicked += ContinueGame;

            _failScreen.RestartClicked += RestartGame;

            _lightDataHolder.DataSelected += OnButtonPressed;
        }

        private void OnDisable()
        {
            _rulesScreen.StartClicked -= StartNewGame;

            _pauseButton.onClick.RemoveListener(PauseGame);

            _pauseScreen.RestartClicked -= RestartGame;
            _pauseScreen.ContinueClicked -= ContinueGame;

            _failScreen.RestartClicked -= RestartGame;

            _lightDataHolder.DataSelected -= OnButtonPressed;
        }

        private void Start()
        {
            _gameScreen.DisableScreen();
            ResetValues();
        }

        private void StartNewGame()
        {
            _gameScreen.EnableScreen();
            ResetTimer();
            StopDisabling();
            StartDisabling();
        }

        private void RestartGame()
        {
            ResetValues();
            StartNewGame();
        }

        private void StartDisabling()
        {
            if (_disablingCoroutine != null)
                return;

            _disablingCoroutine = DisablingCoroutine();
            StartCoroutine(_disablingCoroutine);
        }

        private void StopDisabling()
        {
            if (_disablingCoroutine == null)
                return;

            StopCoroutine(_disablingCoroutine);
            _disablingCoroutine = null;
        }

        private IEnumerator DisablingCoroutine()
        {
            _currentTimer = 0;
            _lightDataHolder.DisableAllObjects();

            yield return new WaitForSeconds(2);

            _lightDataHolder.AssignDatas();

            while (_currentTimer < _maxTimerValue && _lives > 0)
            {
                _currentTimer += Time.deltaTime;
                yield return null;
            }

            if (_currentTimer >= _maxTimerValue)
            {
                DecreaseLife();
            }
        }

        private void PauseGame()
        {
            _pauseScreen.EnableScreen(_score);
            StopDisabling();
        }

        private void ContinueGame()
        {
            ResetTimer();
            StartDisabling();
        }

        private void ResetValues()
        {
            _score = 0;
            _lives = 3;
            ResetTimer();
            UpdateUIElements();
            UpdateLivesImages();
        }

        private void ResetTimer()
        {
            _currentTimer = 0;
        }

        private void DecreaseLife()
        {
            StopDisabling();
            _lives--;
            if (_lives <= 0)
            {
                StartCoroutine(HandleGameOver());
            }
            else
            {
                UpdateLivesImages();
                StartDisabling();
            }
        }

        private IEnumerator HandleGameOver()
        {
            yield return new WaitForSeconds(1);
            ProcessGameEnd();
        }

        private void OnButtonPressed(LightData data)
        {
            if (data.LightType == LightType.Red)
            {
                StopDisabling();
                IncreaseScore();
                return;
            }

            DecreaseLife();
        }

        private void IncreaseScore()
        {
            _score++;
            UpdateUIElements();
            ResetTimer();
            StopDisabling();
            StartDisabling();
        }

        private void ProcessGameEnd()
        {
            var score = RecordHolder.GetRecordByType(_gameType);

            if (_score > score)
            {
                RecordHolder.AddNewRecord(_gameType, _score);
            }

            _lightDataHolder.DisableAllObjects();
            _failScreen.EnableScreen(_score, _gameType);
        }

        private void UpdateUIElements()
        {
            _scoreText.text = _score.ToString();
        }

        private void UpdateLivesImages()
        {
            for (int i = 0; i < _livesImages.Length; i++)
            {
                _livesImages[i].gameObject.SetActive(i < _lives);
            }
        }
    }
}
