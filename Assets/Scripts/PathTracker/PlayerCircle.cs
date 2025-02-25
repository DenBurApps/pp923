using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PathTracker
{
    public class PlayerCircle : MonoBehaviour
    {
        [SerializeField] private Vector2 _initialVelocity = new Vector2(5f, 5f);
        [SerializeField] private float _centerThreshold = 1f;
        [SerializeField] private Transform _targetCircle;
        [SerializeField] private Vector2 _minBounds;
        [SerializeField] private Vector2 _maxBounds;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _minVelocityThreshold = 0.5f;
        [SerializeField] private LayerMask _healObjectLayer;
        [SerializeField] private float _healObjectThreshold = 1.5f;
        [SerializeField] private Rect _inputZone;

        private Rigidbody2D _rb;
        private bool _isInputEnabled = true;
        private Vector2 _savedVelocity = Vector2.zero;

        public event Action GotCenter;
        public event Action Missed;
        public event Action GotHealth;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = _initialVelocity.normalized * _speed;
            
            _inputZone = new Rect(_minBounds.x, _minBounds.y, _maxBounds.x - _minBounds.x, _maxBounds.y - _minBounds.y);
        }

        private void Update()
        {
            if (_isInputEnabled)
            {
                CheckBounds();
                HandleTouchInput();
            }
        }

        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    
                    if (!_inputZone.Contains(touchPosition))
                        return;

                    Vector2 playerCirclePosition = transform.position;
                    float distanceToTargetCircle = Vector2.Distance(playerCirclePosition, _targetCircle.position);

                    if (distanceToTargetCircle <= _centerThreshold)
                    {
                        GotCenter?.Invoke();
                    }
                    else
                    {
                        Missed?.Invoke();
                    }

                    Collider2D healObject =
                        Physics2D.OverlapCircle(touchPosition, _healObjectThreshold, _healObjectLayer);

                    if (healObject != null)
                    {
                        GotHealth?.Invoke();
                        Destroy(healObject.gameObject);
                    }
                }
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector2 normal = collision.contacts[0].normal;
            _rb.velocity = Vector2.Reflect(_rb.velocity, normal).normalized * _speed;

            _rb.velocity = ApplyRandomAngle(_rb.velocity);
        }

        private void CheckBounds()
        {
            Vector3 ballPosition = transform.position;

            if (ballPosition.x < _minBounds.x || ballPosition.x > _maxBounds.x)
            {
                _rb.velocity = new Vector2(-_rb.velocity.x, _rb.velocity.y);
                ballPosition.x = Mathf.Clamp(ballPosition.x, _minBounds.x, _maxBounds.x);
            }

            if (ballPosition.y < _minBounds.y || ballPosition.y > _maxBounds.y)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_rb.velocity.y);
                ballPosition.y = Mathf.Clamp(ballPosition.y, _minBounds.y, _maxBounds.y);
            }

            CorrectVelocity();

            transform.position = ballPosition;
        }

        private void CorrectVelocity()
        {
            Vector2 velocity = _rb.velocity;

            if (Mathf.Abs(velocity.x) < _minVelocityThreshold)
            {
                velocity.x = Mathf.Sign(velocity.x) * _minVelocityThreshold;
            }

            if (Mathf.Abs(velocity.y) < _minVelocityThreshold)
            {
                velocity.y = Mathf.Sign(velocity.y) * _minVelocityThreshold;
            }

            _rb.velocity = velocity.normalized * _speed;
        }

        private Vector2 ApplyRandomAngle(Vector2 direction)
        {
            float randomAngle = Random.Range(-5f, 5f);
            float angleInRadians = randomAngle * Mathf.Deg2Rad;

            float cos = Mathf.Cos(angleInRadians);
            float sin = Mathf.Sin(angleInRadians);

            float newX = direction.x * cos - direction.y * sin;
            float newY = direction.x * sin + direction.y * cos;

            return new Vector2(newX, newY).normalized * _speed;
        }

        public void EnableInput()
        {
            _isInputEnabled = true;
        }

        public void DisableInput()
        {
            _isInputEnabled = false;
        }

        public void StopMovement()
        {
            _savedVelocity = _rb.velocity;
            _rb.velocity = Vector2.zero;
        }

        public void ResumeMovement()
        {
            if (_savedVelocity != Vector2.zero)
            {
                _rb.velocity = _savedVelocity;
                _savedVelocity = Vector2.zero;
            }
        }
    }
}