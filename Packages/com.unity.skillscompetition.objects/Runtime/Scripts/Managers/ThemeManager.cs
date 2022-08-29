using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace Managers
{
    public class ThemeManager : MonoBehaviour
    {

        private ThemeData _themeData;
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private StyleSheet defaultSheet;
        private AsyncOperationHandle<ThemeData> _addressableObj;

        public static Action ClearTheme;
        public static Action<Renderer> ApplyLogoTexture;
        public static Func<StyleSheet> ReturnStyleSheet;

        void Start()
        {
            DontDestroyOnLoad(this);
            
            OptionsPage.OnThemeSave += LoadTheme;
            ClearTheme += OnClearTheme;
            ApplyLogoTexture += OnApplyLogoTexture;
            ReturnStyleSheet = GetStyleSheet;
            
            LoadDefaultTheme();
        }

        private void LoadTheme(string address)
        {
            Addressables.LoadAssetAsync<ThemeData>(address).Completed += OnThemeLoaded;
        }

        private void LoadDefaultTheme()
        {
            LoadTheme(PlayerPrefs.GetString(OptionsPage.PlayerPrefsThemeKey));
        }

        public Texture2D GetLogo()
        {
            return _themeData == null
                ? new Texture2D(1, 1)
                : _themeData.logo;
        }

        public Color GetColor()
        {
            return _themeData == null 
                ? new Color(255, 137, 0) 
                : _themeData.color;
        }

        public StyleSheet GetStyleSheet()
        {
            return _themeData == null
                ? defaultSheet
                : _themeData.styleSheet;
        }

        private void OnThemeLoaded(AsyncOperationHandle<ThemeData> obj)
        {
            if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.Log("Failed to load theme asset!");
                return;
            }

            if (_themeData != null)
            {
                uiDocument.rootVisualElement.styleSheets.Remove(GetStyleSheet());
                ReleaseAsset();
            }

            _addressableObj = obj;
            _themeData = obj.Result;
            
            uiDocument.rootVisualElement.styleSheets.Add(_themeData.styleSheet);
        }

        private void OnApplyLogoTexture(Renderer textureRenderer)
        {
            textureRenderer.material.mainTexture = GetLogo();
        }

        private void ReleaseAsset()
        {
            Addressables.Release(_addressableObj);
        }

        private void OnClearTheme()
        {
            OptionsPage.OnThemeSave -= LoadTheme;
            ClearTheme -= OnClearTheme;
            ReleaseAsset();
            Destroy(gameObject);
        }
    }
}
