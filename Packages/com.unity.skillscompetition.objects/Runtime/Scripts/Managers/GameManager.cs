using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] 
        private float timeLeft;
        private int _mPoints;
        private bool _mGameOver;
        
        [SerializeField] private Collider mTargetZone;
        
        private const string BallReference = "Ball";
        private const string TargetReference = "Target";
        
        private AsyncOperationHandle<GameObject> _mBallReference;
        private AsyncOperationHandle<GameObject> _mTargetReference;

        
        private InputAction _mFireAction;
        private InputAction _mLookAction;
        
        private GameObject _mBasketball;
        private GameObject _mTarget;
        
        private const string HighScoreKey = "highScore";

        void Awake()
        {
            _mPoints = 0;
            _mGameOver = false;
            UIManager.SetPoints(_mPoints);
            UIManager.SetShotClock(timeLeft);
            UIManager.SetStyleSheet(ThemeManager.ReturnStyleSheet());

            SetupInputActions();
            
            Addressables.LoadAssetAsync<GameObject>(BallReference).Completed += OnBallLoaded;
            Addressables.LoadAssetAsync<GameObject>(TargetReference).Completed += OnTargetLoaded;
        }

        void Start()
        {
            CameraController.SetCursorLock(true, false, CursorLockMode.Locked);
        }

        void Update()
        {
            if (_mGameOver)
                return;
            
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
                EndGame();
            
            UIManager.SetShotClock(timeLeft);
        }

        private void OnBallLoaded(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to load asset!");
                return;
            }

            _mBallReference = obj;
            
            _mBasketball = Instantiate(obj.Result);
            _mBasketball.GetComponent<BallController>().OnScore += IncreaseScore;
            _mFireAction.performed += context => _mBasketball.GetComponent<BallController>().LaunchBall(context);
            _mBasketball.GetComponent<BallController>().TargetZone = mTargetZone;
        }

        private void OnTargetLoaded(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Failed to load asset!");
                return;
            }

            _mTargetReference = obj;
            _mTarget = Instantiate(obj.Result);

            ThemeManager.ApplyLogoTexture(_mTarget.GetComponent<Renderer>());
            
            RandomTargetPosition();
        }

        private void EndGame()
        {
            _mGameOver = true;
            
            if (!PlayerPrefs.HasKey(HighScoreKey) || _mPoints > PlayerPrefs.GetInt(HighScoreKey))
                PlayerPrefs.SetInt(HighScoreKey, _mPoints);
            
            CameraController.SetCursorLock(false, true, CursorLockMode.None);
            
            DisableInputActions();

            ThemeManager.ClearTheme();
            
            DestroyAssets();
            
            UIManager.DisplayEndUI(_mPoints, PlayerPrefs.GetInt(HighScoreKey));
        }

        private void IncreaseScore(bool hasBounced)
        {
            _mPoints += hasBounced ? 2 : 1;
            UIManager.SetPoints(_mPoints);
            RandomTargetPosition();
        }

        private void RandomTargetPosition()
        {
            var bounds = mTargetZone.bounds;

            var randomPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    Random.Range(bounds.min.y, bounds.max.y),
                    Random.Range(bounds.min.z, bounds.max.z)
                );
            
            _mTarget.transform.position = randomPosition;
        }

        private void SetupInputActions()
        {
            _mFireAction = new InputAction(binding: "<Mouse>/leftButton");
            _mLookAction = new InputAction(binding: "<Pointer>/delta");
            
            _mLookAction.performed += context => CameraController.Look(context);
            _mLookAction.Enable();
            _mFireAction.Enable();
        }

        private void DisableInputActions()
        {
            _mFireAction.Disable();
            _mLookAction.Disable();
        }

        private void DestroyAssets()
        {
            Destroy(_mBasketball);
            Destroy(_mTarget);
            
            Addressables.Release(_mBallReference);
            Addressables.Release(_mTargetReference);
        }
    }
}

