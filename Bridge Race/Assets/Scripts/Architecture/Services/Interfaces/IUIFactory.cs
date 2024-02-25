using UI.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Architecture.Services.Interfaces
{
    public interface IUIFactory
    {
        LoadingCurtain LoadingCurtain { get; }
        void CreateLoadingCurtain();
        void CreateFullScreenWindow(AssetReferenceGameObject prefab, Transform parent);
    }
}