using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Paridot
{
    public class Player : TransitionObject
    {
        [SerializeField] private InputReader _input;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpTimeLimit;
        [SerializeField] [Range(0,1)] private float airMoveScale;

        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private GameObject feet;
        [SerializeField] private float maxRaycastDistance;

        private Rigidbody _rb;
        
        private Vector3 _moveDirection;
        private bool _isJumping;
        private float _timeJumping;

        private void Start()
        {
            base.Start();
            _rb = GetComponent<Rigidbody>();
            
            _input.MoveEvent += HandleMove;
            _input.JumpEvent += HandleJump;
            _input.JumpCancelledEvent += HandleJumpCancelled;
        }
        

        private void FixedUpdate()
        {
            Move();
            Jump();
        }

        private void HandleMove(Vector2 dir)
        {
            
            if (_gameState == GameState.Perspective)
            {
                _moveDirection = new Vector3(dir.y, 0, -dir.x);
            }
            else
            {
                _moveDirection = new Vector3(dir.x, 0, 0);
            }
        }

        private void HandleJump()
        {
            print(IsGrounded());
            if (!IsGrounded())
            {
                return;
            }
            _isJumping = true;
            _timeJumping = 0;
        }

        private void HandleJumpCancelled()
        {
            _isJumping = false;
        }

        private void Move()
        {
            Vector3 movement = _moveDirection * (moveSpeed * Time.deltaTime);
            
            if (!IsGrounded())
            {
                Vector3 force = movement - _rb.velocity;
                force.y = 0;
                _rb.AddForce(force * airMoveScale);
            }
            else
            {
                movement.y = _rb.velocity.y;
                _rb.velocity = movement;
            }
        }

        private bool IsGrounded()
        {
            // return Physics.CheckSphere(transform.position, 1.1f, (int)groundLayer);
            return Physics.Raycast(feet.transform.position, Vector3.down, maxRaycastDistance, groundLayer);
        }

        private void Jump()
        {
            if (!_isJumping)
            {
                return;
            }

            if (_timeJumping >= jumpTimeLimit)
            {
                _isJumping = false;
                return;
            }

            _timeJumping += Time.deltaTime;

            if (IsGrounded())
            {
                _rb.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
            }
            else
            {
                _rb.AddForce(Vector3.up * jumpForce);
            }
            
        }

    }
}
