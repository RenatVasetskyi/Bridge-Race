using System;

namespace Architecture.Services.Interfaces
{
    public interface ISceneLoader
    {
        void Load(string nextScene, Action onLoaded = null);
    }
}


