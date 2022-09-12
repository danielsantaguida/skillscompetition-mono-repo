using Scripts.Managers;
using UnityEngine;

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

            _targetZoneBounds = Context.mTargetZone.bounds;
            RandomTargetPosition();
        }
        
        public override void Update()
        {
            Context.TimeLeft -= Time.deltaTime;

            if (Context.TimeLeft <= 0)
                Context.SwitchState(new EndState(Context));
            
            UIManager.SetShotClock(Context.TimeLeft);
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
