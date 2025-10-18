using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Runtime.Enemies
{
    /// <summary>
    /// This spawns the enemies randomly.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemyBase _enemyPrefab;
        [SerializeField] private int _numberOfEnemiesToSpawn = 5;
        [SerializeField] private float _spawnRadius = 10f;

        private void Start()
        {
            for (var i = 0; i < _numberOfEnemiesToSpawn; i++)
            {
                var pos = transform.position + Random.insideUnitSphere * _spawnRadius;
                pos.y = transform.position.y;
                Instantiate(_enemyPrefab, pos, Quaternion.identity);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Gizmos.DrawSphere(transform.position, _spawnRadius);
        }
    }
}