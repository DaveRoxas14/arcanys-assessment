using System;
using UnityEngine;

namespace Game.Scripts.Runtime.Enemies
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.MOVEMENT)] 
        [SerializeField] protected float _moveSpeed = 3f;
        [SerializeField] protected float _chaseRange = 10f;
        [SerializeField] protected float _attackRange = 10f;
        [SerializeField] protected float _jumpForce = 5f;
        [SerializeField] protected float _gravity = -9.8f;
        [SerializeField] protected float _airControlMultiplier = 0.5f;

        [Header(ArcanysConstants.INSPECTOR.ATTACK)] 
        [SerializeField] protected int _damage = 10;
        [SerializeField] protected float _attackCooldown = 1.5f;
        
        protected Transform _player;
        protected CharacterController _controller;
        protected Vector3 _velocity;
        protected bool _isGrounded;
        protected bool _isAttacking;
        protected float _lastAttackTime;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _player = GameManager.Instance.GetPlayer().transform;
        }
        
        protected virtual void Update()
        {
            if (!_player) return;

            Move();
            AttackWhenInDistance();
        }


        protected virtual void Move()
        {
            _isGrounded = _controller.isGrounded;
            if (_isGrounded && _velocity.y < 0) _velocity.y = -2f;

            var distance = Vector3.Distance(transform.position, _player.position);

            if (distance <= _chaseRange)
            {
                var dir = (_player.position - transform.position).normalized;
                dir.y = 0;

                // This will rotate towards the player
                if (dir.magnitude > 0.1f)
                {
                    var lookRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
                }

                // This will move towards the player
                var move = dir * _moveSpeed;
                _controller.Move(move * Time.deltaTime);
            }

            // Jumping logic
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        protected virtual void AttackWhenInDistance()
        {
            var distance = Vector3.Distance(transform.position, _player.position);
            if (distance <= _attackRange && Time.time - _lastAttackTime >= _attackCooldown)
            {
                Attack();
                _lastAttackTime = Time.time;
            }
        }

        protected abstract void Attack();
    }
}