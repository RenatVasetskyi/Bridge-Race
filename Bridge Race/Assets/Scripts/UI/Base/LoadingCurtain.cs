using UnityEngine;

namespace UI.Base
{
    public class LoadingCurtain : MonoBehaviour
    {
        private const int StartValue = 1;
        private const int EndValue = 0;
        
        private const float Duration = 1f;
        
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Show()
        {
            _canvasGroup.alpha = StartValue;
            
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _canvasGroup.alpha = StartValue;
            
            gameObject.SetActive(true);
            
            LeanTween.value(StartValue, EndValue, Duration)
                .setOnUpdate((value) => _canvasGroup.alpha = value)
                .setOnComplete(() => Destroy(gameObject));
        }
    }
}