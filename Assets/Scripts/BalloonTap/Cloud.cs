using UnityEngine;

namespace BalloonTap
{
    public class Cloud : MonoBehaviour
    {
        [SerializeField] private CloudType _cloudType;
        [SerializeField] private MovingComponent _movingComponent;

        public CloudType CloudType => _cloudType;
        public MovingComponent MovingComponent => _movingComponent;
    }

    public enum CloudType
    {
        Bonus,
        Empty
    }
}