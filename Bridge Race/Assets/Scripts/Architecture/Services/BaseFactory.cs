using System.Threading.Tasks;
using Architecture.Services.Interfaces;
using UI.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Architecture.Services
{
    public class BaseFactory : IBaseFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;

        public LoadingCurtain LoadingCurtain { get; private set; }

        public BaseFactory(DiContainer container, IAssetProvider assetProvider)
        {
            _container = container;
            _assetProvider = assetProvider;
        }

        public T CreateBaseWithContainer<T>(string path) where T : Component
        {
            return _container.InstantiatePrefabForComponent<T>(_assetProvider.Initialize<T>(path));
        }

        public T CreateBaseWithContainer<T>(string path, Transform parent) where T : Component
        {
            return _container.InstantiatePrefabForComponent<T>(_assetProvider.Initialize<T>(path), parent);
        }

        public T CreateBaseWithContainer<T>(string path, Vector3 at, Quaternion rotation, Transform parent) where T : Component
        {
            return _container.InstantiatePrefabForComponent<T>(_assetProvider
                    .Initialize<T>(path), at, rotation, parent);
        }

        public T CreateBaseWithContainer<T>(T prefab, Vector3 at, Quaternion rotation, Transform parent) where T : Component
        {
            return _container.InstantiatePrefabForComponent<T>(prefab, at, rotation, parent);
        }

        public GameObject CreateBaseWithContainer(GameObject prefab, Vector3 at, Quaternion rotation, Transform parent)
        {
            return _container.InstantiatePrefab(prefab, at, rotation, parent);
        }

        public T CreateBaseWithObject<T>(string path) where T : Component
        {
            return Object.Instantiate(_assetProvider.Initialize<T>(path));
        }

        public GameObject CreateBaseWithContainer(string path, Transform parent)
        {
            return _container.InstantiatePrefab(_assetProvider.Initialize<GameObject>(path), parent);
        }
        
        public async Task<GameObject> CreateAddressableWithContainer
            (AssetReferenceGameObject assetReference, Vector3 at, Quaternion rotation, Transform parent)
        {
            GameObject loadedResource = await _assetProvider.Load<GameObject>(assetReference);

            return _container.InstantiatePrefab(loadedResource, at, rotation, parent);
        }

        public async Task<GameObject> CreateAddressableWithObject(AssetReferenceGameObject assetReference, Vector3 at, Quaternion rotation,
            Transform parent)
        {
            GameObject loadedResource = await _assetProvider.Load<GameObject>(assetReference);
            
            return Object.Instantiate(loadedResource, at, rotation, parent);
        }

        public async void CreateLoadingCurtain(AssetReferenceGameObject prefab)
        {
            if (LoadingCurtain != null)
            {
                LoadingCurtain.Show();
                
                return;
            }
            
            LoadingCurtain = (await CreateAddressableWithContainer(prefab,
                Vector3.zero, Quaternion.identity, null)).GetComponent<LoadingCurtain>();
            
            LoadingCurtain.Show();  
        }

        public async void CreateGameOverWindow(AssetReferenceGameObject prefab, Transform parent)
        {
            RectTransform window = (await CreateAddressableWithContainer(prefab,
                Vector3.zero, Quaternion.identity, parent)).GetComponent<RectTransform>();
            
            window.anchorMin = Vector2.zero;
            window.anchorMax = Vector2.one;
            window.sizeDelta = Vector2.zero;
            
            window.transform.localScale = Vector3.one;
            window.transform.localPosition = Vector3.zero;
            
            window.gameObject.SetActive(true);
        }
    }
}