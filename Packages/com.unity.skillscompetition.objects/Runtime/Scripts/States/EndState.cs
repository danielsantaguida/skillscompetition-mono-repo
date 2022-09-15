using Scripts.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace Scripts.States
{
    public class EndState : GameState
    {
        
        private const string HighScoreKey = "highScore";

        public EndState(GameStateManager context) : base(context)
        {
        }
        
        public override void Enter()
        {
            if (!PlayerPrefs.HasKey(HighScoreKey) || Context.Points > PlayerPrefs.GetInt(HighScoreKey))
                PlayerPrefs.SetInt(HighScoreKey, Context.Points);
            
            CameraController.SetCursorLock(false, true, CursorLockMode.None);
            
            ThemeManager.ClearTheme();
            
            DestroyAssets();
            
            UIManager.DisplayEndUI(Context.Points, PlayerPrefs.GetInt(HighScoreKey));
        }
        
        public override void Update()
        {
        }

        public override void Exit()
        {
        }

        private void DestroyAssets()
        {
            Addressables.ReleaseInstance(Context.mBasketball.gameObject);
            Addressables.ReleaseInstance(Context.mTarget);
        }
        
    }
}
