using System;
using Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Scripts.States
{
    public class PlayState : GameState
    {
        private Bounds _targetZoneBounds;
        
        public PlayState(GameStateManager context) : base(context)
        {
        }

        public override void Enter()
        {
            Context.mBasketball.OnScore += IncreaseScore;
            Context.mPauseAction.performed += PauseGame;
#if PLATFORM_ANDROID
            UIManager.PauseGame += PauseGame;
#endif
            _targetZoneBounds = Context.mTargetZone.bounds;
            
            Context.mPauseAction.Enable();
            Context.mLookAction.Enable();
            Context.mFireAction.Enable();
        }
        
        public override void Update()
        {
            Context.TimeLeft -= Time.deltaTime;

            if (Context.TimeLeft <= 0)
                Context.SwitchState(new EndState(Context));
            
            UIManager.SetShotClock(Context.TimeLeft);
        }

        public override void Exit()
        {
            Context.mLookAction.Disable();
            Context.mFireAction.Disable();
            
            Context.mPauseAction.performed -= PauseGame;
#if PLATFORM_ANDROID
            UIManager.PauseGame -= PauseGame;
#endif
        }

        private void PauseGame(InputAction.CallbackContext ctx)
        {
            PauseGame();
        }

        private void PauseGame()
        {
            Context.SwitchState(new PauseState(Context));

        }

        private void IncreaseScore(bool hasBounced)
        {
            Context.Points += hasBounced ? 2 : 1;
            UIManager.SetPoints(Context.Points);
            RandomTargetPosition();
        }

        private void RandomTargetPosition()
        {
            var randomPosition = new Vector3(
                Random.Range(_targetZoneBounds.min.x, _targetZoneBounds.max.x),
                Random.Range(_targetZoneBounds.min.y, _targetZoneBounds.max.y),
                Random.Range(_targetZoneBounds.min.z, _targetZoneBounds.max.z)
            );
            
            Context.mTarget.transform.position = randomPosition;
        }
    }
}
