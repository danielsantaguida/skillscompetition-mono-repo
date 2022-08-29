using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Scripts.UI
{

    public class OptionsPage : VisualElement
    {

        public const string PlayerPrefsVolumeKey = "volume";
        public const string PlayerPrefsSensitivityKey = "sensitivity";
        public const string PlayerPrefsQualityKey = "quality";
        public const string PlayerPrefsThemeKey = "theme";
        
        private VisualElement _mSaveButton;
        private SliderInt _mVolumeSlider;
        private SliderInt _mSensitivitySlider;
        private DropdownField _mQualityDropdown;
        private DropdownField _mThemeDropdown;

        private readonly List<string> _mThemeChoices;

        public static Action<string> OnThemeSave;
        
        public new class UxmlFactory : UxmlFactory<OptionsPage, UxmlTraits> { }

        public OptionsPage()
        {

            _mThemeChoices = new List<string>()
            {
                "Default",
                "Raptors",
                "Mavericks",
                "Heat"
            };

            DefaultPrefs();
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            _mSaveButton = this.Q("save-button");
            _mVolumeSlider = this.Q<SliderInt>("volume-slider");
            _mSensitivitySlider = this.Q<SliderInt>("sensitivity-slider");
            _mQualityDropdown = this.Q<DropdownField>("quality-dropdown");
            _mThemeDropdown = this.Q<DropdownField>("theme-dropdown");

            SetOptions();

            _mSaveButton?.RegisterCallback<ClickEvent>(ev => SaveOptions());
        }

        private void DefaultPrefs()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsVolumeKey))
                PlayerPrefs.SetInt(PlayerPrefsVolumeKey, 50);
            
            if (!PlayerPrefs.HasKey(PlayerPrefsSensitivityKey))
                PlayerPrefs.SetInt(PlayerPrefsSensitivityKey, 5);
            
            if (!PlayerPrefs.HasKey(PlayerPrefsQualityKey))
                PlayerPrefs.SetString(PlayerPrefsQualityKey, QualitySettings.names[1]);
            
            if (!PlayerPrefs.HasKey(PlayerPrefsThemeKey))
                PlayerPrefs.SetString(PlayerPrefsThemeKey, _mThemeChoices[0]);
            
            PlayerPrefs.Save();
        }
        
        private void SaveOptions()
        {
            PlayerPrefs.SetInt(PlayerPrefsVolumeKey, _mVolumeSlider.value);
            PlayerPrefs.SetInt(PlayerPrefsSensitivityKey, _mSensitivitySlider.value);
            PlayerPrefs.SetString(PlayerPrefsQualityKey, _mQualityDropdown.value);
            PlayerPrefs.SetString(PlayerPrefsThemeKey, _mThemeDropdown.value);

            LoadQualityAsset(_mQualityDropdown.value);

            OnThemeSave(_mThemeDropdown.value);
            
            PlayerPrefs.Save();
            
            parent.style.display = DisplayStyle.None;
        }

        private void SetOptions()
        {
            //Volume slider
            _mVolumeSlider.highValue = 100;
            _mVolumeSlider.value = PlayerPrefs.GetInt(PlayerPrefsVolumeKey);
            
            //Sensitivity slider
            _mSensitivitySlider.highValue = 10;
            _mSensitivitySlider.value = PlayerPrefs.GetInt(PlayerPrefsSensitivityKey);
            
            //Quality options
            _mQualityDropdown.choices = QualitySettings.names.ToList();
            _mQualityDropdown.value = PlayerPrefs.GetString(PlayerPrefsQualityKey);
            
            //Theme options
            _mThemeDropdown.choices = _mThemeChoices;
            _mThemeDropdown.value = PlayerPrefs.GetString(PlayerPrefsThemeKey);
        }

        private void LoadQualityAsset(string qualitySetting)
        {
            // Addressables.LoadAssetAsync<UniversalRenderPipelineAsset>(qualitySetting).Completed += SetQuality;

            QualitySettings.SetQualityLevel(QualitySettings.names.ToList().IndexOf(qualitySetting));
        }

        private void SetQuality(AsyncOperationHandle<RenderPipelineAsset> obj)
        {
            if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to load quality asset!");
                return;
            }

            QualitySettings.renderPipeline = obj.Result;
            Addressables.Release(obj);
        }

    }
}