using System;
using System.Collections;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ColorSwitch
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ObjectsHolder _objectsHolder;
        [SerializeField] private Color[] _possibleTextColors;
        [SerializeField] private ScreenVisabilityHandler _gameScreen;
        [SerializeField] private GameObject _timer;
        [SerializeField] private TMP_Text _typeToChoseText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private FailScreen _failScreen;
        [SerializeField] private RulesScreen _rulesScreen;
        [SerializeField] private GameType _gameType;

        private readonly float _probabilityChance = 0.5f;
        private readonly int _maxTimerValue = 5;
        private int _score;
        private float _currentTimer;
        private ColorType _colorTypeToClick;
        private FigureType _figureTypeToClick;
        private bool _figureSelected;

        private IEnumerator _timerCoroutine;

        private void Start()
        {
            _gameScreen.DisableScreen();
            ResetValues();
        }
        
        private void OnEnable()
        {
            _rulesScreen.StartClicked += StartNewGame;

            _pauseButton.onClick.AddListener(PauseGame);

            _pauseScreen.RestartClicked += RestartGame;
            _pauseScreen.ContinueClicked += ContinueGame;

            _failScreen.RestartClicked += RestartGame;

            _objectsHolder.DataSelected += ProcessTypeClicked;
        }

        private void OnDisable()
        {
            _rulesScreen.StartClicked -= StartNewGame;

            _pauseButton.onClick.RemoveListener(PauseGame);

            _pauseScreen.RestartClicked -= RestartGame;
            _pauseScreen.ContinueClicked -= ContinueGame;

            _failScreen.RestartClicked -= RestartGame;

            _objectsHolder.DataSelected -= ProcessTypeClicked;
        }

        private void StartNewGame()
        {
            StopTimer();
            _objectsHolder.DisableAllObjects();
            _gameScreen.EnableScreen();
            _objectsHolder.AssignAllObjects();
            GetRandomEnumValue();
            StartTimer();
        }

        private void RestartGame()
        {
            ResetValues();
            StartNewGame();
            UpdateUI();
        }

        private void GetRandomEnumValue()
        {
            bool useFigureType = Random.value > _probabilityChance;
            var random = Random.Range(0, _objectsHolder.SelectedDatas.Count);
            TypeData randomData = _objectsHolder.SelectedDatas[random];
            
            if (useFigureType)
            {
                _figureTypeToClick = randomData.FigureType;
                _typeToChoseText.text = _figureTypeToClick.ToString();
                _figureSelected = true;
            }
            else
            {
                _colorTypeToClick = randomData.ColorType;
                _typeToChoseText.text = _colorTypeToClick.ToString();
                _figureSelected = false;
            }

            var randomIndex = Random.Range(0, _possibleTextColors.Length);
            _typeToChoseText.color = _possibleTextColors[randomIndex];
        }

        private void PauseGame()
        {
            _pauseScreen.EnableScreen(_score);
            StopTimer();
        }

        private void ContinueGame()
        {
            StartTimer();
        }

        private void StartTimer()
        {
            _timer.SetActive(true);
            if (_timerCoroutine != null)
                return;

            _timerCoroutine = TimerCoroutine();
            StartCoroutine(_timerCoroutine);
        }

        private void StopTimer()
        {
            if (_timerCoroutine == null)
                return;

            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
            _timer.SetActive(false);
        }

        private IEnumerator TimerCoroutine()
        {
            while (_currentTimer < _maxTimerValue)
            {
                _currentTimer += Time.deltaTime;
                yield return null;
            }

            ProcessGameEnd();
        }

        private void ProcessGameEnd()
        {
            var score = RecordHolder.GetRecordByType(_gameType);

            if (_score > score)
                RecordHolder.AddNewRecord(_gameType, _score);

            StopTimer();
            _failScreen.EnableScreen(_score, _gameType);
        }

        private void ProcessTypeClicked(TypeData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            StopTimer();

            bool isCorrect = _figureSelected
                ? _figureTypeToClick == data.FigureType
                : _colorTypeToClick == data.ColorType;

            if (isCorrect)
            {
                ProcessGameWin();
            }
            else
            {
                ProcessGameEnd();
            }
        }

        private void ResetTargetValues()
        {
            _colorTypeToClick = default;
            _figureTypeToClick = default;
        }

        private void ProcessGameWin()
        {
            _score++;
            UpdateUI();
            _currentTimer = 0;
            ResetTargetValues();
            StartNewGame();
        }

        private void ResetValues()
        {
            _score = 0;
            _currentTimer = 0;
            ResetTargetValues();
            _timer.SetActive(false);
        }

        private void UpdateUI()
        {
            _scoreText.text = _score.ToString();
        }
    }
}