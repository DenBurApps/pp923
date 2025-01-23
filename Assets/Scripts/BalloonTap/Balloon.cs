using System.Collections;
using UnityEngine;

namespace BalloonTap
{
    public class Balloon : MonoBehaviour
    {
        [SerializeField] private BalloonType _balloonType;
        [SerializeField] private MovingComponent _movingComponent;
        
        public BalloonType Type => _balloonType;
        public MovingComponent MovingComponent => _movingComponent;
    }

    public enum BalloonType
    {
        Red,
        Purple,
        Yellow
    }
    
    
}
