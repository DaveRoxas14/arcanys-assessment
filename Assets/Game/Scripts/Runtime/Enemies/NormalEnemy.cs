using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Runtime.Enemies
{
    public class NormalEnemy : EnemyBase
    {
        protected override void Attack()
        {
            Debug.Log($"[Enemy] {name} attacked the player! Removing {_damage} gems.");
            ScoringManager.Instance.RemoveScore(_damage);
        }
        
        protected override void Move()
        {
            base.Move();
            
            if (_isGrounded && Random.value < 0.01f)
            {
                _velocity.y = _jumpForce;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}