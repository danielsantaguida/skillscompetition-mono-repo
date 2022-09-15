using System;
using Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.States
{
    public class PauseState : GameState
    {

        public static Action ResumeGame;
        
        public PauseState(GameStateManager context) : base(context)
        {
        }

        public override void Enter()
        {
            CameraController.SetCursorLock(false, true, CursorLockMode.None);
            ResumeGame += OnResumeGame;
            UIManager.DisplayPauseUI();
            UIManager.ResumeGame += OnResumeGame;
            Context.mPauseAction.performed += OnResumeGame;
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
            CameraController.SetCursorLock(true, false, CursorLockMode.Locked);
            UIManager.HidePauseUI();
            UIManager.ResumeGame -= OnResumeGame;
            Context.mPauseAction.performed -= OnResumeGame;
        }

        private void OnResumeGame(InputAction.CallbackContext ctx)
        {
            OnResumeGame();   
        }

        private void OnResumeGame()
        {
            Context.SwitchState(new PlayState(Context));
        }
    }
}
