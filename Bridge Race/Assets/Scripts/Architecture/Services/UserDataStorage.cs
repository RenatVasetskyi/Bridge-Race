using System;
using System.IO;
using Architecture.Services.Interfaces;
using Data;
using UnityEngine;

namespace Architecture.Services
{
    public class UserDataStorage : IUserDataStorage
    {
        private const string SaveNameKey = "UserName";
        private const string DefaultName = "User";

        private const string PhotoFromNativeGallery = "UserPhoto";
        private const string FileTypeFromNativeGallery = "Photo";
        
        private const string UserPhotoSaveId = "UsersSelectedPhoto";
        
        private readonly ISaveService _saveService;
        private readonly GameSettings _gameSettings;

        public event Action OnNameChanged;
        public event Action<string> OnPickUserPhotoFromGalleryError;
        public event Action<Sprite> OnUserPhotoPickedFromGallery;

        public string UserName { get; private set; }
        public Sprite UserPhoto { get; private set; }

        public UserDataStorage(ISaveService saveService, GameSettings gameSettings)
        {
            _saveService = saveService;
            _gameSettings = gameSettings;
        }

        public void SetUserName(string name)
        {
            UserName = name == string.Empty ? DefaultName : name;

            SaveName();
            
            OnNameChanged?.Invoke();
        }

        public void PickPhotoFromNativeGallery()
        {
            NativeGallery.Permission permissionToReadImage = NativeGallery.CheckPermission
                (NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);

            if (permissionToReadImage != NativeGallery.Permission.Granted)
            {
                NativeGallery.RequestPermission(NativeGallery.PermissionType.Read,
                    NativeGallery.MediaType.Image);
            }
            else
            {
                NativeGallery.GetImageFromGallery(CreateSpriteFromPhotoPath, 
                    PhotoFromNativeGallery, FileTypeFromNativeGallery);
            }
        }

        private void CreateSpriteFromPhotoPath(string path)
        {
            try
            {
                Texture2D texture = new Texture2D(1, 1);
                
                byte[] imageInBytes = File.ReadAllBytes(path);
                
                texture.LoadImage(imageInBytes); 

                CreateSpriteFromTexture(texture);
            }
            catch (Exception exception)
            {
                OnPickUserPhotoFromGalleryError?.Invoke(exception.Message);
            }
        }

        private void CreateSpriteFromTexture(Texture2D texture)
        {
            UserPhoto = Sprite.Create(texture, new Rect(0, 0, texture.width,
                texture.height), Vector2.one * 0.5f); // 0.5f means center of image

            SaveUserPhoto(UserPhoto);

            OnUserPhotoPickedFromGallery?.Invoke(UserPhoto);
        }

        public void Load()
        {
            UserName = _saveService.HasKey(SaveNameKey) ?
                _saveService.LoadString(SaveNameKey) : DefaultName;

            UserPhoto = _saveService.HasKey(UserPhotoSaveId) ? _saveService
                .LoadSprite(UserPhotoSaveId) : _gameSettings.DefaultUserPhoto;
        }   

        private void SaveName()
        {
            _saveService.SaveString(SaveNameKey, UserName);
        }

        private void SaveUserPhoto(Sprite sprite)
        {
            _saveService.SaveSprite(UserPhotoSaveId, sprite);
        }
    }
}