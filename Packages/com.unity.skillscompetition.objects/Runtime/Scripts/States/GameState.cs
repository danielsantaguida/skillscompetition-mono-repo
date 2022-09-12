using System.Collections;
using System.Collections.Generic;
using Scripts.Managers;
using UnityEngine;

namespace Scripts.States
{

    public abstract class GameState
    {

        protected GameStateManager Context;

        protected GameState(GameStateManager context)
        {
            Context = context;
        }

        public abstract void Enter();
        public abstract void Update();
    }
}