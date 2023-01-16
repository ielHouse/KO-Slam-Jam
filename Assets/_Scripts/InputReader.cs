using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Paridot
{
    [CreateAssetMenu(menuName = "Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IMenuActions
    {
        private GameInput _gameInput;

        public event Action<Vector2> MoveEvent;
        public event Action JumpEvent;
        public event Action JumpCancelledEvent;
        public event Action TransitionEvent;
        public event Action PauseEvent;
        public event Action ResumeEvent;
        public event Action SmashEvent;
        public event Action SmashCancelledEvent;

        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                
                _gameInput.Gameplay.SetCallbacks(this);
                _gameInput.Menu.SetCallbacks(this);
                
                // _gameInput.Gameplay.Enable();
                _gameInput.Menu.Enable();
            }
        }

        private void OnDisable()
        {
            _gameInput.Gameplay.Disable();
            _gameInput.Menu.Disable();
        }

        public void ActivateGameplay()
        {
            _gameInput.Gameplay.Enable();
            _gameInput.Menu.Disable();
        }
        
        public void ActivateMenu()
        {
            _gameInput.Menu.Enable();
            _gameInput.Gameplay.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                JumpEvent?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                JumpCancelledEvent?.Invoke();
            }
        }

        public void OnTransition(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                TransitionEvent?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ActivateMenu();
                PauseEvent?.Invoke();
            }
        }

        public void OnSmash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                SmashEvent?.Invoke();
            }
            if (context.phase == InputActionPhase.Canceled)
            {
                SmashCancelledEvent?.Invoke();
            }
        }

        public void OnResume(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                ActivateGameplay();
                ResumeEvent?.Invoke();
            }
        }
    }
}
