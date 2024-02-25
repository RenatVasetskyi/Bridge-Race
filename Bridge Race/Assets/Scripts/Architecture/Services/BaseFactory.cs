using System.Threading.Tasks;
using Architecture.Services.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Architecture.Services
{
    public class BaseFactory : IBaseFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;

        protected BaseFactory(DiContainer container, IAssetProvider assetProvider)
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
    }
}