using UnityEngine;
using Random = UnityEngine.Random;

namespace BalloonTap
{
    public class SpawnArea : MonoBehaviour
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        private float _yPosition;

        private void Awake()
        {
            _yPosition = transform.position.y;
        }

        public Vector2 GetRandomXPositionToSpawn()
        {
            float randomX = Random.Range(_minX, _maxX);
        
            return new Vector2(randomX, _yPosition);
        }
    }
}
