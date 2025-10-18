using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Runtime.Enemies
{
    /// <summary>
    /// This spawns the enemies randomly.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EnemyBase _enemyPrefab;
        
        [Header("Settings")]
        [SerializeField] private int _numberOfEnemiesToSpawn = 5;
        [SerializeField] private float _spawnRadius = 10f;
        [SerializeField] private int _spawnDelay = 3;

        private async void Start()
        {
            try
            {
                for (var i = 0; i < _numberOfEnemiesToSpawn; i++)
                {
                    var delay = _spawnDelay * 1000;
                    await Task.Delay(delay);
                    
                    if (!transform) break;

                    var pos = transform.position + Random.insideUnitSphere * _spawnRadius;
                    pos.y = transform.position.y;
                    Instantiate(_enemyPrefab, pos, Quaternion.identity);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Gizmos.DrawSphere(transform.position, _spawnRadius);
        }
    }
}