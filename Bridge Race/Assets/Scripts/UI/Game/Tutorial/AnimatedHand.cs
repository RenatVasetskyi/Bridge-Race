using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace UI.Game.Tutorial
{
    public class AnimatedHand : MonoBehaviour
    {
        [SerializeField] private float _movementDuration;
        
        [SerializeField] private Transform[] _movePoints;

        [SerializeField] private Ease _easing;

        private Vector3[] _movementVectors;

        public void DoAnimation()
        {
            _movementVectors = _movePoints.Select(x => x.position).ToArray();
            
            Animate();
        }
        
        private void Animate()
        {
            transform.DOPath(_movementVectors, _movementDuration)
                .SetEase(_easing).onComplete += DoAnimation;
        }
    }
}