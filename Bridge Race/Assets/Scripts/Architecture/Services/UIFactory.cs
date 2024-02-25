using System.Threading.Tasks;
using Architecture.Services.Interfaces;
using Data;
using UI.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Architecture.Services
{
    public class UIFactory : BaseFactory, IUIFactory
    {
        private readonly GameSettings _gameSettings;
        public LoadingCurtain LoadingCurtain { get; private set; }
        
        public UIFactory(DiContainer container, IAssetProvider assetProvider, GameSettings gameSettings)
            : base(container, assetProvider)
        {
            _gameSettings = gameSettings;
        }

        public async void CreateLoadingCurtain()
        {
            if (LoadingCurtain != null)
            {
                LoadingCurtain.Show();
                
                return;
            }
            
            LoadingCurtain = await CreateObjectForComponent<LoadingCurtain>
                (_gameSettings.Prefabs.LoadingCurtain, null);
            
            LoadingCurtain.Show();  
        }

        public async void CreateFullScreenWindow(AssetReferenceGameObject prefab, Transform parent)
        {
            RectTransform editNameWindow = await CreateObjectForComponent
                <RectTransform>(prefab, parent);
             
            ResizeRectTransformOnFullScreen(editNameWindow);
        }
        
        private async Task<T> CreateObjectForComponent<T>(AssetReferenceGameObject prefab,
            Transform parent) where T : Component
        {
            return (await CreateAddressableWithContainer(prefab, Vector3.zero,
                Quaternion.identity, parent)).GetComponent<T>();
        }

        private void ResizeRectTransformOnFullScreen(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            
            rectTransform.transform.localScale = Vector3.one;
            rectTransform.transform.localPosition = Vector3.zero;
        }
    }
}