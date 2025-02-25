using System.Collections;
using RecordSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PathTracker
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Image[] _livesImages;
        [SerializeField] private ScreenVisabilityHandler _gameScreen;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private PlayerCircle _playerCircle;
        [SerializeField] private HealObject _healObjectPrefab;
        [SerializeField] private TargetCircle _targetCircle;
        [SerializeField] private YellowDot _yellowDot;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private RulesScreen _rulesScreen;
        [SerializeField] private FailScreen _failScreen;
        [SerializeField] private GameType _gameType;

        [Header("Heal Object Settings")]
        [SerializeField] private Vector2 _spawnBoundsMin;
        [SerializeField] private Vector2 _spawnBoundsMax;
        [SerializeField] private float _minSpawnInterval = 5f;
        [SerializeField] private float _maxSpawnInterval = 10f;

        private int _score;
        private int _lives;
        private Coroutine _healObjectSpawner;

        private void OnEnable()
        {
            _rulesScreen.StartClicked += StartNewGame;
            _pauseButton.onClick.AddListener(PauseGame);
            _pauseScreen.RestartClicked += RestartGame;
            _pauseScreen.ContinueClicked += ContinueGame;
            _failScreen.RestartClicked += RestartGame;
            _playerCircle.Missed += DecreaseLife;
            _playerCircle.GotHealth += IncreaseLife;
            _playerCircle.GotCenter += IncreaseScore;
        }

        private void OnDisable()
        {
            _rulesScreen.StartClicked -= StartNewGame;
            _pauseButton.onClick.RemoveListener(PauseGame);
            _pauseScreen.RestartClicked -= RestartGame;
            _pauseScreen.ContinueClicked -= ContinueGame;
            _failScreen.RestartClicked -= RestartGame;
            _playerCircle.Missed -= DecreaseLife;
            _playerCircle.GotHealth -= IncreaseLife;
            _playerCircle.GotCenter -= IncreaseScore;
        }

        private void Start()
        {
            _gameScreen.DisableScreen();
            ResetValues();
            _playerCircle.gameObject.SetActive(false);
            _targetCircle.gameObject.SetActive(false);
        }

        private void StartNewGame()
        {
            _playerCircle.gameObject.SetActive(true);
            _targetCircle.gameObject.SetActive(true);
            _gameScreen.EnableScreen();
            _playerCircle.EnableInput();
            _healObjectSpawner = StartCoroutine(SpawnHealObjects());
        }

        private void RestartGame()
        {
            ResetValues();
            StartNewGame();
        }

        private void PauseGame()
        {
            _pauseScreen.EnableScreen(_score);
            _playerCircle.DisableInput();
            _playerCircle.StopMovement();
            StopCoroutine(_healObjectSpawner);
        }

        private void ContinueGame()
        {
            _playerCircle.EnableInput();
            _playerCircle.ResumeMovement();
            _healObjectSpawner = StartCoroutine(SpawnHealObjects());
        }

        private void ResetValues()
        {
            _score = 0;
            _lives = 3;
            UpdateUIElements();
            UpdateLivesImages();
            if (_healObjectSpawner != null)
            {
                StopCoroutine(_healObjectSpawner);
            }
        }

        private void DecreaseLife()
        {
            _lives--;
            if (_lives <= 0)
            {
                ProcessGameOver();
            }
            else
            {
                UpdateLivesImages();
            }
        }

        private void IncreaseScore()
        {
            _score++;
            UpdateUIElements();
            _targetCircle.TeleportToRandomPosition();

            if (_score % 5 == 0)
            {
                _yellowDot.Enable();
            }
        }

        private void IncreaseLife()
        {
            _lives += 1;

            if (_lives > 3)
                _lives = 3;
            
            UpdateLivesImages();
        }

        private void ProcessGameOver()
        {
            var score = RecordHolder.GetRecordByType(_gameType);

            if (_score > score)
            {
                RecordHolder.AddNewRecord(_gameType, _score);
            }

            _playerCircle.DisableInput();
            _playerCircle.gameObject.SetActive(false);
            _targetCircle.gameObject.SetActive(false);
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

        private IEnumerator SpawnHealObjects()
        {
            while (true)
            {
                float interval = Random.Range(_minSpawnInterval, _maxSpawnInterval);
                yield return new WaitForSeconds(interval);

                Vector2 spawnPosition = new Vector2(
                    Random.Range(_spawnBoundsMin.x, _spawnBoundsMax.x),
                    Random.Range(_spawnBoundsMin.y, _spawnBoundsMax.y)
                );

                Instantiate(_healObjectPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
