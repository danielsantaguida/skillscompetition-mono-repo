using System;
using Scripts.Managers;
using UnityEngine;

namespace Scripts.States
{
    public class CutsceneState : GameState
    {
        public CutsceneState(GameStateManager context) : base(context)
        {
        }

        public override void Enter()
        {
        }

        public override void Update()
        {
            if (Math.Abs(Context.dollyCart.m_Position - Context.dollyCart.m_Path.PathLength) <= 0.2f) 
                Context.SwitchState(new PlayState(Context));
            
        }

        public override void Exit()
        {
            Context.trackVirtualCamera.gameObject.SetActive(false);
        }
    }
}
