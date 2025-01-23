using UnityEngine;

namespace BalloonTap
{
    public class PoolReturner : MonoBehaviour
    {
        [SerializeField] private BalloonSpawner _balloonSpawner;
        [SerializeField] private CloudSpawner _cloudSpawner;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent(out Balloon balloon))
            {
                _balloonSpawner.ReturnToPool(balloon);
            }
            else if (collider.TryGetComponent(out Cloud cloud))
            {
                _cloudSpawner.ReturnToPool(cloud);
            }
        }
    }
}