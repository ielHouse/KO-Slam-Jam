using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Paridot
{
    [CreateAssetMenu(menuName = "Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions
    {
        private GameInput _gameInput;

        public event Action<Vector2> MoveEvent;
        public event Action JumpEvent;
        public event Action JumpCancelledEvent;
        public event Action TransitionEvent;

        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                
                _gameInput.Gameplay.SetCallbacks(this);
                
                _gameInput.Gameplay.Enable();
            }
        }

        private void OnDisable()
        {
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
    }
}
