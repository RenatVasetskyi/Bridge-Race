using System.Threading.Tasks;
using UI.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Architecture.Services.Interfaces
{
    public interface IBaseFactory
    {
        T CreateBaseWithContainer<T>(string path) where T : Component;
        T CreateBaseWithContainer<T>(string path, Transform parent) where T : Component;
        T CreateBaseWithContainer<T>(string path, Vector3 at, Quaternion rotation, Transform parent) where T : Component;
        T CreateBaseWithContainer<T>(T prefab, Vector3 at, Quaternion rotation, Transform parent) where T : Component;
        GameObject CreateBaseWithContainer(GameObject prefab, Vector3 at, Quaternion rotation, Transform parent);
        T CreateBaseWithObject<T>(string path) where T : Component;
        GameObject CreateBaseWithContainer(string path, Transform parent);
        Task<GameObject> CreateAddressableWithContainer
            (AssetReferenceGameObject assetReference, Vector3 at, Quaternion rotation, Transform parent);
        Task<GameObject> CreateAddressableWithObject(AssetReferenceGameObject assetReference, Vector3 at,
            Quaternion rotation, Transform parent);
    }
}