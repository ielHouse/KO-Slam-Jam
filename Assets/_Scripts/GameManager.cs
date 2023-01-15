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
        [SerializeField] private Animator _computerAnim;
        private Vector3 _startPosition;
        [SerializeField] private float _deathZoneY;
        
        [SerializeField] private InputReader _input;

        [SerializeField] private float _transitionTime;

        [SerializeField] private List<ParticleSystem> _winParticles;

        private float _transitioning = 0f;
        private bool _isTransitioning = false;

        private bool _playerDead = false;
        private bool _won = false;

        private GameState _gameState;

        private float _timer;
        
        public static event Action<GameState, float, float> TransitionGameEvent;
        public static event Action PlayerDeathEvent;
        public static event Action RestartEvent;
        

        private void CheckTimer()
        {
            if (_timer <= 0)
            {
                KillPlayer();
            }
        }
        
        private void Start()
        {
            _gameState = GameState.Perspective;
            
            HandlePause();
            _timer = 120f;

            _input.TransitionEvent += TransitionState;
            _input.PauseEvent += HandlePause;
            _input.ResumeEvent += HandleResume;

            Player.SmashedLike += HandleSmashedLike;

            _startPosition = _player.transform.position;
        }

        private void HandleSmashedLike()
        {
            _won = true;
            _computerAnim.SetBool("SPEED", true);
            foreach (var particles in _winParticles)
            {
                particles.Play();
            }
        }

        public void RestartGame()
        {
            RestartEvent?.Invoke();
            foreach (var particles in _winParticles)
            {
                particles.Stop();
            }
            _playerDead = false;
            _won = false;
            _computerAnim.SetBool("SPEED", false);
            _timer = 120f;
            HandleResume();
            _input.ActivateGameplay();
        }

        private void UpdateTimer()
        {
            if (!_won)
            {
                _timer -= Time.deltaTime;
            }
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
            _input.ActivateMenu();
        }

        private IEnumerator PlayerDeath()
        {
            PlayerDeathEvent?.Invoke();
            
            yield return new WaitForSeconds(5);
            
            if (_gameState == GameState.Side)
            {
                TransitionState();
            }

            HandlePause();
            _player.transform.position = _startPosition;
        }

        private void KillPlayer()
        {
            _playerDead = true;
            StartCoroutine(PlayerDeath());
        }

        private void Update()
        {
            UpdateTimer();

            if (!_playerDead)
            {
                CheckTimer();
            }
            
            if (_player.transform.position.y < _deathZoneY && !_playerDead)
            {
                KillPlayer();
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
