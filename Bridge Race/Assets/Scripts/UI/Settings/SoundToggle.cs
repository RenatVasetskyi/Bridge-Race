using Architecture.Services.Interfaces;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Settings
{
    public class SoundToggle : MonoBehaviour
    {
        [SerializeField] private SoundType _type;
        
        [SerializeField] private Button _button;

        [SerializeField] private Image _image;
        
        [SerializeField] private Sprite _enabledSprite;
        [SerializeField] private Sprite _disabledSprite;

        private IAudioService _audioService;
        
        [Inject]
        public void Construct(IAudioService audioService) =>
            _audioService = audioService;

        private void OnEnable()
        {
            SetState();
            
            _button.onClick.AddListener(ChangeState);
        }

        private void OnDisable() => 
            _button.onClick.RemoveListener(ChangeState);

        private void SetState()
        {
            switch (_type)
            {
                case SoundType.Music:
                    _image.sprite = _audioService.IsMusicOn ? _enabledSprite : _disabledSprite;
                    break;
                case SoundType.Sound:
                    _image.sprite = _audioService.IsSfxOn ? _enabledSprite : _disabledSprite;
                    break;
            }
        }

        private void ChangeState()
        {
            _audioService.PlaySfx(SfxType.UIClick);
            
            switch (_type)
            {
                case SoundType.Sound:
                    if (_audioService.IsSfxOn)
                    {
                        _audioService.ChangeVolume(_type, SoundVolumeType.Off);
                        _image.sprite = _disabledSprite;
                    }
                    else
                    {
                        _audioService.ChangeVolume(_type, SoundVolumeType.On);
                        _image.sprite = _enabledSprite;
                    }
                    break;
                case SoundType.Music:
                    if (_audioService.IsMusicOn)
                    {
                        _audioService.ChangeVolume(_type, SoundVolumeType.Off);
                        _image.sprite = _disabledSprite;
                    }
                    else
                    {
                        _audioService.ChangeVolume(_type, SoundVolumeType.On);
                        _image.sprite = _enabledSprite;
                    }
                    break;
            }
        }
    }
}