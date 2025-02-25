using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PathTracker
{
    public class YellowDot : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;

        public event Action Clicked;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }
        
        
        public void Enable()
        {
            SetRandomPosition();
            gameObject.SetActive(true);
            StartCoroutine(StartDisabling());
        }
        
        public void Disable()
        {
            gameObject.SetActive(false);
            StopCoroutine(StartDisabling());
        }
        
        private IEnumerator StartDisabling()
        {
            var interval = new WaitForSeconds(3);
            yield return interval;
            Disable();
        }
        
        private void SetRandomPosition()
        {
            float randomX = UnityEngine.Random.Range(_minBounds.x, _maxBounds.x);
            float randomY = UnityEngine.Random.Range(_minBounds.y, _maxBounds.y);
            transform.position = new Vector3(randomX, randomY, transform.position.z);
        }
        
        private void OnClicked()
        {
            Clicked?.Invoke();
        }
    }
}
