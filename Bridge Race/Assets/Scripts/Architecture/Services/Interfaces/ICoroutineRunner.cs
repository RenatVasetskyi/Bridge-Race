using System.Collections;
using UnityEngine;

namespace Architecture.Services.Interfaces
{
    public interface ICoroutineRunner 
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
    }
}
