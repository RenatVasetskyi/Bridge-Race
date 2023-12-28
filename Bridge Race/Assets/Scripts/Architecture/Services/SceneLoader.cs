using System;
using System.Collections;
using Architecture.Services.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Architecture.Services
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        
        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string nextScene, Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(nextScene, onLoaded));
        }

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();   
        }
    }
}