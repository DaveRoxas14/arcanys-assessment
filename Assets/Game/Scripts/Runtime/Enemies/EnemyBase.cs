using System;
using System.Threading.Tasks;
using Game.Scripts.Runtime.Audio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Runtime.Enemies
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class EnemyBase : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.REFERENCES)]
        [SerializeField] protected Animator _animator;

        [SerializeField] protected SoundEffect _hitSfx;
        
        [Header(ArcanysConstants.INSPECTOR.MOVEMENT)] 
        [SerializeField] protected float _moveSpeed = 3f;
        [SerializeField] protected float _chaseRange = 10f;
        [SerializeField] protected float _attackRange = 10f;
        [SerializeField] protected float _jumpForce = 5f;
        [SerializeField] protected float _gravity = -9.8f;
        [SerializeField] protected float _airControlMultiplier = 0.5f;
        [SerializeField] protected int _deathDelay = 1;

        [Header(ArcanysConstants.INSPECTOR.ATTACK)] 
        [SerializeField] protected int _damage = 10;
        [SerializeField] protected float _attackCooldown = 1.5f;
        
        protected Transform _player;
        protected CharacterController _controller;
        protected Vector3 _velocity;
        protected bool _isGrounded;
        protected bool _isAttacking;
        protected float _lastAttackTime;
        protected bool _isDead;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _player = GameManager.Instance.GetPlayer().transform;
            
            _animator.SetBool(ArcanysConstants.ANIMATIONS.RUN, true);
        }
        
        protected virtual void Update()
        {
            if (!_player || _isDead) return;

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
                AudioManager.Instance.PlaySFX(_hitSfx.clip);
                var rand = Random.Range(0, 3);
                {
                    switch (rand)
                    {
                        case 0:
                            _animator.SetTrigger(ArcanysConstants.ANIMATIONS.ATTACK_1);
                            break;
                        case 1:
                            _animator.SetTrigger(ArcanysConstants.ANIMATIONS.ATTACK_2);
                            break;
                        case 2:
                            _animator.SetTrigger(ArcanysConstants.ANIMATIONS.ATTACK_3);
                            break;
                    }
                }
                _lastAttackTime = Time.time;
            }
        }

        public async void KillEnemy()
        {
            try
            {
                _animator.SetTrigger(ArcanysConstants.ANIMATIONS.DIE);
                _isDead = true;

                await Task.Delay(_deathDelay * ArcanysConstants.INTEGERS.MILLISECOND);
                
                Destroy(gameObject);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        protected abstract void Attack();
    }
}