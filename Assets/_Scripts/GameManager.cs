using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Paridot
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _UIGameplay;
        [SerializeField] private GameObject _UIMenu;
        [SerializeField] private TextMeshProUGUI _timerText;
        
        [SerializeField] private GameObject _player;
        private Vector3 _startPosition;
        [SerializeField] private float _deathZoneY;
        
        [SerializeField] private InputReader _input;

        [SerializeField] private float _transitionTime;
        private float _transitioning = 0f;
        private bool _isTransitioning = false;

        private GameState _gameState;

        private float _timer;
        
        public static event Action<GameState, float, float> TransitionGameEvent;
        public static event Action PlayerDeathEvent;
        

        private void Start()
        {
            _gameState = GameState.Perspective;
            
            HandlePause();
            _timer = 120f;

            _input.TransitionEvent += TransitionState;
            _input.PauseEvent += HandlePause;
            _input.ResumeEvent += HandleResume;

            _startPosition = _player.transform.position;
        }

        public void RestartGame()
        {
            _timer = 120f;
            HandleResume();
            PlayerDeath();
            _input.ActivateGameplay();
        }

        private void UpdateTimer()
        {
            _timer -= Time.deltaTime;
            _timerText.text = ((int)_timer).ToString();
        }

        private void HandleResume()
        {
            Time.timeScale = 1f;

            _UIGameplay.SetActive(true);
            _UIMenu.SetActive(false);
        }

        private void HandlePause()
        {
            Time.timeScale = 0f;

            // _UIGameplay.SetActive(false);
            _UIMenu.SetActive(true);
        }

        private void PlayerDeath()
        {
            PlayerDeathEvent?.Invoke();
            _startPosition.z = _player.transform.position.z;
            _player.transform.position = _startPosition;
        }

        private void Update()
        {
            UpdateTimer();
            if (_player.transform.position.y < _deathZoneY)
            {
                PlayerDeath();
            }
            
            if (_isTransitioning)
            {
                if (_transitioning <= _transitionTime)
                {
                    _transitioning += Time.unscaledDeltaTime;
                }
                else
                {
                    _isTransitioning = false;
                    Time.timeScale = 1;
                }
            }
        }

        private void TransitionState()
        {
            if (!_isTransitioning)
            {
                _isTransitioning = true;
                Time.timeScale = 0;
                _gameState = 1 - _gameState;
                _transitioning = 0f;
                TransitionGameEvent?.Invoke(_gameState, _transitionTime, _player.transform.position.z);
            }
        }
    }
}
