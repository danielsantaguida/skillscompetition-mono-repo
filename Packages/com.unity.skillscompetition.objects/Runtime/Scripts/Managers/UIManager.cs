using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Managers
{


    public class UIManager : MonoBehaviour
    {
        private Label _mShotClock;
        private Label _mPointsLabel;
        private VisualElement _mEndScreen;

        [SerializeField] private UIDocument m_UIDocument;

        public static Action<int> SetPoints;
        public static Action<float> SetShotClock;
        public static Action<int, int> DisplayEndUI;
        public static Action<StyleSheet> SetStyleSheet;

        void Awake()
        {
            _mShotClock = m_UIDocument.rootVisualElement.Q<Label>("shot-clock");
            _mPointsLabel = m_UIDocument.rootVisualElement.Q<Label>("points-label");
            _mEndScreen = m_UIDocument.rootVisualElement.Q("end-page");
            
            _mEndScreen.Q<Button>("menu-button").RegisterCallback<ClickEvent>(ev => LoadMainMenu());

            SetPoints += OnSetPoints;
            SetShotClock += OnSetShotClock;
            DisplayEndUI += OnDisplayEndUI;
            SetStyleSheet += OnSetStyleSheet;
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
        }
    }
}
