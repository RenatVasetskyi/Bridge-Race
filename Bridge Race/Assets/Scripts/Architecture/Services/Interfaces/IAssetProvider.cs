using UnityEngine;

namespace Architecture.Services.Interfaces
{
    public interface IAssetProvider
    {
        T Initialize<T>(string path) where T : Object;
    }
}
