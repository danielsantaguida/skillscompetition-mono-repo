using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        private Label _mShotClock;
        private Label _mPointsLabel;
        private VisualElement _mEndScreen;
        private VisualElement _pauseScreen;

        [SerializeField] private UIDocument m_UIDocument;

        public static Action<int> SetPoints;
        public static Action<float> SetShotClock;
        public static Action<int, int> DisplayEndUI;
        public static Action<StyleSheet> SetStyleSheet;
        public static Action DisplayPauseUI;
        public static Action HidePauseUI;
        public static Action ResumeGame;
#if PLATFORM_ANDROID
        public static Action PauseGame;

#endif

        void Awake()
        {
            _mShotClock = m_UIDocument.rootVisualElement.Q<Label>("shot-clock");
            _mPointsLabel = m_UIDocument.rootVisualElement.Q<Label>("points-label");
            _mEndScreen = m_UIDocument.rootVisualElement.Q("end-page");
            _pauseScreen = m_UIDocument.rootVisualElement.Q("pause-screen");

#if PLATFORM_ANDROID
            var placeholder = m_UIDocument.rootVisualElement.Q("placeholder");
            placeholder.style.display = DisplayStyle.None;
                
            var pauseButton = m_UIDocument.rootVisualElement.Q("pause-button");
            pauseButton.RegisterCallback<ClickEvent>(ev => PauseGame());
#else
            var pauseContainer = m_UIDocument.rootVisualElement.Q("pause-container");
            pauseContainer.style.display = DisplayStyle.None;
#endif
            
            _mEndScreen.Q<Button>("menu-button").RegisterCallback<ClickEvent>(ev => LoadMainMenu());
            _pauseScreen.Q<Button>("resume-button").RegisterCallback<ClickEvent>(ev => ResumeGame());

            SetPoints += OnSetPoints;
            SetShotClock += OnSetShotClock;
            DisplayEndUI += OnDisplayEndUI;
            SetStyleSheet += OnSetStyleSheet;
            DisplayPauseUI += OnDisplayPauseUI;
            HidePauseUI += OnHidePauseUI;
        }

        private void OnSetShotClock(float time)
        {
            var roundModifier = (time >= 10f) ? 0 : 1;
            
            _mShotClock.text = Math.Round(time, roundModifier).ToString(new CultureInfo("en-US"));
        }

        private void OnSetPoints(int points)
        {
            _mPointsLabel.text = "Points: " + points;
        }

        private void OnDisplayEndUI(int points, int highScore)
        {
            _mShotClock.text = "0";
            
            _mEndScreen.style.display = DisplayStyle.Flex;
            _mEndScreen.Q<Label>("high-score-label").text = "High Score: " + highScore;
            _mEndScreen.Q<Label>("player-score-label").text = "Your Score: " + points;
        }

        private void OnDisplayPauseUI()
        {
            _pauseScreen.style.display = DisplayStyle.Flex;
        }
        
        private void OnHidePauseUI()
        {
            _pauseScreen.style.display = DisplayStyle.None;
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadSceneAsync("Menu");
        }

        private void OnSetStyleSheet(StyleSheet styleSheet)
        {
            m_UIDocument.rootVisualElement.styleSheets.Add(styleSheet);
        }

        private void OnDestroy()
        {
            SetPoints -= OnSetPoints;
            SetShotClock -= OnSetShotClock;
            DisplayEndUI -= OnDisplayEndUI;
            SetStyleSheet -= OnSetStyleSheet;
            DisplayPauseUI -= OnDisplayPauseUI;
            HidePauseUI -= OnHidePauseUI;
        }
    }
}
