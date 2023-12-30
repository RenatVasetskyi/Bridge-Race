using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture.Services.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Architecture.Services
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _resourceCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();
        
        public T Initialize<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public async Task<T> Load<T>(AssetReferenceGameObject assetReference) where T : Object
        {
            if (_resourceCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
            {
                if (completedHandle.IsValid())
                    return completedHandle.Result as T;       
            }
            
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);

            handle.Completed += operation =>
            {
                _resourceCache[assetReference.AssetGUID] = operation;
            };

            AddHandle(assetReference.AssetGUID, handle);
            
            return await handle.Task;
        }
        
        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandle in _handles.Values)
            {
                foreach (AsyncOperationHandle handle in resourceHandle)
                {
                    if (handle.IsValid())
                        Addressables.Release(handle);       
                }
            }
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandle))
            {
                resourceHandle = new List<AsyncOperationHandle>();

                _handles[key] = resourceHandle;
                
                resourceHandle.Add(handle);
            }
        }
    }
}