using Scripts.States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Scripts.Managers
{
    public class GameStateManager : MonoBehaviour
    {

        private GameState _currentState;

        public float TimeLeft { get; set; }
        public int Points { get; set; }
        
        public BallController mBasketball;
        public GameObject mTarget;
        
        public Collider mTargetZone;

        public InputAction mFireAction;
        public InputAction mLookAction;
        
        void Start()
        {
            _currentState = new InitializeState(this);
            _currentState.Enter();
        }

        void Update()
        {
            _currentState.Update();
        }

        public void SwitchState(GameState state)
        {
            _currentState = state;
            _currentState.Enter();
        }
        
    }
}