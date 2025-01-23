using System;
using RecordSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BalloonTap
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ScreenVisabilityHandler _gameScreen;
        [SerializeField] private BalloonSpawner _balloonSpawner;
        [SerializeField] private CloudSpawner _cloudSpawner;
        [SerializeField] private TouchInputHandler _touchInputHandler;
        [SerializeField] private Image[] _livesImages;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private RulesScreen _rulesScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private FailScreen _failScreen;
        [SerializeField] private GameObject _blowImage;
        [SerializeField] private GameType _gameType;

        private int _score;
        private int _lives;

        private void OnEnable()
        {
            _rulesScreen.StartClicked += StartNewGame;
            
            _pauseButton.onClick.AddListener(PauseGame);

            _pauseScreen.RestartClicked += StartNewGame;
            _pauseScreen.ContinueClicked += ContinueGame;

            _failScreen.RestartClicked += StartNewGame;

            _touchInputHandler.BalloonClicked += BalloonCatched;
            _touchInputHandler.CloudClicked += CloudCatched;
        }

        private void OnDisable()
        {
            _rulesScreen.StartClicked -= StartNewGame;
            
            _pauseButton.onClick.RemoveListener(PauseGame);

            _pauseScreen.RestartClicked -= StartNewGame;
            _pauseScreen.ContinueClicked -= ContinueGame;

            _failScreen.RestartClicked -= StartNewGame;

            _touchInputHandler.BalloonClicked -= BalloonCatched;
            _touchInputHandler.CloudClicked -= CloudCatched;
        }

        private void Start()
        {
            _gameScreen.DisableScreen();
        }

        private void StartNewGame()
        {
            _gameScreen.EnableScreen();
            ResetValues();
            _balloonSpawner.StartSpawn();
            _cloudSpawner.StartSpawn();
            _touchInputHandler.StartDetectingTouch();
        }

        private void ResetValues()
        {
            _score = 0;
            _lives = 3;
            UpdateUIElements();
            UpdateLivesImages();
            _balloonSpawner.ResetSpeed();
        }

        private void PauseGame()
        {
            _balloonSpawner.StopSpawn();
            _balloonSpawner.ReturnAllObjectsToPool();
            _touchInputHandler.StopDetectingTouch();
            _pauseScreen.EnableScreen(_score);
        }

        private void ContinueGame()
        {
            _balloonSpawner.StartSpawn();
            _touchInputHandler.StartDetectingTouch();
        }

        private void BalloonCatched(Balloon balloon)
        {
            if (balloon.Type == BalloonType.Red)
            {
                IncreaseScore(balloon);
                return;
            }
            
            DecreaseLife(balloon);
        }

        private void CloudCatched(Cloud cloud)
        {
            if(cloud.CloudType == CloudType.Bonus)
                IncreaseLife(cloud);
        }
        
        private void IncreaseScore(Balloon balloon)
        {
            _score += 10;
            UpdateUIElements();
            _balloonSpawner.ReturnToPool(balloon);
            _balloonSpawner.IncreaseSpeed();
        }

        private void DecreaseLife(Balloon balloon)
        {
            _lives--;
            UpdateLivesImages();
            
            var gameObject = Instantiate(_blowImage);
            gameObject.transform.position = balloon.transform.position;
            Destroy(gameObject, 2);
            _balloonSpawner.ReturnToPool(balloon);
            
            if (_lives <= 0)
            {
                ProcessGameEnd();
            }
        }

        private void IncreaseLife(Cloud cloud)
        {
            _lives++;
            if (_lives > 3)
                _lives = 3;
            
            UpdateLivesImages();
            _cloudSpawner.ReturnToPool(cloud);
        }

        private void ProcessGameEnd()
        {
            _balloonSpawner.ReturnAllObjectsToPool();
            _cloudSpawner.ReturnAllObjectsToPool();
            _touchInputHandler.StopDetectingTouch();
            _failScreen.EnableScreen(_score, _gameType);
        }
        
        private void UpdateUIElements()
        {
            _scoreText.text = _score.ToString();
        }

        private void UpdateLivesImages()
        {
            foreach (var image in _livesImages)
            {
                image.gameObject.SetActive(false);
            }

            for (int i = 0; i < _lives; i++)
            {
                if(i < _livesImages.Length)
                    _livesImages[i].gameObject.SetActive(true);
            }
        }
        
    }
}