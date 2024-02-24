using System;
using UnityEngine;

namespace Architecture.Services.Interfaces
{
    public interface IUserDataStorage
    {
        event Action OnNameChanged;
        event Action<string> OnPickUserPhotoFromGalleryError;
        event Action<Sprite> OnUserPhotoPickedFromGallery;
        string UserName { get; }
        Sprite UserPhoto { get; }
        void SetUserName(string name);
        void PickPhotoFromNativeGallery();
        void Load();
    }
}