using Scripts.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Scripts.States
{
    public class InitializeState : GameState
    {

        private const string BallAddress = "Ball";
        private const string TargetAddress = "Target";

        private bool _isBallDone;
        private bool _isTargetDone;
        
        public InitializeState(GameStateManager context) : base(context)
        {
        }
        
        public override void Enter()
        {
            Context.Points = 0;
            Context.TimeLeft = 24;
            UIManager.SetPoints(Context.Points);
            UIManager.SetShotClock(Context.TimeLeft);
            UIManager.SetStyleSheet(ThemeManager.ReturnStyleSheet());

            SetupInputActions();
            
            Addressables.InstantiateAsync(BallAddress).Completed += OnBallLoaded;
            Addressables.InstantiateAsync(TargetAddress).Completed += OnTargetLoaded;

            CameraController.SetCursorLock(true, false, CursorLockMode.Locked);
        }
        
        public override void Update()
        {
            if (_isBallDone && _isTargetDone)
                Context.SwitchState(new PlayState(Context));
        }

        public override void Exit()
        {
        }

        private void OnBallLoaded(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to load asset!");
                return;
            }

            Context.mBasketball = obj.Result.GetComponent<BallController>();
            Context.mFireAction.performed 
                += ctx => Context.mBasketball.LaunchBall(ctx);
            Context.mBasketball.TargetZone = Context.mTargetZone;
            _isBallDone = true;
        }

        private void OnTargetLoaded(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to load asset!");
                return;
            }

            Context.mTarget = obj.Result;
            ThemeManager.ApplyLogoTexture(Context.mTarget.GetComponent<Renderer>());
            _isTargetDone = true;
        }
        
        private void SetupInputActions()
        {
            Context.mFireAction = new InputAction(binding: "<Touchscreen>/primaryTouch/tap");
            Context.mFireAction.AddBinding(new InputBinding("<Mouse>/leftButton"));
            
            Context.mLookAction = new InputAction(binding: "<Pointer>/delta");
            Context.mPauseAction = new InputAction(binding: "<Keyboard>/escape");
            
            Context.mLookAction.performed += ctx => CameraController.Look(ctx);
            Context.mPauseAction.Enable();
            Context.mLookAction.Enable();
            Context.mFireAction.Enable();
        }
    }
}
