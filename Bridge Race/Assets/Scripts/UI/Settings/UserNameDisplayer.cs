using Architecture.Services.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Settings
{
    public class UserNameDisplayer : MonoBehaviour
    {   
        [SerializeField] private TextMeshProUGUI _userNameText;
        
        private IUserDataStorage _userDataStorage;
        
        [Inject]
        public void Construct(IUserDataStorage userDataStorage)
        {
            _userDataStorage = userDataStorage;
        }

        private void Awake()
        {
            UpdateNameText();
        }

        private void OnEnable()
        {
            _userDataStorage.OnNameChanged += UpdateNameText;
        }

        private void OnDisable()
        {
            _userDataStorage.OnNameChanged -= UpdateNameText;
        }

        private void UpdateNameText()
        {
            _userNameText.text = _userDataStorage.UserName;
        }
    }
}