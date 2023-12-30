using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Architecture.Services.Interfaces
{
    public interface IAssetProvider
    {
        T Initialize<T>(string path) where T : Object;
        Task<T> Load<T>(AssetReferenceGameObject assetReference) where T : Object;
        void CleanUp();
    }
}
