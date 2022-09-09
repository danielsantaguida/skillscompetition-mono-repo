using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class LightLoader : MonoBehaviour
    {

        private const string LightingSceneAddress = "LightingScene";
        private AsyncOperationHandle<SceneInstance> _loadedScene;

        void Start()
        {
            _loadedScene = Addressables.LoadSceneAsync(
                LightingSceneAddress, 
                loadMode: LoadSceneMode.Additive,
                activateOnLoad: true
                );

            _loadedScene.Completed += OnLightingLoaded;
        }

        private void OnLightingLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle)
        {
            SceneManager.SetActiveScene(asyncOperationHandle.Result.Scene);
        }

        private void OnDestroy()
        {
            Addressables.UnloadSceneAsync(_loadedScene);
        }
    }
}
