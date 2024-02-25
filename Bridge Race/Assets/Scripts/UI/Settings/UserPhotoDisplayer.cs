using System;
using Architecture.Services.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Settings
{
    public class UserPhotoDisplayer : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        private IUserDataStorage _userDataStorage;
        
        [Inject]
        public void Construct(IUserDataStorage userDataStorage)
        {
            _userDataStorage = userDataStorage;
        }
        
        private void OnEnable()
        {
            SetPickedPhoto(_userDataStorage.UserPhoto);

            _userDataStorage.OnUserPhotoPickedFromGallery += SetPickedPhoto;
        }

        private void OnDisable()
        {
            _userDataStorage.OnUserPhotoPickedFromGallery -= SetPickedPhoto;
        }

        private void SetPickedPhoto(Sprite photo)
        {
            _image.sprite = photo;
        }
    }
}
