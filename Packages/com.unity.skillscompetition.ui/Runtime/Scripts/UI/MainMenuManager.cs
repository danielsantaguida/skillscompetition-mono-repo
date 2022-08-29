using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuManager : VisualElement
    {
        private VisualElement m_StartPage;
        private VisualElement m_OptionsPage;
        
        public new class UxmlFactory : UxmlFactory<MainMenuManager, UxmlTraits> { }

        public MainMenuManager() 
        { 
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }
        
        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            m_StartPage = this.Q("start-page");
            m_OptionsPage = this.Q("options-page");
            
            //Setup title screen buttons
            m_StartPage?.Q("start-button").RegisterCallback<ClickEvent>(ev => StartGame());
            m_StartPage?.Q("options-button").RegisterCallback<ClickEvent>(ev => EnableOptionsPage());
            m_StartPage?.Q("exit-button").RegisterCallback<ClickEvent>(ev => ExitGame());
            
            this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void EnableOptionsPage()
        {
            m_OptionsPage.style.display = DisplayStyle.Flex;
        }

        private void StartGame()
        {
            SceneManager.LoadSceneAsync("PassingChallenge");
        }

        private void ExitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
