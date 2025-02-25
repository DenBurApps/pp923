using UnityEngine;
using Random = UnityEngine.Random;

namespace BalloonTap
{
    public class SpawnArea : MonoBehaviour
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;
        
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

        public float GetRandomXPosition()
        {
            return Random.Range(_minX, _maxX);
        }
        
        public float GetRandomYPosition()
        {
            return Random.Range(_minY, _maxY);
        }
    }
}
