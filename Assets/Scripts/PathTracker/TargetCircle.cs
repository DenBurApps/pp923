using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PathTracker
{
    public class TargetCircle : MonoBehaviour
    {
        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;

        public event Action CatchedPlayer;
        
        public void TeleportToRandomPosition()
        {
            float newX = Random.Range(_minBounds.x, _maxBounds.x);
            float newY = Random.Range(_minBounds.y, _maxBounds.y);
            transform.position = new Vector2(newX, newY);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerCircle circle))
            {
                CatchedPlayer?.Invoke();
            }
        }
    }
}
