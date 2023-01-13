using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Paridot
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputReader _input;

        [SerializeField] private float _transitionTime;
        private float _transitioning = 0f;
        private bool _isTransitioning = false;

        private GameState _gameState;

        public static event Action<GameState, float> TransitionGameEvent;

        private void Start()
        {
            _gameState = GameState.Perspective;

            _input.TransitionEvent += TransitionState;
        }

        private void Update()
        {
            if (_isTransitioning)
            {
                if (_transitioning <= _transitionTime)
                {
                    _transitioning += Time.deltaTime;
                }
                else
                {
                    _isTransitioning = false;
                }
            }
        }

        private void TransitionState()
        {
            if (!_isTransitioning)
            {
                _isTransitioning = true;
                _gameState = 1 - _gameState;
                _transitioning = 0f;
                TransitionGameEvent?.Invoke(_gameState, _transitionTime);
            }
        }
    }
}
