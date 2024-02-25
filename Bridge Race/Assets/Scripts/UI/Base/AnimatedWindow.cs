using UnityEngine;

namespace UI.Base
{
    public class AnimatedWindow : MonoBehaviour
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;
        
        [SerializeField] private LeanTweenType _easing = LeanTweenType.easeOutBack;
        
        [SerializeField] private float _movementDuration = 0.6f;
        
        private void OnEnable()
        {
            DoAnimation();
        }

        private void DoAnimation()
        {
            transform.localPosition = _startPoint.localPosition;
            
            LeanTween.moveLocal(gameObject, _endPoint.localPosition, _movementDuration)
                .setEase(_easing);
        }
    }
}
