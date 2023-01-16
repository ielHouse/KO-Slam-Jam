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

        [SerializeField] private Animator _anim;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpTimeLimit;
        [SerializeField] [Range(0,1)] private float airMoveScale;

        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private GameObject feet;
        [SerializeField] private float maxRaycastDistance;

        [SerializeField] private float _smashRadius;
        [SerializeField] private LayerMask _computer;

        public bool slamming = false;

        public static event Action SmashedLike;
        
        

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
            _input.SmashEvent += HandleSmash;
            _input.SmashCancelledEvent += HandleSmashCancelled;

            GameManager.PlayerDeathEvent += HandleDeath;
            GameManager.RestartEvent += HandleRestart;
        }

        private void HandleSmashCancelled()
        {
            slamming = false;
        }

        private void HandleSmash()
        {
            _anim.SetBool("SMASH", true);
            slamming = true;
            
        }

        private void HandleRestart()
        {
            _anim.SetBool("OoT", false);
            
        }

        private void HandleDeath()
        {
            _anim.SetBool("OoT", true);
        }


        private void FixedUpdate()
        {
            Move();
            Jump();
        }

        private void Update()
        {
            if (slamming)
            {
                if (Physics.CheckSphere(transform.position, _smashRadius, _computer))
                {
                    SmashedLike?.Invoke();
                }
            }
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
            if (!IsGrounded())
            {
                return;
            }

            _anim.SetBool("jump", true);
            _isJumping = true;
            _timeJumping = 0;
        }

        private void HandleJumpCancelled()
        {
            _isJumping = false;
            
            _anim.SetBool("jump", false);
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
                if (Vector3.Dot(_rb.velocity, Vector3.right)>0)
                {
                    _anim.SetBool("Level_Start", true);
                    _anim.SetBool("beepbeepbackupsounds", false);
                }
                else if (Vector3.Dot(_rb.velocity, Vector3.right)<0)
                {
                    _anim.SetBool("Level_Start", false);
                    _anim.SetBool("beepbeepbackupsounds", true);
                }
                else
                {
                    _anim.SetBool("Level_Start", false);
                    _anim.SetBool("beepbeepbackupsounds", false);
                }
                
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
                _anim.SetBool("jump", false);
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
