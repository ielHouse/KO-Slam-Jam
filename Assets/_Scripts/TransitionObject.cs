using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paridot
{
    public class TransitionObject : MonoBehaviour
    {
        protected GameState _gameState { get; private set; }

        protected void Start()
        {
            GameManager.TransitionGameEvent += HandleTransition;
        }

        private void HandleTransition(GameState state, float time)
        {
            _gameState = state;
            StartCoroutine(Transition(state, time));
        }

        private IEnumerator Transition(GameState state, float time)
        {
            float timeElapsed = 0f;

            while (timeElapsed <= time)
            {
                float t = timeElapsed / time;
                PerformTransition(t);
                timeElapsed += Time.unscaledDeltaTime;

                yield return null;
            }

            PerformTransition(1);
            yield return null;
        }

        protected virtual void PerformTransition(float t)
        {
            
        }


        private void OnDisable()
        {
            GameManager.TransitionGameEvent -= HandleTransition;
        }
    }
}
