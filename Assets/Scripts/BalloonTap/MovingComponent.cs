using System.Collections;
using UnityEngine;

namespace BalloonTap
{
    public class MovingComponent : MonoBehaviour
    {
        [SerializeField] private float _speed = 0;
        
        private Transform _transform;
        private IEnumerator _movingCoroutine;
        
        private void Awake()
        {
            _transform = transform;
        }

        public void EnableMovement(Vector3 direction)
        {
            if (_movingCoroutine == null)
                _movingCoroutine = StartMoving(direction);

            StartCoroutine(_movingCoroutine);
        }
    
        public void DisableMovement()
        {
            if (_movingCoroutine != null)
            {
                StopCoroutine(_movingCoroutine);
                _movingCoroutine = null;
            }
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private IEnumerator StartMoving(Vector3 direction)
        {
            while (true)
            {
                _transform.position += direction * _speed * Time.deltaTime;
            
                yield return null;
            }
        }
    }
}
