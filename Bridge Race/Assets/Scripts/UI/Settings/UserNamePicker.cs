using System.Collections;
using System.Text.RegularExpressions;
using Architecture.Services.Interfaces;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Settings
{
    public class UserNamePicker : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        [SerializeField] private Button _saveButton;
        
        private IUserDataStorage _userDataStorage;
        private IAudioService _audioService;
        
        [Inject]
        public void Construct(IUserDataStorage userDataStorage, IAudioService audioService)
        {
            _userDataStorage = userDataStorage;
            _audioService = audioService;
        }

        private void OnEnable()
        {
            _inputField.onValidateInput += ValidateInput;
            _saveButton.onClick.AddListener(SaveName);
            
            _audioService.PlaySfx(SfxType.UIClick);
            
            WriteCurrentNameToTextField();
            
            StartCoroutine(ActivateInput());
        }

        private void OnDisable()
        {
            _saveButton.onClick.RemoveListener(SaveName);
            
            _inputField.onValidateInput -= ValidateInput;
        }

        private char ValidateInput(string input, int charIndex, char addedChar)
        {
            if (Regex.IsMatch(addedChar.ToString(), "[^a-zA-Z0-9]"))
                addedChar = '\0';

            return addedChar;
        }

        private void SaveName()
        {
            _audioService.PlaySfx(SfxType.UIClick);
            
            _userDataStorage.SetUserName(_inputField.text);
            
            Destroy(gameObject);
        }

        private IEnumerator ActivateInput()
        {
            yield return null;
            
            _inputField.ActivateInputField();
        }

        private void WriteCurrentNameToTextField()
        {
            _inputField.text = _userDataStorage.UserName;
        }
    }
}